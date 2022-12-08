using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DPManager : LearningManager
{
    private bool isLearning = false;
    private int[] stoneCountState = new int[9];
    private Dictionary<int, float> stateValueFunction = new Dictionary<int, float>();
    private float discountFactor = 0.9f;
    private static System.Random random = new System.Random();

    public override void ApplyTrain()
    {
        if (!isLearning)
        {
            Debug.Log("Start DP");
            InitValueFunction();
            ImplementValueIteration();
        }
        isLearning = true;
    }

    private void InitValueFunction()
    {
        Debug.Log("Initalize DP Value Function");
        ResetStoneCountState();
        stateValueFunction.Clear();

        for (int i = 0; i <= GameParameters.totalStateCount; i++)
        {
            GameState gameState = new GameState();
            gameState.BuildBoardState(i);

            if (gameState.IsValidSecondPhase())
            {
                stateValueFunction.Add(i * 3 + (int)GameStone.BLACK, 0f);
                stateValueFunction.Add(i * 3 + (int)GameStone.WHITE, 0f);
                stoneCountState[8]++;
            }
            else if (gameState.IsValidFirstPhase())
            {
                stateValueFunction.Add(i * 3 + gameState.GetTurnOfFirstPhase(), 0f);
                if (gameState.numberOfBlacks == 0 && gameState.numberOfWhites == 0)
                {
                    stoneCountState[0]++;
                }
                else if (gameState.numberOfBlacks == 1 && gameState.numberOfWhites == 0)
                {
                    stoneCountState[1]++;
                }
                else if (gameState.numberOfBlacks == 1 && gameState.numberOfWhites == 1)
                {
                    stoneCountState[2]++;
                }
                else if (gameState.numberOfBlacks == 2 && gameState.numberOfWhites == 1)
                {
                    stoneCountState[3]++;
                }
                else if (gameState.numberOfBlacks == 2 && gameState.numberOfWhites == 2)
                {
                    stoneCountState[4]++;
                }
                else if (gameState.numberOfBlacks == 3 && gameState.numberOfWhites == 2)
                {
                    stoneCountState[5]++;
                }
                else if (gameState.numberOfBlacks == 3 && gameState.numberOfWhites == 3)
                {
                    stoneCountState[6]++;
                }
                else if (gameState.numberOfBlacks == 4 && gameState.numberOfWhites == 3)
                {
                    stoneCountState[7]++;
                }
            }
        }
        Debug.Log("Complete Init DP Value Function");
        Debug.Log($"Black: 0, White: 0 - {stoneCountState[0]}");
        Debug.Log($"Black: 1, White: 0 - {stoneCountState[1]}");
        Debug.Log($"Black: 1, White: 1 - {stoneCountState[2]}");
        Debug.Log($"Black: 2, White: 1 - {stoneCountState[3]}");
        Debug.Log($"Black: 2, White: 2 - {stoneCountState[4]}");
        Debug.Log($"Black: 3, White: 2 - {stoneCountState[5]}");
        Debug.Log($"Black: 3, White: 3 - {stoneCountState[6]}");
        Debug.Log($"Black: 4, White: 3 - {stoneCountState[7]}");
        Debug.Log($"Black: 4, White: 4 - {stoneCountState[8]}");
    }

    private void ResetStoneCountState()
    {
        for (int i=0; i<stoneCountState.Length; i++)
        {
            stoneCountState[i] = 0;
        }
    }

    private void ImplementValueIteration()
    {
        Debug.Log("Implement DP");
        int loopCount = 0;
        bool terminateLoop = false;

        while (!terminateLoop)
        {
            Dictionary<int, float> nextStateValueFunction = new Dictionary<int, float>();
            float valueFunctionUpdateAmount = 0f;

            foreach(KeyValuePair<int, float> valueFunctionEntry in stateValueFunction)
            {
                float updateValue = UpdateValueFunction(valueFunctionEntry.Key);
                float updateAmount = Math.Abs(valueFunctionEntry.Value - updateValue);
                nextStateValueFunction[valueFunctionEntry.Key] = updateValue;
                if (updateAmount > valueFunctionUpdateAmount)
                    valueFunctionUpdateAmount = updateAmount;
            }
            stateValueFunction = new Dictionary<int, float>(nextStateValueFunction);
            loopCount++;
            Debug.Log($"DP {loopCount} Implement, Update Error {valueFunctionUpdateAmount}");
            if (valueFunctionUpdateAmount < 0.01f)
                terminateLoop = true;
        }
    }

    private float UpdateValueFunction(int gameStateKey)
    {
        GameState gameState = new GameState(gameStateKey);

        if (gameState.IsFinalState())
            return 0f;

        List<float> actionExpectationList = new List<float>();

        for (int i = GameParameters.actionMinNumber; i <= GameParameters.actionmaxNumber; i++)
        {
            if (gameState.IsValidMove(i))
            {
                GameState nextState = gameState.GetNextState(i);
                float reward = nextState.GetReward();
                float actionExpectation = reward + discountFactor * stateValueFunction[nextState.boardStateKey];
                actionExpectationList.Add(actionExpectation);
            }
        }

        if (actionExpectationList.Count > 0)
        {
            if (gameState.turn == 1)
                return actionExpectationList.Max();
            else if (gameState.turn == 2)
                return actionExpectationList.Min();
        }
        return 0f;
    }

    public int GetNextMove(int boardStateKey)
    {
        IEnumerable<int> actionCandidates = GetNextMoveCandidate(boardStateKey);
        if (actionCandidates.Count() == 0)
            return 0;
        return actionCandidates.ElementAt(random.Next(0, actionCandidates.Count()));
    }

    private IEnumerable<int> GetNextMoveCandidate(int boardStateKey)
    {
        float selectedExpectation = 0f;
        GameState gameState = new GameState(boardStateKey);
        Dictionary<int, float> actionCandidateDictionary = new Dictionary<int, float>();

        for (int i = GameParameters.actionMinNumber; i <= GameParameters.actionmaxNumber; i++)
        {
            if (gameState.IsValidMove(i))
            {
                GameState nextState = gameState.GetNextState(i);
                float reward = nextState.GetReward();
                float actionExpectation = reward + discountFactor * stateValueFunction[nextState.boardStateKey];
                actionCandidateDictionary.Add(i, actionExpectation);
            }
        }

        if (actionCandidateDictionary.Count == 0)
            return new List<int>();

        if (gameState.turn == (int)GameStone.BLACK)
        {
            selectedExpectation = actionCandidateDictionary.Select(e => e.Value).Max();
        }
        else if (gameState.turn == (int)GameStone.WHITE)
        {
            selectedExpectation = actionCandidateDictionary.Select(e => e.Value).Min();
        }
        return actionCandidateDictionary.Where(e => e.Value == selectedExpectation).Select(e => e.Key);
    }


}
