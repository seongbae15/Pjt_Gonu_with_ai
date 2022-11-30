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
    [SerializeField]
    private Player[] players = new Player[2];
    private int totalStoneCount = 0;
    private int maxStoneLimit = 8;
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

    public void PlaceStone(Transform stonePlaceTransform)
    {
        string playerName = "";
        // Phase 1
        if (turn % 2 == 0)
        {
            playerName = "white";
        }
        else
        {
            playerName = "black";
        }
        if (totalStoneCount == maxStoneLimit)
        {
            // Phase 2
            Debug.Log("This is Phase 2");
        }
        else
        {
            players[turn % 2].PlaceStone(stonePlaceTransform);
            totalStoneCount++;
            Debug.Log($"{turn} : {playerName} / playerOnStone : {players[turn % 2].onStoneCount} /total stone : {totalStoneCount}");
            turn++;
        }
    }

    public void SelectStone(Collider2D collider)
    {
        if (totalStoneCount == maxStoneLimit)
        {
            string color = "";
            switch (turn % 2)
            {
                case 0:
                    color = "white";
                    break;
                case 1:
                    color = "black";
                    break;
            }
            Debug.Log($"{turn} : {color} Stone Select");
            turn++;
        }
    }
}
