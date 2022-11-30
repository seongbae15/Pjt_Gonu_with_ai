using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public enum QFunctionType
    {
        SARSA,
        QLEARNING,
    }
    class Program
    {
        public static DPManager dpManager;
        public static GameManager gameManager;
        public static SARSAManager sarsaManger;
        public static QLearningManager qLearningManager;

        static void Main(string[] args)
        {
            dpManager = new DPManager();
            sarsaManger = new SARSAManager();
            qLearningManager = new QLearningManager();
            gameManager = new GameManager();

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
            Console.WriteLine("2) Start SARSA.");
            Console.WriteLine("3) Start Q-Learning.");
            Console.WriteLine("4) Start Game");
            Console.WriteLine("5) Exit");
            Console.WriteLine(Environment.NewLine);
            Console.Write("Input : ");

            switch (Console.ReadLine())
            {
                case "1":
                    dpManager.UpdateByDP();
                    return true;
                case "2":
                    sarsaManger.UpdateSARSA();
                    return true;
                case "3":
                    qLearningManager.UpdateByQLearning();
                    return true;
                case "4":
                    gameManager.PlayGame();
                    return true;
                case "5" +
                "":
                    return false;
                default:
                    return true;
            }

        }
    }
}
