using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public class SARSAManager
    {
        public Dictionary<int, Dictionary<int, float>> actionValueFunction { get; set; }
        public Dictionary<int, float> functionAccForEpisodeCount { get; set; }
        public float discountFactor = 0.9f;
        public float updateStep = 0.01f;

        public SARSAManager()
        {
            actionValueFunction = new Dictionary<int, Dictionary<int, float>>();
            functionAccForEpisodeCount = new Dictionary<int, float>();

        }

        public void UpdateSARSA()
        {
            // 상속으로 구현해보기. (DP, SARSA, Q-Learning)
            InitValueFunc();

            ApplySARSA();
        }

        private void InitValueFunc()
        {
            Console.Clear();
            Console.WriteLine("Start SARSA");
            Console.WriteLine("Init Value Function");

            actionValueFunction.Clear();
            actionValueFunction = Utilities.CreateActionValueFunction();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("가치함수 초기화 완료");

            Console.WriteLine(Environment.NewLine);
            Console.Write("Press Any keys");
            Console.ReadLine();
        }

        private void ApplySARSA()
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
                    int secondAction = Utilities.GetEpsilonGreedyAction(secondState.nextTurn, actionValueFunction[secondState.boardStateKey]);
                    float reward = secondState.GetReward();

                    float firstStateActionValue = actionValueFunction[firstState.boardStateKey][firstAction];
                    float secondStateActionValue = 0f;
                    if (secondAction != 0)
                        secondStateActionValue = actionValueFunction[secondState.boardStateKey][secondAction];

                    float updatedActionValue = firstStateActionValue + updateStep * (reward + discountFactor * secondStateActionValue - firstStateActionValue);
                    actionValueFunction[firstState.boardStateKey][firstAction] = updatedActionValue;

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
                        valueFunctionAcc = Utilities.EvaluateValueFunction(QFunctionType.SARSA);
                        functionAccForEpisodeCount.Add(episodeCount, valueFunctionAcc);
                        functionAcc = $"Function Acc {valueFunctionAcc}%.";
                    }
                    Console.WriteLine($"에피소드 {episodeCount}개 처리. {functionAcc}");
                }
                
                if (episodeCount > 1000000)
                {
                    if (isDPFunctionAvailable)
                    {
                        functionAccForEpisodeCount.Add(episodeCount, Utilities.EvaluateValueFunction(QFunctionType.SARSA));
                    }
                    keepUpdating = false;
                }
            }
            Console.WriteLine(Environment.NewLine);
            Console.Write("SARSA를 종료합니다. 아무 키나 누르세요:");
            Console.ReadLine();

        }

        public IEnumerable<int> GetNextMoveCandidate(int boardStateKey)
        {
            GameState gameState = new GameState(boardStateKey);
            return Utilities.GetGreedyActionCandidate(gameState.nextTurn, actionValueFunction[boardStateKey]);
        }

    }
}
