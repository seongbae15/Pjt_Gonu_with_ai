using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public enum GamePlayer
    {
        DynamicProgramming,
        Human,
        None,
    }

    public class GameManager
    {
        public GamePlayer blackPlayer;
        public GamePlayer whitePlayer;

        public void PlayGame()
        {
            while (true)
            {
                blackPlayer = SelectPlayer("Black");
                if (blackPlayer == GamePlayer.None)
                    return;

                whitePlayer = SelectPlayer("White");
                if (whitePlayer == GamePlayer.None)
                    return;

                ManageGame();
            }
        }

        public void ManageGame()
        {
            GameState gameState = new GameState();
            int gameTurnCount = 0;
            int gameMove = 0;
            bool isGameFinished = gameState.IsFinalState();

            while (!isGameFinished)
            {
                isGameFinished = gameState.DisplayBoard(gameTurnCount, gameMove, blackPlayer, whitePlayer);

                Console.WriteLine(Environment.NewLine);

                if (isGameFinished)
                {
                    Console.Write("Game ends. Press Any keys.");
                    Console.ReadLine();
                }
                else
                {
                    GamePlayer playerForNextTurn = GetGamePlayer(gameState.nextTurn);
                    if (playerForNextTurn == GamePlayer.Human)
                    {
                        gameMove = GetHumanGameMove(gameState);
                    }
                    else
                    {
                        Console.Write("Press any keys.");
                        Console.ReadLine();

                        switch (playerForNextTurn)
                        {
                            case GamePlayer.DynamicProgramming:
                                gameMove = Program.dpManager.GetNextMove(gameState.boardStateKey);
                                break;
                        }
                    }
                    gameState.MakeMove(gameMove);
                    gameTurnCount++;

                }
            }
        }

        private int GetHumanGameMove(GameState gameState)
        {
            Console.Write("Input Next Action (1 ~ 9) : ");
            string humanMove = Console.ReadLine();

            while (true)
            {
                try
                {
                    int gameMove = Int32.Parse(humanMove);
                    if (gameMove >= 1 && gameMove <= 9 && gameState.IsValidMove(gameMove))
                        return gameMove;
                    else
                    {
                        Console.Write("This is Wrong Action. Input the other action (1 ~ 9) : ");
                        humanMove = Console.ReadLine();
                    }
                }
                catch
                {
                    Console.Write("This is Wrong Action. Input the other action (1 ~ 9) : ");
                    humanMove = Console.ReadLine();
                }
            }
        }

        private GamePlayer GetGamePlayer(int turn)
        {
            if (turn == 1)
                return blackPlayer;
            else
                return whitePlayer;
        }


        private GamePlayer SelectPlayer(string playerLabel)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Select {playerLabel}.");
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("1) Dynamic Programming");
                Console.WriteLine("2) SARSA");
                Console.WriteLine("3) Q-Learning");
                Console.WriteLine("4) Human");
                Console.WriteLine("5) Exit Game");
                Console.Write("Select (1 ~ 5) : ");

                switch (Console.ReadLine())
                {
                    case "1":
                        if (Program.dpManager.stateValueFunc.Count > 0)
                        {
                            return GamePlayer.DynamicProgramming;
                        }
                        else
                        {
                            Console.Write("상태 가치 함수가 정의되어 있지 않습니다. 동적 프로그래밍을 수행하거나, 가치 함수를 읽어오세요.");
                            Console.WriteLine(Environment.NewLine);
                            Console.Write("Press Any Keys.");
                            Console.ReadLine();
                        }
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        Console.Write("사람을 선택하셨습니다..");
                        Console.WriteLine(Environment.NewLine);
                        Console.Write("아무 키나 누르세요:");
                        Console.ReadLine();
                        return GamePlayer.Human;
                    case "5":
                        Console.WriteLine("Back to main menu.");
                        Console.WriteLine(Environment.NewLine);
                        Console.Write("Press Any Keys");
                        Console.ReadLine();
                        return GamePlayer.None;
                    default:
                        break;
                }
            }
        }
    }
}
