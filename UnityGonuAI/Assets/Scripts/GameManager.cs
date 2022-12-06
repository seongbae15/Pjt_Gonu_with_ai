using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private int turn;
    public int phase { private set; get; }
    public bool isGameEnd { private set; get; }
    [SerializeField]
    private BoardManager boardManager;
    [SerializeField]
    private Player[] players = new Player[2];
    [SerializeField]
    private GameObject[] stones = new GameObject[2];

    private int maxStoneLimit = 8;
    private List<int>[] checks = { new List<int>() { 0, 1, 2 },
                                    new List<int>() { 3, 4, 5 },
                                    new List<int>() { 6, 7, 8 },
                                    new List<int>() { 0, 3, 6 },
                                    new List<int>() { 1, 4, 7 },
                                    new List<int>() { 2, 5, 8 },
                                    new List<int>() { 0, 4, 8 },
                                    new List<int>() { 2, 4, 6 },
                                    };

    private DPManager dpManagper = new DPManager();
    private SARSAManager sarsaManager = new SARSAManager();
    private QLearningManager qlearningManager = new QLearningManager();


    private void Awake()
    {
        PlayerType blackType = GetPlayerType(PlayerPrefs.GetInt("Black"));
        PlayerType whiteType = GetPlayerType(PlayerPrefs.GetInt("White"));

        if (blackType == PlayerType.DP || whiteType == PlayerType.DP)
        {
            dpManagper.InitValueFunction();
        }
        if (blackType == PlayerType.SARSA || whiteType == PlayerType.SARSA)
        {
            sarsaManager.InitValueFunction();
        }
        if (blackType == PlayerType.QLEARNING || whiteType == PlayerType.QLEARNING)
        {
            qlearningManager.InitValueFunction();
        }

        //Player 상태 초기화
        players[1].Init(blackType);
        players[0].Init(whiteType);

    }

    private PlayerType GetPlayerType(int playerTypeNumber)
    {
        return (PlayerType)playerTypeNumber;
    }

    void Start()
    {
        turn = 1;
        phase = 1;
        isGameEnd = false;
    }

    public void PlaceStone(Transform pointTransform)
    {
        if (GetTotalStoneCount() == maxStoneLimit)
        {
            // Phase 2
            if (players[turn % 2].havingStone)
            {
                int curPoint = players[turn % 2].havingStone.GetComponent<Stone>().stonePositionNumber;
                int nextPoint = pointTransform.gameObject.GetComponent<Point>().GetPointNumber();
                if (boardManager.IsValidConnection(curPoint, nextPoint))
                {
                    players[turn % 2].MoveStone(pointTransform);

                    isGameEnd = CheckGameEndState();
                    if (!isGameEnd)
                    {
                        turn++;
                    }
                    else
                    {
                        UIManager.Instance.DisplayGameEndScreen(turn);
                    }
                }
            }
        }
        else
        {
            GameObject stone = Instantiate(stones[turn % 2], pointTransform);
            players[turn % 2].PlaceStone(stone, pointTransform.gameObject.GetComponent<Point>().GetPointNumber());
            
            isGameEnd = CheckGameEndState();
            if (!isGameEnd)
            {
                if (GetTotalStoneCount() == maxStoneLimit)
                {
                    phase = 2;
                    UIManager.Instance.UpdatePhase(phase);
                }
                turn++;
            }
            else
            {
                UIManager.Instance.DisplayGameEndScreen(turn);
            }

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

    private bool CheckGameEndState()
    {
        List<int> stonePositions = players[turn % 2].GetStonePositions();
        if (stonePositions.Count >= 3)
        {
            for (int i = 0; i < checks.Length; i++)
            {
                if (checks[i].All(checkPoint => stonePositions.Contains(checkPoint)))
                    return true;
            }
        }
        return false;
    }
}
