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
    [SerializeField]
    private GameObject[] stones = new GameObject[2];

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
        if (GetTotalStoneCount() == maxStoneLimit)
        {
            // Phase 2
            Debug.Log("This is Phase 2");
        }
        else
        {
            Instantiate(stones[turn % 2], stonePlaceTransform);
            players[turn % 2].PlaceStone(stonePlaceTransform);
            turn++;
        }
    }

    public void SelectStone(Collider2D collider)
    {
        if (GetTotalStoneCount() == maxStoneLimit)
        {
            if ((collider.CompareTag("BlackStone") && turn % 2 == 1) || (collider.CompareTag("WhiteStone") && turn % 2 == 0))
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
                // Move Stone
                //// Check hasStone
                players[turn % 2].UpdateHasStoneState(collider.gameObject);
                //// 

                //turn++;
            }
        }
    }

    private int GetTotalStoneCount()
    {
        int count = 0;
        for (int i = 0; i < players.Length; i++)
        {
            count += players[i].onStoneCount;
        }
        return count;
    }
}
