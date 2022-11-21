using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GonuAI
{
    public class DPManager
    {
        public Dictionary<int, float> stateValueFunc { get; set; }
        public float discountFactor = 0.9f;

        int num00 = 0;
        int num10 = 0;
        int num11 = 0;
        int num21 = 0;
        int num22 = 0;
        int num32 = 0;
        int num33 = 0;
        int num43 = 0;
        int num44 = 0;

        public DPManager()
        {
            stateValueFunc = new Dictionary<int, float>();
        }
        
        public void UpdateByDP()
        {
            InitValueFunc();

            ApplyDP();
        }

        private void InitValueFunc()
        {
            Console.Clear();
            Console.WriteLine("Start Dynamic Programming!");
            Console.WriteLine("Initialize Value Function.");

            ResetStateCount();
            stateValueFunc.Clear();

            for (int i = 0; i <= GameParameters.stateCount; i++)
            {
                GameState state = new GameState();
                state.PopulateBoard(i);

                if (state.IsValidSecondStage())
                {
                    stateValueFunc.Add(i * 3 + 1, 0.0f);
                    stateValueFunc.Add(i * 3 + 2, 0.0f);
                    if (state.numberOfBlacks == 4 && state.numberOfWhites == 4)
                        num44++;
                }
                else if (state.IsValidFirstStage())
                {
                    stateValueFunc.Add(i * 3 + state.GetFirstStageTurn(), 0.0f);

                    if (state.numberOfBlacks == 0 && state.numberOfWhites == 0)
                        num00++;
                    if (state.numberOfBlacks == 1 && state.numberOfWhites == 0)
                        num10++;
                    if (state.numberOfBlacks == 1 && state.numberOfWhites == 1)
                        num11++;
                    if (state.numberOfBlacks == 2 && state.numberOfWhites == 1)
                        num21++;
                    if (state.numberOfBlacks == 2 && state.numberOfWhites == 2)
                        num22++;
                    if (state.numberOfBlacks == 3 && state.numberOfWhites == 2)
                        num32++;
                    if (state.numberOfBlacks == 3 && state.numberOfWhites == 3)
                        num33++;
                    if (state.numberOfBlacks == 4 && state.numberOfWhites == 3)
                        num43++;
                }
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Complete to init value func.");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine($"Black: 0, White: 0 - {num00}");
            Console.WriteLine($"Black: 1, White: 0 - {num10}");
            Console.WriteLine($"Black: 1, White: 1 - {num11}");
            Console.WriteLine($"Black: 2, White: 1 - {num21}");
            Console.WriteLine($"Black: 2, White: 2 - {num22}");
            Console.WriteLine($"Black: 3, White: 2 - {num32}");
            Console.WriteLine($"Black: 3, White: 3 - {num33}");
            Console.WriteLine($"Black: 4, White: 3 - {num43}");
            Console.WriteLine($"Black: 4, White: 4 - {num44}");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Press Any Keys.");
            Console.ReadLine();
        }

        private void ApplyDP()
        {
            Console.Clear();
            Console.WriteLine("Apply DP.");
            Console.WriteLine(Environment.NewLine);

            int loopCount = 0;
            bool terminateLoop = false;

            while (!terminateLoop)
            {
                Dictionary<int, float> nextStateValueFunc = new Dictionary<int, float>();
                float valueFunctionUpdateAmount = 0f;

                foreach(KeyValuePair<int, float> valueFunctionEntry in stateValueFunc)
                {
                    float updatedValue = UpdateValueFunction(valueFunctionEntry.Key);
                    float updatedAmount = Math.Abs(valueFunctionEntry.Value - updatedValue);
                    nextStateValueFunc[valueFunctionEntry.Key] = updatedValue;

                    if (updatedAmount > valueFunctionUpdateAmount)
                        valueFunctionUpdateAmount = updatedAmount;
                }

                stateValueFunc = new Dictionary<int, float>(nextStateValueFunc);
                loopCount++;
                Console.WriteLine($"DP {loopCount} Implement, Update Error {valueFunctionUpdateAmount}");
                if (valueFunctionUpdateAmount < 0.01f)
                    terminateLoop = true;
            }
            Console.WriteLine(Environment.NewLine);
            Console.Write("Press Any Keys");
            Console.ReadLine();
        }

        public float UpdateValueFunction(int gameStateKey)
        {
            GameState gameState = new GameState(gameStateKey);

            if (gameState.IsFinalState())
                return 0.0f;

            List<float> actionExpectationList = new List<float>();

            for (int i = GameParameters.actionMinIdx; i<= GameParameters.actionMaxIdx; i++)
            {
                if (gameState.IsValidMove(i))
                {
                    GameState nextState = gameState.GetNextState(i);
                    float reward = nextState.GetReward();
                    float actionExpectation = reward + discountFactor * stateValueFunc[nextState.boardStateKey];
                    actionExpectationList.Add(actionExpectation);
                }
            }

            if (actionExpectationList.Count > 0)
            {
                if (gameState.nextTurn == 1)
                    return actionExpectationList.Max();
                else if (gameState.nextTurn == 2)
                    return actionExpectationList.Min();
            }
            return 0f;
        }

        private void ResetStateCount()
        {
            num00 = 0;
            num10 = 0;
            num11 = 0;
            num21 = 0;
            num22 = 0;
            num32 = 0;
            num33 = 0;
            num43 = 0;
            num44 = 0;
        }
    }
}
