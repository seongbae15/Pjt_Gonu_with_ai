using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName { get; private set; }
    public int onStoneCount { get; private set; }

    public GameObject havingStone { get; private set; }

    private GameObject[] stones = new GameObject[9];

    // Start is called before the first frame update
    void Start()
    {
        playerName = " ";
        havingStone = null;
    }

    public void PlaceStone(GameObject stone, int pointNumber)
    {
        UpdateStoneInfo(stone, pointNumber);
    }

    private void UpdateStoneInfo(GameObject stone, int pointNumber)
    {
        stones[pointNumber] = stone;
        stones[pointNumber].GetComponent<Stone>().UpdateStonePositionNumber(pointNumber);
        onStoneCount++;
    }

    public void UpdateStoneSelection(GameObject selectedGameObject)
    {
        //hasStone = hasStone ? false : true;
        if (!havingStone)
        {
            havingStone = selectedGameObject;
        }
        else
        {
            if (havingStone == selectedGameObject)
            {
                havingStone = null;
            }
            else
            {
                havingStone = selectedGameObject;
            }
        }
    }

    public void MoveStone(Transform pointTransform)
    {
        havingStone.transform.position = pointTransform.position;
        int oldPointNumber = havingStone.gameObject.GetComponent<Stone>().stonePositionNumber;
        int newPointNumber = pointTransform.gameObject.GetComponent<Point>().GetPointNumber();
        havingStone.gameObject.GetComponent<Stone>().UpdateStonePositionNumber(oldPointNumber);
        stones[newPointNumber] = havingStone.gameObject;
        stones[oldPointNumber] = null;
        havingStone = null;
    }

}
