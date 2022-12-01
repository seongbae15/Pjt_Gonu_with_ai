using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName { get; private set; }
    public int onStoneCount { get; private set; }

    public GameObject havingStone { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        playerName = " ";
        havingStone = null;
    }

    public void PlaceStone(Transform stonePlaceTransform)
    {
        UpdateStoneCount();
    }

    private void UpdateStoneCount()
    {
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
        }
    }

    public void MoveStone(Transform pointTransform)
    {
        havingStone.transform.position = pointTransform.position;
        Debug.Log(pointTransform.position);
        havingStone = null;
    }

}
