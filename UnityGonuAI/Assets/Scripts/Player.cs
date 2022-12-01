using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName { get; private set; }
    public int onStoneCount { get; private set; }

    private GameObject havingStone = null;

    // Start is called before the first frame update
    void Start()
    {
        playerName = " ";
    }

    public void PlaceStone(Transform stonePlaceTransform)
    {
        UpdateStoneCount();
    }

    private void UpdateStoneCount()
    {
        onStoneCount++;
    }

    public void UpdateHasStoneState(GameObject selectedGameObject)
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
}
