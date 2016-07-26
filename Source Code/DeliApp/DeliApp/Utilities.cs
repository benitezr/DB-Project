using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliApp
{
    public static class Utilities
    {
        public static string readCancelable()
        {
            string word = "";
            StringBuilder builder = new StringBuilder();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape)
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (builder.Length > 0)
                    {
                        Console.Write("\b \b");
                        builder.Length--;
                    }
                    keyInfo = Console.ReadKey(true);
                    continue;
                }
                Console.Write(keyInfo.KeyChar);
                builder.Append(keyInfo.KeyChar);
                keyInfo = Console.ReadKey(true);
            }

            if (keyInfo.Key == ConsoleKey.Enter) { word = builder.ToString(); }

            return word;
        }

        public static DateTime FirstDayOfWeek(this DateTime dt)
        {
            var firstDay = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var diff = dt.DayOfWeek - firstDay;

            return dt.AddDays(-diff);
        }

        public static DateTime LastDayOfWeek(this DateTime dt)
        {
            return dt.FirstDayOfWeek().AddDays(6);
        }

        public static DateTime FirstDayOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static DateTime FirstDayOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        public static DateTime LastDayOfYear(this DateTime dt)
        {
            return dt.FirstDayOfYear().AddYears(1).AddDays(-1);
        }
    }
}
