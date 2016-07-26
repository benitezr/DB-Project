using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables.Core;

namespace DeliApp
{
    public class OrderMenu : Menu
    {
        private int customerID, orderLineCount, orderID;
        private decimal orderTotal, moneySaved;
        private string customerName;
        private bool loyal, completed;

        public OrderMenu(string newTitle, MenuManager newManager, int newID, string newName, bool isLoyal)
            : base(newTitle, newManager)
        {
            customerID = newID;
            customerName = newName;
            orderID = 0;
            orderLineCount = 1;
            orderTotal = moneySaved = 0;
            loyal = isLoyal;
            completed = false;
        }

        public override void Display()
        {
            bool quit = false, display = true;
            int option;

            do
            {
                if (display)
                {
                    NewTitle(title);
                    Console.WriteLine("Customer: {0}\n", customerName);
                    Console.WriteLine("[1] Standard Order\n[2] Custom Order\n[3] Complete Order\n[4] Back");
                }
                
                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out option)) { option = 0; }

                switch (option)
                {
                    case 1:
                        InsertOrder();
                        display = true;
                        break;
                    case 2:
                        InsertCustomOrder();
                        display = true;
                        break;
                    case 3:
                        if (orderLineCount > 1) { CompleteOrder(); quit = true; break; }
                        Console.WriteLine("\nMust create an order before completing");
                        display = false;
                        break;
                    case 4:
                        if (!completed && orderLineCount > 1) { Console.WriteLine("\nMust complete the order before exiting"); display = false; break; }
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("\nInput invalid");
                        display = false;
                        break;
                }
            } while (!quit);
            menuMng.RemoveMenu();
        }

        private void PrintTotal()
        {
            StringBuilder builder = new StringBuilder();
            string word = String.Format("Total: ${0}", orderTotal.ToString("0.00"));
            builder.AppendLine(" " + new string('-', word.Length + 4));
            builder.AppendLine(String.Format(" | {0} |", word));
            builder.AppendLine(" " + new string('-', word.Length + 4));
            Console.WriteLine(builder.ToString());
        }

        private void CompleteOrder()
        {
            Console.Clear();
            using (var db = new AppContext())
            {
                ConsoleTable table = new ConsoleTable("Line Number", "Menu Item", "Quantity");

                var orderLines = from o in db.ORDER_LINES
                                 where o.ORDER_ID == orderID
                                 select o;

                var loyalCustomer = db.LOYAL_CUSTOMERS.SingleOrDefault(x => x.CUS_ID == customerID);
                                
                foreach (var line in orderLines)
                {
                    var menuItem = db.MENU_OPTIONS.Single(x => x.MENU_ID == line.MENU_ID);
                    table.AddRow(line.LINE_NUM, menuItem.MENU_NAME, line.LINE_QUANTITY);
                    if (loyalCustomer != null && loyalCustomer.LCUS_POINTS >= 30 && menuItem.MENU_NAME.Equals("Sandwich"))
                    {
                        loyalCustomer.LCUS_POINTS -= 30;
                        moneySaved = menuItem.MENU_PRICE;
                        orderTotal -= moneySaved;                                              
                    }
                }
                db.SaveChanges();
                table.Write();
            }
            PrintTotal();
            if (moneySaved > 0) { Console.WriteLine("Your loyalty points saved you ${0}!", moneySaved.ToString("0.00")); }
            completed = true;
            Console.ReadKey();            
        }

        private void InsertOrder()
        {
            List<string> fields = new List<string> { "Item Name: ", "Order Quantity: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Input Order", fields);
                if (inputs != null)
                {
                    ExecInsertOrder(inputs.ToArray());
                    inputs.Clear();
                }
                else
                    break;
            }
        }

        private void ExecInsertOrder(string[] values)
        {
            string itemName = values[0];
            int orderQuantity;
            if (!int.TryParse(values[1], out orderQuantity)) { Console.WriteLine("\nInvalid quantity inserted"); return; }

            using (var db = new AppContext())
            {
                if (orderLineCount <= 1)
                {
                    var order = new ORDER{ CUS_ID = customerID, ORDER_DATE = DateTime.UtcNow };
                    db.ORDERS.Add(order);
                    db.SaveChanges();
                    orderID = order.ORDER_ID;
                }

                var menuOption = db.MENU_OPTIONS.SingleOrDefault(x => x.MENU_NAME.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                if (menuOption == null)
                {
                    var sandwich = db.SANDWICHES.FirstOrDefault(x => x.SAND_NAME.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                    if (sandwich != null)
                    {
                        menuOption = db.MENU_OPTIONS.SingleOrDefault(x => x.MENU_ID == sandwich.MENU_ID);

                        for (int i = 0; i < orderQuantity; i++)
                        {
                            var accountTrans = new ACCOUNT
                            {
                                SAND_ID = sandwich.SAND_ID,
                                ACC_REVENUE = menuOption.MENU_PRICE,
                                ACC_COST = menuOption.MENU_COST,
                                ACC_PROFIT = menuOption.MENU_PRICE - menuOption.MENU_COST,
                                ACC_DATE = DateTime.UtcNow
                            };
                            db.ACCOUNTS.Add(accountTrans);
                            db.SaveChanges();
                        }                        
                    }
                    else { return; }
                }

                var orderLine = new ORDER_LINES
                {
                    LINE_NUM = orderLineCount,
                    ORDER_ID = orderID,
                    MENU_ID = menuOption.MENU_ID,
                    LINE_QUANTITY = orderQuantity
                };

                orderTotal += (menuOption.MENU_PRICE * orderQuantity);

                if (loyal)
                {
                    var loyalCust = db.LOYAL_CUSTOMERS.Single(x => x.CUS_ID == customerID);
                    loyalCust.LCUS_POINTS += (int)orderTotal;
                }
                
                db.ORDER_LINES.Add(orderLine);
                db.SaveChanges();            
            }
            Console.WriteLine("\n---------------\nOrder Line {0} for Order #{1} added", orderLineCount, orderID);
            orderLineCount++;
            Console.ReadKey();
        }

        private void InsertCustomOrder()
        {
            List<string> fields = new List<string> { "Sandwich Name: ", "Meat: ", "Meat Quantity: ", "Cheese: ", "Bread: ", "Order Quantity: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Customize Sandwich", fields);
                if (inputs != null)
                {
                    ExecInsertSandwich(inputs.ToArray());
                    inputs.Clear();
                }
                else
                    break;
            }
        }   
        
        private void ExecInsertSandwich(string[] values)
        {
            string sandwichName = values[0], meat = values[1], cheese = values[3], bread = values[4], orderQuantity = values[5];
            int meatQuantity;
            if (!int.TryParse(values[2], out meatQuantity)) { Console.WriteLine("\nInsert valid quantity amount"); return; }

            using (var db = new AppContext())
            {
                var menu = db.MENU_OPTIONS.SingleOrDefault(x => x.MENU_NAME.Equals("Sandwich"));

                if (menu != null)
                {
                    var sandwich = new SANDWICH
                    {
                        MENU_ID = menu.MENU_ID,
                        SAND_NAME = sandwichName,
                        SAND_MEAT_TYPE = meat,
                        SAND_MEAT_QUANTITY = meatQuantity,
                        SAND_CHEESE_TYPE = cheese,
                        SAND_BREAD_TYPE = bread
                    };

                    db.SANDWICHES.Add(sandwich);
                    db.SaveChanges();

                    ExecInsertOrder(new string[] { sandwich.SAND_NAME, orderQuantity });
                }
            }
        }         
    }
}
