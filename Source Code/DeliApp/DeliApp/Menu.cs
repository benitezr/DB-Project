using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliApp
{
    public class Menu
    {
        protected string title;
        protected MenuManager menuMng;

        public Menu(string newTitle, MenuManager newManager)
        {
            title = newTitle;
            menuMng = newManager;
        }

        public virtual void Display()
        {

        }

        protected void NewTitle(string newTitle)
        {
            Console.Clear();
            Console.WriteLine("{0}\n{1}\n", newTitle, new String('-', newTitle.Length));
        }

        protected List<string> NewInputMenu(string newTitle, List<string> newFields, bool clearConsole = true)
        {
            List<string> inputs = new List<string>();

            if (clearConsole)
            {
                NewTitle(newTitle);
                Console.WriteLine("Press ESC to cancel");
            }

            int i = 0;
            do
            {
                Console.Write("\n{0}", newFields[i]);
                inputs.Add(Utilities.readCancelable());
                Console.WriteLine();
                if (string.IsNullOrEmpty(inputs[i])) { break; }
                i++;
            } while (i <= newFields.Count - 1);

            return i == newFields.Count ? inputs : null;
        }
    }
}
