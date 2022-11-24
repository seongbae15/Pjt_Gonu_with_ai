using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public class QLearningManager
    {
        public Dictionary<int, Dictionary<int, float>> actionValueFunction { get; set; }
        public Dictionary<int, float> functionAccForEpisodeCount { get; set; }
        public float discountFactor = 0.9f;
        public float updateStep = 0.01f;
        public QLearningManager()
        {
            actionValueFunction = new Dictionary<int, Dictionary<int, float>>();
            functionAccForEpisodeCount = new Dictionary<int, float>();
        }

        public void UpdateByQLearning()
        {
            InitializeValueFunction();

            ApplyQLearning();
        }

        private void InitializeValueFunction()
        {
            Console.Clear();
            Console.WriteLine("Start Q-Learninig.");
            Console.WriteLine("Init Value Function.");

            actionValueFunction.Clear();
            actionValueFunction = Utilities.CreateActionValueFunction();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Value Function 초기화 완료");

            Console.WriteLine(Environment.NewLine);
            Console.Write("Press any keys");
            Console.ReadLine();
        }

        private void ApplyQLearning()
        {
            Console.Clear();
            Console.WriteLine("가치 함수 업데이트 시작");
            Console.WriteLine(Environment.NewLine);

            int episodeCount = 0;
            bool keepUpdating = true;
            bool isDPFunctionAvailable = Program.dpManager.stateValueFunc.Count > 0;
            functionAccForEpisodeCount.Clear();

            while (keepUpdating)
            {
                GameState firstState = new GameState();
                bool episodeFinished = false;
                while (!episodeFinished)
                {
                    int firstAction = Utilities.GetEpsilonGreedyAction(firstState.nextTurn, actionValueFunction[firstState.boardStateKey]);
                    GameState secondState = firstState.GetNextState(firstAction);
                    float reward = secondState.GetReward();

                    float firstStateActionValue = actionValueFunction[firstState.boardStateKey][firstAction];
                    float secondStateActionValue = Utilities.GetGreedyActionValue(secondState.nextTurn, actionValueFunction[secondState.boardStateKey]);

                    float updateActionValue = firstStateActionValue + updateStep * (reward + discountFactor * secondStateActionValue - firstStateActionValue);
                    actionValueFunction[firstState.boardStateKey][firstAction] = updateActionValue;

                    if (secondState.IsFinalState() || actionValueFunction[secondState.boardStateKey].Count == 0)
                    {
                        episodeFinished = true;
                        episodeCount++;
                    }
                    else
                    {
                        firstState = secondState;
                    }
                }

                if (episodeCount % 1000 == 0)
                {
                    float valueFunctionAcc = 0f;
                    string functionAcc = "";

                    if (isDPFunctionAvailable)
                    {
                        valueFunctionAcc = Utilities.EvaluateValueFunction(QFunctionType.QLEARNING);
                        functionAccForEpisodeCount.Add(episodeCount, valueFunctionAcc);
                        functionAcc = $"함수 정확도 {valueFunctionAcc}%";
                    }
                    Console.WriteLine($"에피소드를 {episodeCount}개 처리했습니다. {functionAcc}");
                }
                
                if (episodeCount > 1000000)
                {
                    if (isDPFunctionAvailable)
                    {
                        functionAccForEpisodeCount.Add(episodeCount, Utilities.EvaluateValueFunction(QFunctionType.QLEARNING));
                    }
                    keepUpdating = false;
                }
            }
            Console.WriteLine(Environment.NewLine);
            Console.Write("Q-러닝 완료");
            Console.ReadLine();

        }
        public IEnumerable<int> GetNextMoveCandidate(int boardStateKey)
        {
            GameState gameState = new GameState(boardStateKey);
            return Utilities.GetGreedyActionCandidate(gameState.nextTurn, actionValueFunction[boardStateKey]);
        }

    }
}
