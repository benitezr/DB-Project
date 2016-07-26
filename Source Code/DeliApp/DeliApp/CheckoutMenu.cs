using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliApp
{
    public class CheckoutMenu : Menu
    {

        private int id;
        private string name;
        private bool loyal;

        public CheckoutMenu(string newTitle, MenuManager newManager)
            : base(newTitle, newManager)
        {
            id = 0;
            name = null;
            loyal = false;
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
                    Console.WriteLine("[1] Retrieve Loyal Customer\n[2] Add Customer\n[3] Place Order\n[4] Back");
                }

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out option)) { option = 0; }

                switch (option)
                {
                    case 1:
                        if (id > 0) { Console.WriteLine("\nCustomer has already been added"); display = false; break; }
                        SelectLoyalCustomer();
                        display = true;
                        break;
                    case 2:
                        if (id > 0) { Console.WriteLine("\nCustomer has already been added"); display = false; break; }
                        InsertCustomer();
                        display = true;
                        break;
                    case 3:
                        if (id < 1) { Console.WriteLine("\nCustomer has not been added"); display = false; break; }
                        menuMng.AddMenu(new OrderMenu("Place Order", menuMng, id, name, loyal));
                        display = true;
                        break;
                    case 4:
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

        private void SelectLoyalCustomer()
        {
            List<string> fields = new List<string> { "First Name: ", "Last Name: ", "Phone: ", "Zipcode: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Search Loyal Customers", fields);
                if (inputs != null)
                {
                    ExecSelect(inputs.ToArray());
                    inputs.Clear();
                    if (id > 0) { break; }
                }
                else
                    break;
            }
        }

        private void ExecSelect(string[] values)
        {
            string fname = values[0], lname = values[1], phone = values[2], zip = values[3];
            StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;

            using (var db = new AppContext())
            {
                var customer = (from c in db.CUSTOMERS
                                join lc in db.LOYAL_CUSTOMERS on c.CUS_ID equals lc.CUS_ID
                                where c.CUS_FNAME.Equals(fname, ignoreCase) && c.CUS_LNAME.Equals(lname, ignoreCase) && lc.LCUS_PHONE.Equals(phone) && lc.LCUS_ZIPCODE.Equals(zip)
                                select c).SingleOrDefault();

                if(customer == null)
                {
                    Console.WriteLine("\nLoyal customer does not exist");
                    Console.ReadKey();
                    return;
                }

                id = customer.CUS_ID;
                name = customer.CUS_FNAME + " " + customer.CUS_LNAME;
                loyal = true;
            }
            Console.WriteLine("\n---------------\nLoyal Customer found");
            Console.ReadKey();
        }

        private void InsertCustomer()
        {
            List<string> fields = new List<string> { "First Name: ", "Last Name: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Add Customer", fields);
                if (inputs != null)
                {
                    ExecInsertCustomer(inputs.ToArray());
                    inputs.Clear();
                    if (id > 0) { break; }
                }
                else
                    break;
            }
        }

        private void ExecInsertCustomer(string[] values)
        {
            string fname = values[0], lname = values[1];

            using (var db = new AppContext())
            {
                var customer = new CUSTOMER
                {
                    CUS_FNAME = fname,
                    CUS_LNAME = lname
                };

                db.CUSTOMERS.Add(customer);
                db.SaveChanges();
                id= customer.CUS_ID;
                name = customer.CUS_FNAME + " " + customer.CUS_LNAME;
            }
            Console.WriteLine("\n---------------\nCustomer #{0} added successfully", id);
            Console.ReadKey();
        }
    }
}
