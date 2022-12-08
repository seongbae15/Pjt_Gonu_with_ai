using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    public int numberOfBlacks { private set; get; }
    public int numberOfWhites { private set; get; }

    public int boardStateKey { private set; get; }
    public int turn { private set; get; }

    private int[] boardState = new int[9] {0, 0, 0, 0, 0, 0, 0, 0, 0};
    private int gameWinner;

    public GameState()
    {
        boardState = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        turn = 1;
        boardStateKey = 1;
        numberOfBlacks = 0;
        numberOfWhites = 0;
        gameWinner = 0;
    }

    public GameState(int boardStateKey)
    {
        boardState = new int[9];
        this.boardStateKey = boardStateKey;
        turn = boardStateKey % 3;
        gameWinner = 0;
        BuildBoardState(boardStateKey / 3);
    }

    public void BuildBoardState(int boardState)
    {
        int curBoardState = boardState;
        numberOfBlacks = 0;
        numberOfWhites = 0;

        for (int i = 8; i >= 0; i--)
        {
            int stoneNumberOnBoard = curBoardState % 3;
            curBoardState /= 3;

            this.boardState[i] = stoneNumberOnBoard;

            if (stoneNumberOnBoard == (int)GameStone.BLACK)
            {
                numberOfBlacks++;
            }
            else if (stoneNumberOnBoard == (int)GameStone.WHITE)
            {
                numberOfWhites++;
            }
        }
    }

    public void UpdateCurGameState()
    {

    }

    public bool IsValidFirstPhase()
    {
        if (numberOfBlacks > 4)
            return false;
        else if (numberOfWhites > 3)
            return false;
        if (numberOfBlacks == numberOfWhites || numberOfBlacks == numberOfWhites + 1)
            return true;
        return false;
    }

    public bool IsValidSecondPhase()
    {
        if (numberOfBlacks == 4 && numberOfWhites == 4)
            return true;
        return false;
    }

    public int GetTurnOfFirstPhase()
    {
        if (numberOfBlacks == numberOfWhites)
            return (int)GameStone.BLACK;
        else if (numberOfBlacks == numberOfWhites + 1)
            return (int)GameStone.WHITE;
        else
            return 0;
    }

    public bool IsFinalState()
    {
        gameWinner = 0;
        if (boardState[0] == boardState[1] && boardState[1] == boardState[2])
        {
            if (boardState[0] != 0)
            {
                gameWinner = boardState[0];
                return true;
            }
        }
        else if (boardState[3] == boardState[4] && boardState[4] == boardState[5])
        {
            if (boardState[3] != 0)
            {
                gameWinner = boardState[3];
                return true;
            }
        }
        else if (boardState[6] == boardState[7] && boardState[7] == boardState[8])
        {
            if (boardState[6] != 0)
            {
                gameWinner = boardState[6];
                return true;
            }
        }
        else if (boardState[0] == boardState[3] && boardState[3] == boardState[6])
        {
            if (boardState[0] != 0)
            {
                gameWinner = boardState[0];
                return true;
            }
        }
        else if (boardState[1] == boardState[4] && boardState[4] == boardState[7])
        {
            if (boardState[1] != 0)
            {
                gameWinner = boardState[1];
                return true;
            }
        }
        else if (boardState[2] == boardState[5] && boardState[5] == boardState[8])
        {
            if (boardState[2] != 0)
            {
                gameWinner = boardState[2];
                return true;
            }
        }
        else if (boardState[0] == boardState[4] && boardState[4] == boardState[8])
        {
            if (boardState[0] != 0)
            {
                gameWinner = boardState[0];
                return true;
            }
        }
        else if (boardState[0] == boardState[4] && boardState[4] == boardState[8])
        {
            if (boardState[0] != 0)
            {
                gameWinner = boardState[0];
                return true;
            }
        }
        return false;
    }

    public bool IsValidMove(int curPoint)
    {
        if (IsValidFirstPhase())
        {
            if (boardState[curPoint] == 0)
                return true;
        }
        else if (IsValidSecondPhase())
        {
            if (boardState[curPoint] != turn)
                return false;

            IEnumerable<int> connectedPoints = GameParameters.connectedPointsDict[curPoint];
            IEnumerable<int> connectedEmptyPoints = connectedPoints.Where(p => boardState[p] == 0);
            
            if (connectedEmptyPoints.Count() > 0)
            {
                IEnumerable<int> connectedOpponents = connectedPoints.Where(p => boardState[p] != turn);
                if (connectedOpponents.Count() > 0)
                    return true;
            }
        }
        return false;
    }

    public GameState GetNextState(int curPoint)
    {
        GameState nextState = new GameState(boardStateKey);
        nextState.MakeMove(curPoint);
        return nextState;
    }

    public void MakeMove(int curPoint)
    {
        if (IsValidFirstPhase())
        {
            boardState[curPoint] = turn;
            if (turn == 1)
            {
                numberOfBlacks++;
            }
            else if(turn == 2)
            {
                numberOfWhites++;
            }
        }
        else if (IsValidSecondPhase())
        {
            IEnumerable<int> connectedPoints = GameParameters.connectedPointsDict[curPoint];
            int emptyPoint = 0;
            foreach (int connectedPoint in connectedPoints)
            {
                if (boardState[connectedPoint] == 0)
                {
                    emptyPoint = connectedPoint;
                    break;
                }
            }
            boardState[curPoint] = 0;
            boardState[emptyPoint] = turn;
        }
        if (turn == 1)
        {
            turn = 2;
        }
        else if (turn ==2)
        {
            turn = 1;
        }
        int curboardStateKey = 0;
        for (int i=GameParameters.actionMinNumber; i <= GameParameters.actionmaxNumber; i++)
        {
            curboardStateKey = curboardStateKey * 3;
            curboardStateKey = curboardStateKey + boardState[i];
        }
        boardStateKey = curboardStateKey * 3 + turn;
    }

    public float GetReward()
    {
        if (IsFinalState())
        {
            if (gameWinner == 1)
                return 100f;
            else if (gameWinner == 2)
                return -100f;
        }
        return 0f;
    }

}
