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
    private int turn;
    private PlayerType[] playerTypes = new PlayerType[2];
    private GameObject[] stonesOnBoard = new GameObject[9];

    [SerializeField]
    private GameObject[] stones = new GameObject[2];
    [SerializeField]
    private DPManager dpManager;

    public int phase { private set; get; }
    public bool isGameEnd { private set; get; }


    private void Awake()
    {
        PlayerType whiteType = GetPlayerType(PlayerPrefs.GetInt("White"));
        PlayerType blackType = GetPlayerType(PlayerPrefs.GetInt("Black"));
        playerTypes[0] = GetPlayerType(PlayerPrefs.GetInt("White"));
        playerTypes[1] = GetPlayerType(PlayerPrefs.GetInt("Black"));

        // Init AI
        if (playerTypes[0] == PlayerType.DP || playerTypes[1] == PlayerType.DP)
        {
            dpManager.ApplyTrain();
        }
        if (playerTypes[0] == PlayerType.SARSA || playerTypes[1] == PlayerType.SARSA)
        {

        }
        if (playerTypes[0] == PlayerType.QLEARNING || playerTypes[1] == PlayerType.QLEARNING)
        {
            
        }

        turn = 1;
        phase = 1;
    }

    private PlayerType GetPlayerType(int playerTypeNumber)
    {
        return (PlayerType)playerTypeNumber;
    }

    private void Update()
    {
        if (gonuState.gameWinner == 0)
        {
            int aiMove = 0;
            if (playerTypes[turn % 2] != PlayerType.HUMAN)
            {
                switch (playerTypes[turn % 2])
                {
                    case PlayerType.DP:
                        aiMove = dpManager.GetNextMove(gonuState.boardStateKey);
                        break;
                    case PlayerType.SARSA:
                        break;
                    case PlayerType.QLEARNING:
                        break;
                }
                if (gonuState.IsValidFirstPhase())
                {
                    gonuState.MakeMove(aiMove);
                    Transform newPointTransform = GameParameters.Instance.GetPointTransform(aiMove);
                    GameObject stone = Instantiate(stones[turn % 2], newPointTransform.position, newPointTransform.localRotation);
                    stone.GetComponent<Stone>().UpdateStoneInfo(aiMove, turn);
                    stonesOnBoard[aiMove - 1] = stone;
                    CheckGameEnd();
                }
                else if (gonuState.IsValidSecondPhase())
                {
                    GameObject selectedStoneObject = stonesOnBoard[aiMove - 1];
                    Stone selectedStone = selectedStoneObject.GetComponent<Stone>();
                    if ((selectedStone.stoneColor % 2) == (turn % 2))
                    {
                        if (gonuState.IsValidMove(selectedStone.stonePositionNumber))
                        {
                            gonuState.MakeMove(aiMove);
                            Transform newPointTransform = GameParameters.Instance.GetPointTransform(gonuState.temp);
                            selectedStoneObject.transform.position = newPointTransform.position;
                            selectedStone.UpdateStoneInfo(gonuState.temp);
                            stonesOnBoard[selectedStone.stonePositionNumber - 1] = null;
                            stonesOnBoard[gonuState.temp - 1] = selectedStoneObject;
                            CheckGameEnd();
                        }
                    }
                }
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
                stonesOnBoard[pointNumber - 1] = stone;
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
                    Transform newPointTransform = GameParameters.Instance.GetPointTransform(gonuState.temp);
                    stoneTransform.position = newPointTransform.position;
                    selectedStone.UpdateStoneInfo(gonuState.temp);
                    stonesOnBoard[selectedStone.stonePositionNumber - 1] = null;
                    stonesOnBoard[gonuState.temp - 1] = stoneTransform.gameObject;
                    CheckGameEnd();
                }
            }
        }
    }

    private void CheckGameEnd()
    {
        if (gonuState.IsFinalState())
        {
            isGameEnd = true;
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
