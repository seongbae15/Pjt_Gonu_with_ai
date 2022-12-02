using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int stonePositionNumber { private set; get; }

    private void Start()
    {
    }
    public void UpdateStonePositionNumber(int pointNumber)
    {
        stonePositionNumber = pointNumber;
    }

    public int GetStonePositionNumber()
    {
        return stonePositionNumber;
    }

}
