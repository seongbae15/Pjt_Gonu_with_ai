using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    private GameState gonuState = new GameState();

    // 삭제 예정 변수
    public int phase { private set; get; }
    public bool isGameEnd { private set; get; }

    [SerializeField]
    private BoardManager boardManager;
    [SerializeField]
    private Player[] players = new Player[2];
    [SerializeField]
    private GameObject[] stones = new GameObject[2];
    [SerializeField]
    private DPManager dpManager;

    
    private int turn;
    //private int maxStoneLimit = 8;
    private List<int>[] checks = { new List<int>() { 0, 1, 2 },
                                    new List<int>() { 3, 4, 5 },
                                    new List<int>() { 6, 7, 8 },
                                    new List<int>() { 0, 3, 6 },
                                    new List<int>() { 1, 4, 7 },
                                    new List<int>() { 2, 5, 8 },
                                    new List<int>() { 0, 4, 8 },
                                    new List<int>() { 2, 4, 6 },
                                    };


    private void Awake()
    {
        PlayerType blackType = GetPlayerType(PlayerPrefs.GetInt("Black"));
        PlayerType whiteType = GetPlayerType(PlayerPrefs.GetInt("White"));

        // Init AI
        if (blackType == PlayerType.DP || whiteType == PlayerType.DP)
        {
            dpManager.ApplyTrain();
        }
        if (blackType == PlayerType.SARSA || whiteType == PlayerType.SARSA)
        {

        }
        if (blackType == PlayerType.QLEARNING || whiteType == PlayerType.QLEARNING)
        {
            
        }

        turn = 1;
        phase = 1;
        isGameEnd = false;

        //Player 상태 초기화
        players[1].Init(blackType);
        players[0].Init(whiteType);

    }

    private PlayerType GetPlayerType(int playerTypeNumber)
    {
        return (PlayerType)playerTypeNumber;
    }

    private void Update()
    {
        if (!isGameEnd)
        {
            // AI turn 일 때, 작업.
            int aiMove = 0;
            if (players[turn % 2].playerType != PlayerType.HUMAN)
            {
                switch (players[turn % 2].playerType)
                {
                    case PlayerType.DP:
                        aiMove = dpManager.GetNextMove(gonuState.boardStateKey);
                        break;
                    case PlayerType.SARSA:
                        break;
                    case PlayerType.QLEARNING:
                        break;
                }
                gonuState.MakeMove(aiMove);
                Transform pointTransform = GameParameters.Instance.GetPointTransform(aiMove);
                GameObject stone = null;
                stone = Instantiate(stones[turn % 2], pointTransform);


                //players[turn % 2].PlaceStone(stone, pointTransform.gameObject.GetComponent<Point>().GetPointNumber());

                turn++;


                //isGameEnd = CheckGameEndState();

            }
        }
    }


    public void PlaceStone(Transform pointTransform)
    {
        if (gonuState.gameWinner == 0)
        {
            if (gonuState.IsValidFirstPhase())
            {
                int pointNumber = pointTransform.gameObject.GetComponent<Point>().GetPointNumber();
                gonuState.MakeMove(pointNumber);
                GameObject stone = Instantiate(stones[turn % 2], pointTransform.position, pointTransform.localRotation);
                stone.GetComponent<Stone>().UpdateStoneInfo(pointNumber, turn);

                CheckGameEnd();
            }
        }
    }

    public void MoveStone(Transform stoneTransform)
    {
        if (gonuState.IsValidSecondPhase())
        {
            Stone selectedStone = stoneTransform.gameObject.GetComponent<Stone>();
            if ((selectedStone.stoneColor % 2) == (turn % 2))
            {
                if (gonuState.IsValidMove(selectedStone.stonePositionNumber))
                {
                    gonuState.MakeMove(selectedStone.stonePositionNumber);
                    //Destroy(stoneTransform.gameObject);
                    Transform newPointTransform = GameParameters.Instance.GetPointTransform(gonuState.temp - 1);
                    //GameObject stone = Instantiate(stones[turn % 2], newPointTransform.position, newPointTransform.localRotation);
                    stoneTransform.position = newPointTransform.position;
                    stoneTransform.gameObject.GetComponent<Stone>().UpdateStoneInfo(gonuState.temp);
                    CheckGameEnd();
                }
            }
        }
    }

    private void CheckGameEnd()
    {
        if (gonuState.IsFinalState())
        {
            UIManager.Instance.DisplayGameEndScreen(gonuState.gameWinner);
        }
        else
        {
            if (gonuState.IsValidSecondPhase())
            {
                phase = 2;
                UIManager.Instance.UpdatePhase(phase);
            }
            turn++;
        }
    }
}
