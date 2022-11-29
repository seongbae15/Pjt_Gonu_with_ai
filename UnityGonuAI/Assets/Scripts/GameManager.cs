using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }


    public int turn { private set; get; }
    public int phase { private set; get; }


    // Start is called before the first frame update
    void Start()
    {
        turn = 1;
        phase = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void placeStone()
    {
        string playerName = "";
        // TurnÀÌ È¦¼ö Â¦¼ö ¿©ºÎ·Î Player 1, 2 ÆÇ´Ü
        if (turn % 2 == 0)
        {
            playerName = "whete";
        }
        else
        {
            playerName = "black";
        }
        Debug.Log($"{turn} : {playerName}");
        turn++;        
    }
}
