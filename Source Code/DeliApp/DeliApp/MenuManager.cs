using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliApp
{
    public class MenuManager
    {
        private Stack<Menu> menus = new Stack<Menu>();

        public MenuManager()
        {
            Console.SetWindowSize(Math.Min(150, Console.LargestWindowWidth), Math.Min(60, Console.LargestWindowHeight));
            Console.ForegroundColor = ConsoleColor.Green;
            menus.Push(new MainMenu("Main Menu", this));
        }

        public void Run()
        {
            menus.Peek().Display();
        }

        public void AddMenu(Menu newMenu)
        {
            menus.Push(newMenu);
            menus.Peek().Display();
        }

        public void RemoveMenu()
        {
            menus.Pop();
        }
    }
}
