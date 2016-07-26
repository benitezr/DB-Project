using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeliApp
{
    public class MainMenu : Menu
    {
        public MainMenu(string newTitle, MenuManager newManager)
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
                    Console.WriteLine("[1] Loyalty\n[2] Inventory\n[3] Checkout\n[4] Generate Report\n[5] Quit");
                }

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out option)) { option = 0; }

                switch (option)
                {
                    case 1:
                        menuMng.AddMenu(new LoyaltyMenu("New Loyalty Customer", menuMng));
                        display = true;
                        break;
                    case 2:
                        menuMng.AddMenu(new InventoryMenu("Inventory Menu", menuMng));
                        display = true;
                        break;
                    case 3:
                        menuMng.AddMenu(new CheckoutMenu("Checkout Menu", menuMng));
                        display = true;
                        break;
                    case 4:
                        menuMng.AddMenu(new ReportMenu("Generate Report", menuMng));
                        display = true;
                        break;
                    case 5:
                        quit = true;
                        Console.Write("\nTerminating . . . ");
                        Thread.Sleep(500);
                        break;
                    default:
                        Console.WriteLine("\nInput invalid");
                        display = false;
                        break;
                }
            } while (!quit);
        }
    }
}
