using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerType playerType { get; private set; }
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

    public void SetPlayerType(PlayerType playerType)
    {
        this.playerType = playerType;
        if (this.playerType != PlayerType.HUMAN)
        {
            Debug.Log("I'm AI");
            switch (playerType)
            {
                case PlayerType.DP:
                    break;
                case PlayerType.SARSA:
                    break;
                case PlayerType.QLEARNING:
                    break;                   
            }
        }
        else
        {
            Debug.Log("I'm Human");
        }
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

    public List<int> GetStonePositions()
    {
        List<int> stonePositions = new List<int>();
        for (int i = 0; i < stones.Length; i++)
        {
            if (stones[i] != null)
            {
                stonePositions.Add(i);
            }
        }
        return stonePositions;
    }
}
