using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private int[] boardState = new int[9];
    public int numberOfBlacks { private set; get; }
    public int numberOfWhites { private set; get; }

    private int turn;
    private int boardStateKey;
    private int gameWinner;

    void Start()
    {
        for (int i = 0; i < boardState.Length; i++)
        {
            boardState[i] = 0;
        }
        turn = 1;
        boardStateKey = 1;
        numberOfBlacks = 0;
        numberOfWhites = 0;

    }

    public void CreateState(int iState)
    {
        int boardStateNumber = iState;
        numberOfBlacks = 0;
        numberOfWhites = 0;
        for (int i = 8; i >= 0; i--)
        {
            int boardStoneNumber = boardStateNumber % 3;
            boardStateNumber = boardStateNumber / 3;

            boardState[i] = boardStoneNumber;
            if (boardStoneNumber == 1)
            {
                numberOfBlacks++;
            }
            else if (boardStoneNumber == 2)
            {
                numberOfWhites++;
            }
        }
    }

    public bool IsValidSecondPhase()
    {
        if (numberOfBlacks == 4 && numberOfWhites == 4)
            return true;
        else
            return false;
    }

    public bool IsValidFirstPhase()
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
        else if (numberOfBlacks == numberOfWhites + 1)
            return 2;
        else
            return 0;
    }
}