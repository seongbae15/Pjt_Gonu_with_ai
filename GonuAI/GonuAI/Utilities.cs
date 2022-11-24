using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public class Utilities
    {
        public static Random random = new Random();

        public static Dictionary<int, Dictionary<int, float>> CreateActionValueFunction()
        {
            Dictionary<int, Dictionary<int, float>> actionValueFunction = new Dictionary<int, Dictionary<int, float>>();

            for (int i=0; i <= GameParameters.stateCount; i++)
            {
                GameState state = new GameState();
                state.PopulateBoard(i);

                if (state.IsValidSecondStage())
                {
                    actionValueFunction.Add(i * 3 + 1, GetActionValueDictionary(i * 3 + 1));
                    actionValueFunction.Add(i * 3 + 2, GetActionValueDictionary(i * 3 + 2));
                }
                else if (state.IsValidFirstStage())
                {
                    int nextTurn = state.GetFirstStageTurn();
                    actionValueFunction.Add(i * 3 + nextTurn, GetActionValueDictionary(i * 3 + nextTurn));
                }
            }
            return actionValueFunction;
        }

        private static Dictionary<int, float> GetActionValueDictionary(int gameStateKey)
        {
            GameState gameState = new GameState(gameStateKey);
            Dictionary<int, float> actionValues = new Dictionary<int, float>();

            for (int i = GameParameters.actionMinIdx; i <= GameParameters.actionMaxIdx; i++)
            {
                if (gameState.IsValidMove(i))
                {
                    actionValues.Add(i, 0f);
                }
            }
            return actionValues;
        }

        public static IEnumerable<int> GetGreedyActionCandidate(int turn, Dictionary<int, float> actionValues)
        {
            float greedyActionValue = 0f;
            if (actionValues.Count == 0)
                return new List<int>();
            if (turn == 1)
            {
                greedyActionValue = actionValues.Select(e => e.Value).Max();
            }
            else if (turn == 2)
            {
                greedyActionValue = actionValues.Select(e => e.Value).Min();
            }
            return actionValues.Where(e => e.Value == greedyActionValue).Select(e => e.Key);
        }

        public static int GetEpsilonGreedyAction(int turn, Dictionary<int, float> actionValues)
        {
            float greedyActionValue = 0f;
            float epsilon = 10;

            if (actionValues.Count == 0)
                return 0;

            if (turn == 1)
            {
                greedyActionValue = actionValues.Select(e => e.Value).Max();
            }
            else if (turn ==2)
            {
                greedyActionValue = actionValues.Select(e => e.Value).Min();
            }

            int exploitRandom = random.Next(0, 100);
            IEnumerable<int> actionCandidates;

            if (exploitRandom < epsilon)
            {
                actionCandidates = actionValues.Where(e => e.Value != greedyActionValue).Select(e => e.Key);
                if (actionCandidates.Count() == 0)
                    actionCandidates = actionValues.Where(e => e.Value == greedyActionValue).Select(e => e.Key);
            }
            else
            {
                actionCandidates = actionValues.Where(e => e.Value == greedyActionValue).Select(e => e.Key);
            }
            return actionCandidates.ElementAt(random.Next(0, actionCandidates.Count()));
        }

        public static float GetGreedyActionValue(int turn, Dictionary<int, float> actionValues)
        {
            if (actionValues.Count == 0)
                return 0f;
            if (turn == 1)
            {
                return actionValues.Select(e => e.Value).Max();
            }
            else if (turn == 2)
            {
                return actionValues.Select(e => e.Value).Min();
            }
            return 0f;
        }

        public static float EvaluateValueFunction(QFunctionType functionType)
        {
            if (Program.dpManager.stateValueFunc.Count == 0)
                return 0f;

            int totalStateCount = 0;
            int matchingStateCount = 0;

            for (int i=0; i <= GameParameters.stateCount; i++)
            {
                GameState state = new GameState();
                state.PopulateBoard(i);

                if (state.IsValidSecondStage())
                {
                    GameState gameState = new GameState(i * 3 + 1);
                    if (!gameState.IsFinalState() && gameState.CountValidMove() > 0)
                    {
                        if (CompareActionCandidate(i * 3 + 1, functionType))
                        {
                            matchingStateCount++;
                        }
                        totalStateCount++;
                    }

                    gameState = new GameState(i * 3 + 2);
                    if (!gameState.IsFinalState() && gameState.CountValidMove() > 0)
                    {
                        if (CompareActionCandidate(i * 3 + 2, functionType))
                        {
                            matchingStateCount++;
                        }
                        totalStateCount++;
                    }
                }
                else if (state.IsValidFirstStage())
                {
                    int nextTurn = state.GetFirstStageTurn();
                    GameState gameState = new GameState(i * 3 + nextTurn);
                    if (!gameState.IsFinalState() && gameState.CountValidMove() > 0)
                    {
                        if (CompareActionCandidate(i * 3 + nextTurn, functionType))
                        {
                            matchingStateCount++;
                        }
                        totalStateCount++;
                    }
                }
            }
            return ((float)matchingStateCount) / ((float)totalStateCount) * 100f;
        }

        private static bool CompareActionCandidate(int boardStateKey, QFunctionType functionType)
        {
            IEnumerable<int> DPActionCandidate = Program.dpManager.GetNextMoveCandidate(boardStateKey);
            IEnumerable<int> QActionCandidate;

            if (functionType == QFunctionType.SARSA)
                QActionCandidate = Program.sarsaManger.GetNextMoveCandidate(boardStateKey);
            else if(functionType == QFunctionType.QLEARNING)
                QActionCandidate = Program.qLearningManager.GetNextMoveCandidate(boardStateKey);
            else
                QActionCandidate = new List<int>();

            if (QActionCandidate.Count() == 0 && DPActionCandidate.Count() > 0)
                return false;

            IEnumerable<int> UnmatchedActionList = QActionCandidate.Where(e => !DPActionCandidate.Contains(e));
            if (UnmatchedActionList.Count() == 0)
                return true;

            return false;
        }
    }
}
