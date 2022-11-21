using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    internal class Program
    {
        private static DPManager dpManager;

        static void Main(string[] args)
        {
            dpManager = new DPManager();

            bool isShowMenu = true;

            while (isShowMenu)
            {
                isShowMenu = MainMenu();
            }

        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("원하는 동작을 선택하세요.");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("1) Start Dynamic Programming.");
            Console.WriteLine("2) Save Dynamic Porgramming Value Function.");
            Console.WriteLine("3) Load Dynamic Porgramming Value Function.");
            Console.WriteLine("4) Start SARSA.");
            Console.WriteLine("5) Save SARSA Value Function.");
            Console.WriteLine("6) Load SARSA Value Function.");
            Console.WriteLine("71) Start Q-Learning.");
            Console.WriteLine("8) Save Q-Learning Value Function.");
            Console.WriteLine("9) Load Q-Learning Value Function.");
            Console.WriteLine("10) Start Game");
            Console.WriteLine("11) Exit");
            Console.WriteLine(Environment.NewLine);
            Console.Write("Input : ");

            switch (Console.ReadLine())
            {
                case "1":
                    dpManager.UpdateByDP();
                    return true;
                case "2":
                    return true;
                case "3":
                    return true;
                case "4":
                    return true;
                case "5":
                    return true;
                case "6":
                    return true;
                case "7":
                    return true;
                case "8":
                    return true;
                case "9":
                    return true;
                case "10":
                    return true;
                case "11":
                    return false;
                default:
                    return true;
            }

        }
    }
}
