using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables.Core;
using System.Globalization;

namespace DeliApp
{
    public class InventoryMenu : Menu
    {

        public InventoryMenu(string newTitle, MenuManager newManager)
            : base(newTitle, newManager)
        {

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
                    Console.WriteLine("[1] View Inventory On Hand\n[2] Update Inventory\n[3] Add New Inventory Item\n[4] Back");

                    if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out option)) { option = 0; }

                    switch (option)
                    {
                        case 1:
                            DisplayInventory();
                            display = true;
                            break;
                        case 2:
                            UpdateInventory();
                            display = true;
                            break;
                        case 3:
                            InsertInventory();
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
                }
            } while (!quit);
            menuMng.RemoveMenu();
        }

        private void DisplayInventory()
        {
            using (var db = new AppContext())
            {
                NewTitle("Inventory On Hand");

                var query = from i in db.INVENTORY_ITEMS
                            select i;

                ConsoleTable table = new ConsoleTable("Item Name", "Quantity");

                foreach (var item in query)
                {
                    table.AddRow(item.INV_NAME, item.INV_QUANTITY);
                }

                table.Write();
            }
            Console.Write("\nPress any key to go back . . . ");
            Console.ReadKey();
        }

        private void UpdateInventory()
        {
            List<string> fields = new List<string> { "Item Name: ", "Quantity Added: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Update Inventory", fields);
                if (inputs != null)
                {
                    ExecUpdate(inputs.ToArray());
                    inputs.Clear();
                }
                else
                    break;
            }
        }

        private void ExecUpdate(string[] values)
        {
            string item = values[0];
            int quantity;
            CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            if (!int.TryParse(values[1], NumberStyles.AllowLeadingSign, culture, out quantity)) { Console.WriteLine("\nQuantity amount inserted incorrectly"); return; }

            using (var db = new AppContext())
            {
                var inventoryItem = db.INVENTORY_ITEMS.FirstOrDefault(x => x.INV_NAME.Equals(item, StringComparison.OrdinalIgnoreCase));

                if (inventoryItem != null)
                {
                    inventoryItem.INV_QUANTITY += quantity;
                    db.SaveChanges();
                    Console.WriteLine("\n---------------\nItem '{0}' updated successfully", item);
                }
                else
                    Console.WriteLine("\nThere is no item by the name of '{0}'", item);
            }
            Console.ReadKey();
        }

        private void InsertInventory()
        {
            List<string> fields = new List<string> { "Vendor Name: ", "Delivery Date: ", "Cost: ", "Category Name: ", "Item Name: ", "Quantity: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu("Add New Item", fields);
                if (inputs != null)
                {
                    ExecInsert(inputs.ToArray());
                    inputs.Clear();
                }
                else
                    break;
            }
        }

        private void ExecInsert(string[] values)
        {
            string vendorName = values[0], categoryName = values[3], itemName = values[4];
            DateTime deliveryDate = DateTime.Parse(values[1]);
            decimal cost;
            int quantity;
            if (!decimal.TryParse(values[2], out cost) || !int.TryParse(values[5], out quantity))
            {
                Console.WriteLine("\nInvalid inputs where numbers are required");
                Console.ReadKey();
                return;
            }

            using (var db = new AppContext())
            {
                var vendor = db.VENDORS.FirstOrDefault(x => x.VEND_NAME.Equals(vendorName, StringComparison.OrdinalIgnoreCase));

                if (vendor == null)
                {
                    Console.WriteLine("\nVendor '{0}' does not exist", vendorName);
                    Console.ReadKey();
                    return;
                }

                var itemCategory = db.ITEM_CATEGORIES.FirstOrDefault(x => x.CAT_NAME.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

                if (itemCategory == null)
                {
                    Console.WriteLine("\nItem category '{0}' does not exist", itemCategory);
                    Console.ReadKey();
                    return;
                }

                var inventoryItem = new INVENTORY_ITEMS
                {
                    CAT_ID = itemCategory.CAT_ID,
                    INV_NAME = itemName,
                    INV_QUANTITY = quantity
                };

                db.INVENTORY_ITEMS.Add(inventoryItem);
                db.SaveChanges();

                var vendorItem = new VENDOR_ITEMS
                {
                    INV_ID = inventoryItem.INV_ID,
                    VEND_ID = vendor.VEND_ID,
                    ITEM_DELIVERY_DATE = deliveryDate,
                    ITEM_PRICE = cost
                };

                db.VENDOR_ITEMS.Add(vendorItem);
                db.SaveChanges();

                Console.WriteLine("\n---------------\nItem added successfully");
            }
            Console.ReadKey();
        }
    }
}
