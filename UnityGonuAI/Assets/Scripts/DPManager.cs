using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPManager : LearningManager
{
    private Dictionary<int, float> stateValueFunction = new Dictionary<int, float>();

    private int[] states = new int[9];


    public override void InitValueFunction()
    {
        Debug.Log("Start Initialization DP");
        ResetStateCount();
        stateValueFunction.Clear();

        // 모든 상태에 대한 가치함수 초기화
        for (int i = 0; i <= GameParameters.stateCount; i++)
        {
            GameState gameState = new GameState();
            gameState.CreateState(i);

            if (gameState.IsValidSecondPhase())
            {
                stateValueFunction.Add(i * 3 + 1, 0.0f);
                stateValueFunction.Add(i * 3 + 2, 0.0f);
                states[8]++;
            }
            else if (gameState.IsValidFirstPhase())
            {
                stateValueFunction.Add(i * 3 + gameState.GetFirstStageTurn(), 0f);

                if (gameState.numberOfBlacks == 0 && gameState.numberOfWhites == 0)
                {
                    states[0]++;
                }
                else if (gameState.numberOfBlacks == 1 && gameState.numberOfWhites == 0)
                {
                    states[1]++;
                }
                else if (gameState.numberOfBlacks == 1 && gameState.numberOfWhites == 1)
                {
                    states[2]++;
                }
                else if (gameState.numberOfBlacks == 2 && gameState.numberOfWhites == 1)
                {
                    states[3]++;
                }
                else if (gameState.numberOfBlacks == 2 && gameState.numberOfWhites == 2)
                {
                    states[4]++;
                }
                else if (gameState.numberOfBlacks == 3 && gameState.numberOfWhites == 2)
                {
                    states[5]++;
                }
                else if (gameState.numberOfBlacks == 3 && gameState.numberOfWhites == 3)
                {
                    states[6]++;
                }
                else if (gameState.numberOfBlacks == 4 && gameState.numberOfWhites == 3)
                {
                    states[7]++;
                }

            }
        }

        Debug.Log($"Black : 0, White : 0 = {states[0]}");
        Debug.Log($"Black : 1, White : 0 = {states[1]}");
        Debug.Log($"Black : 1, White : 1 = {states[2]}");
        Debug.Log($"Black : 2, White : 1 = {states[3]}");
        Debug.Log($"Black : 2, White : 2 = {states[4]}");
        Debug.Log($"Black : 3, White : 2 = {states[5]}");
        Debug.Log($"Black : 3, White : 3 = {states[6]}");
        Debug.Log($"Black : 4, White : 3 = {states[7]}");
        Debug.Log($"Black : 4, White : 4 = {states[8]}");
        Debug.Log("End Initialization DP");
    }

    public override void StartLearning()
    {
        Debug.Log("Start DP Learning");

    }

    private void ResetStateCount()
    {
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = 0;
        }

    }
}
