using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables.Core;

namespace DeliApp
{
    public class ReportMenu : Menu
    {
        private DateTime firstDayOfWeek, lastDayOfWeek, firstDayOfMonth, lastDayOfMonth, firstDayOfYear, lastDayOfYear;

        public ReportMenu(string newTitle, MenuManager newManager)
            : base(newTitle, newManager)
        {
            firstDayOfWeek = DateTime.Now.FirstDayOfWeek();
            lastDayOfWeek = DateTime.Now.LastDayOfWeek();
            firstDayOfMonth = DateTime.Now.FirstDayOfMonth();
            lastDayOfMonth = DateTime.Now.LastDayOfMonth();
            firstDayOfYear = DateTime.Now.FirstDayOfYear();
            lastDayOfYear = DateTime.Now.LastDayOfYear();
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
                    Console.WriteLine("[1] This Week\n[2] This Month\n[3] This Year\n[4] Specify Date\n[5] Back");
                }

                if (!int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out option)) { option = 0; }

                switch (option)
                {
                    case 1:
                        GenerateReport(firstDayOfWeek, lastDayOfWeek);
                        display = true;
                        break;
                    case 2:
                        GenerateReport(firstDayOfMonth, lastDayOfMonth);
                        display = true;
                        break;
                    case 3:
                        GenerateReport(firstDayOfYear, lastDayOfYear);
                        display = true;
                        break;
                    case 4:
                        SpecifyDate();
                        display = true;
                        break;
                    case 5:
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

        private void GenerateReport(DateTime dt1, DateTime dt2)
        {
            Console.Clear();
            using (var db = new AppContext())
            {
                var query = from a in db.ACCOUNTS
                            join s in db.SANDWICHES on a.SAND_ID equals s.SAND_ID
                            where a.ACC_DATE >= dt1 && a.ACC_DATE <= dt2
                            select new { s.SAND_NAME, a.ACC_REVENUE, a.ACC_COST, a.ACC_PROFIT };

                var count = from items in query
                            group items by new
                            {
                                items.SAND_NAME
                            } into g
                            select new
                            {
                                g.Key.SAND_NAME,
                                ACC_REVENUE = g.Sum(x => x.ACC_REVENUE),
                                ACC_COST = g.Sum(x => x.ACC_COST),
                                ACC_PROFIT = g.Sum(x => x.ACC_PROFIT)
                            };


                ConsoleTable table = new ConsoleTable("Sandwich", "Revenue", "Cost", "Profit");

                foreach (var item in count)
                {
                    table.AddRow(item.SAND_NAME, item.ACC_REVENUE, item.ACC_COST, item.ACC_PROFIT);
                }

                table.Write();
            }
            Console.ReadKey();
        }

        private void SpecifyDate()
        {
            List<string> fields = new List<string> { "Begin Date: ", "End Date: " };
            List<string> inputs;
            DateTime dt1, dt2;
            while (true)
            {
                inputs = NewInputMenu("Specify Dates", fields);
                if (inputs != null)
                {
                    if (!DateTime.TryParse(inputs[0], out dt1) || !DateTime.TryParse(inputs[1], out dt2)) { Console.WriteLine("\nInvalid dates inserted"); Console.ReadKey(); continue; }
                    GenerateReport(DateTime.Parse(inputs[0]), DateTime.Parse(inputs[1]));
                    inputs.Clear();
                    break;
                }
                else
                    break;
            }
        }
    }
}
