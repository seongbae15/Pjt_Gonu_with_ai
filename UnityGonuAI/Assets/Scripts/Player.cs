using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName { get; private set; }
    public int onBoardStoneCount { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        playerName = " ";    
    }

    public void UpdateStoneCount()
    {
        onBoardStoneCount++;
    }
}
