using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliApp
{
    public class LoyaltyMenu : Menu
    {

        public LoyaltyMenu(string newTitle, MenuManager newManager)
            : base(newTitle, newManager)
        {

        }

        public override void Display()
        {
            List<string> fields = new List<string> { "First Name: ", "Last Name: ", "Phone: ", "Email: ", "Address: ", "Zipcode: " };
            List<string> inputs;

            while (true)
            {
                inputs = NewInputMenu(title, fields);
                if (inputs != null)
                {
                    ExecInsert(inputs.ToArray());
                    inputs.Clear();
                }
                else
                    break;
            }
            menuMng.RemoveMenu();
        }

        private void ExecInsert(string[] values)
        {
            using (var db = new AppContext())
            {
                var customer = new CUSTOMER
                {
                    CUS_FNAME = values[0],
                    CUS_LNAME = values[1]
                };

                db.CUSTOMERS.Add(customer);
                db.SaveChanges();

                var loyalCustomer = new LOYAL_CUSTOMERS
                {
                    CUS_ID = customer.CUS_ID,
                    LCUS_PHONE = values[2],
                    LCUS_EMAIL = values[3],
                    LCUS_ADDRESS = values[4],
                    LCUS_ZIPCODE = values[5]
                };

                db.LOYAL_CUSTOMERS.Add(loyalCustomer);
                db.SaveChanges();

                Console.WriteLine("\n---------------\nCustomer #{0} has been successfully added ", customer.CUS_ID);
            }
            Console.ReadKey();
        }
    }
}
