using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stone : MonoBehaviour
{
    public int stonePositionNumber { private set; get; }
    public int stoneColor { private set; get; }

    public void UpdateStoneInfo(int pointNumber, int turn)
    {
        stonePositionNumber = pointNumber;
        if (turn % 2 == 0)
        {
            stoneColor = (int)GameStone.WHITE;
        }
        else
        {
            stoneColor = (int)GameStone.BLACK;
        }
    }
}
