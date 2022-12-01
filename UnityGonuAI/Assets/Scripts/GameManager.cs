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

    public void PlaceStone(Transform pointTransform)
    {
        if (GetTotalStoneCount() == maxStoneLimit)
        {
            // Phase 2
            if (players[turn % 2].havingStone)
            {
                players[turn % 2].MoveStone(pointTransform);
                turn++;
            }
            else
            {
                Debug.Log($"{turn}");
                Debug.Log("Select Stone!");
            }
        }
        else
        {
            Instantiate(stones[turn % 2], pointTransform);
            players[turn % 2].PlaceStone(pointTransform);
            if (GetTotalStoneCount() == maxStoneLimit)
            {
                phase = 2;
                UIManager.Instance.UpdatePhase(phase);
            }
            turn++;
        }
    }

    public void SelectStone(Collider2D collider)
    {
        if (GetTotalStoneCount() == maxStoneLimit)
        {
            if ((collider.CompareTag("BlackStone") && turn % 2 == 1) || (collider.CompareTag("WhiteStone") && turn % 2 == 0))
            {
                players[turn % 2].UpdateStoneSelection(collider.gameObject);
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
