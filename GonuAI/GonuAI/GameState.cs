using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GonuAI.GameState;

namespace GonuAI
{
    public class GameParameters
    {
        public static int stateCount = 19560;
        public static int actionMinIdx = 1;
        public static int actionMaxIdx = 9;
    }
    public class GameState
    {
        public int[,] boardState;
        public int nextTurn;
        public int boardStateKey;
        public int numberOfBlacks;
        public int numberOfWhites;
        public int gameWinner;

        public class ConnectedPosition
        {
            public int row;
            public int col;
            public ConnectedPosition(int r, int c)
            {
                row = r;
                col = c;
            }
        }

        public GameState()
        {
            boardState = new int[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
            nextTurn = 1;
            boardStateKey = 1;
            numberOfBlacks = 0;
            numberOfWhites = 0;
            gameWinner = 0;
        }

        public GameState(int _boardStateKey)
        {
            boardState = new int[3, 3];
            boardStateKey = _boardStateKey;
            nextTurn = _boardStateKey % 3;
            gameWinner = 0;
            PopulateBoard(_boardStateKey / 3);
        }

        public void PopulateBoard(int _boardState)
        {
            int boardValueProcessing = _boardState;
            numberOfBlacks = 0;
            numberOfWhites = 0;

            for (int i = 8; i>=0; i--)
            {
                // 아래 코드 분석 다시 해보기.(돌 숫자 늘리는 부분 이상?
                int boardValue = boardValueProcessing % 3;
                boardValueProcessing = boardValueProcessing / 3;

                boardState[i / 3, i % 3] = boardValue;

                if (boardValue == 1)
                    numberOfBlacks++;
                if (boardValue == 2)
                    numberOfWhites++;
            }
        }

        public bool IsValidSecondStage()
        {
            if (numberOfBlacks == 4 && numberOfWhites == 4)
                return true;
            return false;
        }

        public bool IsValidFirstStage()
        {
            if (numberOfBlacks > 4)
                return false;
            if (numberOfWhites > 3)
                return false;

            if (numberOfBlacks == numberOfWhites || numberOfBlacks == numberOfWhites + 1)
                return true;
            return false;
        }

        public int GetFirstStageTurn()
        {
            if (numberOfBlacks == numberOfWhites)
                return 1;
            if (numberOfBlacks == numberOfWhites + 1)
                return 2;
            return 0;
        }

        public bool IsFinalState()
        {
            gameWinner = 0;
            if (boardState[0, 0] == boardState[0, 1] && boardState[0, 1] == boardState[0, 2])
            {
                if (boardState[0, 0] != 0)
                {
                    gameWinner = boardState[0, 0];
                    return true;
                }
            }
            if (boardState[1, 0] == boardState[1, 1] && boardState[1, 1] == boardState[1, 2])
            {
                if (boardState[1, 0] != 0)
                {
                    gameWinner = boardState[1, 0];
                    return true;
                }
            }
            if (boardState[2, 0] == boardState[2, 1] && boardState[2, 1] == boardState[2, 2])
            {
                if (boardState[2, 0] != 0)
                {
                    gameWinner = boardState[2, 0];
                    return true;
                }
            }
            if (boardState[0, 0] == boardState[1, 0] && boardState[1, 0] == boardState[2, 0])
            {
                if (boardState[0, 0] != 0)
                {
                    gameWinner = boardState[0, 0];
                    return true;
                }
            }
            if (boardState[0, 1] == boardState[1, 1] && boardState[1, 1] == boardState[2, 1])
            {
                if (boardState[0, 1] != 0)
                {
                    gameWinner = boardState[0, 1];
                    return true;
                }
            }
            if (boardState[0, 2] == boardState[1, 2] && boardState[1, 2] == boardState[2, 2])
            {
                if (boardState[0, 2] != 0)
                {
                    gameWinner = boardState[0, 2];
                    return true;
                }
            }
            if (boardState[0, 0] == boardState[1, 1] && boardState[1, 1] == boardState[2, 2])
            {
                if (boardState[0, 0] != 0)
                {
                    gameWinner = boardState[0, 0];
                    return true;
                }
            }
            if (boardState[0, 2] == boardState[1, 1] && boardState[1, 1] == boardState[2, 0])
            {
                if (boardState[0, 2] != 0)
                {
                    gameWinner = boardState[0, 2];
                    return true;
                }
            }

            return false;
        }

        public bool IsValidMove(int move)
        {
            int row = (move - 1) / 3;
            int col = (move - 1) % 3;

            if (IsValidFirstStage())
            {
                if (boardState[row, col] == 0)
                    return true;
            }
            else if (IsValidSecondStage())
            {
                if (boardState[row, col] != nextTurn)
                    return false;
                IEnumerable<ConnectedPosition> connectedPositions = GetConnectedPositions(row, col);
                IEnumerable<ConnectedPosition> connectedEmptySpots = connectedPositions.Where(e => boardState[e.row, e.col] == 0);

                if (connectedEmptySpots.Count() > 0)
                {
                    IEnumerable<ConnectedPosition> connectOpponents = connectedPositions.Where(e => boardState[e.row, e.col] != 0 && boardState[e.row, e.col] != nextTurn);

                    if (connectOpponents.Count() > 0)
                        return true;
                }
            }
            return false;
        }


        private IEnumerable<ConnectedPosition> GetConnectedPositions(int row, int col)
        {
            List<ConnectedPosition> connectedPositionList = new List<ConnectedPosition>();
            if (row == 0 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 0));
            }
            if (row == 0 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(0, 2));
            }
            if (row == 0 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(1, 2));
            }
            if (row == 1 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 0));
            }
            if (row == 1 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 0));
                connectedPositionList.Add(new ConnectedPosition(0, 1));
                connectedPositionList.Add(new ConnectedPosition(0, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 2));
                connectedPositionList.Add(new ConnectedPosition(2, 0));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 1 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(0, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 2 && col == 0)
            {
                connectedPositionList.Add(new ConnectedPosition(1, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
            }
            if (row == 2 && col == 1)
            {
                connectedPositionList.Add(new ConnectedPosition(2, 0));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 2));
            }
            if (row == 2 && col == 2)
            {
                connectedPositionList.Add(new ConnectedPosition(1, 2));
                connectedPositionList.Add(new ConnectedPosition(1, 1));
                connectedPositionList.Add(new ConnectedPosition(2, 1));
            }
            return connectedPositionList;
        }

        public GameState GetNextState(int move)
        {
            GameState nextState = new GameState(boardStateKey);
            nextState.MakeMove(move);
            return nextState;
        }

        public void MakeMove(int move)
        {
            int row = (move - 1) / 3;
            int col = (move - 1) % 3;

            if (IsValidFirstStage())
            {
                boardState[row, col] = nextTurn;

                if (nextTurn == 1)
                    numberOfBlacks++;
                else if (nextTurn == 2)
                    numberOfWhites++;
            }
            else if (IsValidSecondStage())
            {
                int emptyRow = 0;
                int emptyCol = 0;

                for (int i=0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (boardState[i, j] == 0)
                        {
                            emptyRow = i;
                            emptyCol = j;
                            break;
                        }
                    }
                }
                boardState[row, col] = 0;
                boardState[emptyRow, emptyCol] = nextTurn;
            }

            if (nextTurn == 1)
                nextTurn = 2;
            else if (nextTurn == 2)
                nextTurn = 1;

            int _boardStateKey = 0;
            for (int i = 0; i < 9; i++)
            {
                _boardStateKey = _boardStateKey * 3;
                _boardStateKey = _boardStateKey + boardState[i / 3, i % 3];

            }
            boardStateKey = _boardStateKey * 3 + nextTurn;
        }

        public float GetReward()
        {
            if (IsFinalState())
            {
                if (gameWinner == 1)
                    return 100.0f;
                else if (gameWinner == 2)
                    return -100.0f;
            }
            return 0.0f;
        }

        public int CountValidMove()
        {
            int count = 0;
            for (int i=GameParameters.actionMinIdx; i<=GameParameters.actionMaxIdx; i++)
            {
                if (IsValidMove(i))
                    count++;
            }
            return count;
        }

        public bool DisplayBoard(int turnCount, int lastMove, GamePlayer blackPlayer, GamePlayer whitePlayer)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine($"○ : {blackPlayer}, ● : {whitePlayer}");
            Console.Write($"Turn : {turnCount}, ");
            Console.WriteLine($"{GetTurnMark()}");
            Console.WriteLine(Environment.NewLine);

            if (IsValidFirstStage())
            {
                Console.WriteLine("Step 1.....");
            }
            else
            {
                Console.WriteLine("Step 2.....");
            }

            if (lastMove != 0)
            {
                Console.WriteLine($"Last Move - Row: {(lastMove - 1) / 3}, Column: {(lastMove - 1) % 3}");
            }
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine($"\t{GetGameBoardValue(0, 0)}\t\t{GetGameBoardValue(0, 1)}\t\t{GetGameBoardValue(0, 2)}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"\t{GetGameBoardValue(1, 0)}\t\t{GetGameBoardValue(1, 1)}\t\t{GetGameBoardValue(1, 2)}");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"\t{GetGameBoardValue(2, 0)}\t\t{GetGameBoardValue(2, 1)}\t\t{GetGameBoardValue(2, 2)}");

            bool isFinish = IsFinalState();
            Console.WriteLine(Environment.NewLine);
            switch (gameWinner)
            {
                case 1:
                    Console.WriteLine("○ is Winner");
                    break;
                case 2:
                    Console.WriteLine("● is Winner");
                    break;
                default:
                    Console.WriteLine("Game is going");
                    break;
            }
            return isFinish;

        }

        private string GetTurnMark()
        {
            return nextTurn == 1 ? "○" : "●";
        }

        private string GetGameBoardValue(int row, int col)
        {
            switch(boardState[row, col])
            {
                case 1:
                    return "○";
                case 2:
                    return "●";
                default:
                    return "＋";
            }
        }
    }
}
