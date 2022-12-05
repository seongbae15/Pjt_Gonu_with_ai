using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerType
{
    HUMAN = 0,
    DP = 1,
    SARSA = 2,
    QLEARNING = 3,
}

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI menuSelectTitle;

    private int playerSelectioin = 1;
    private string playerColor = "Black";

    private void Awake()
    {
        PlayerPrefs.SetInt("Black", (int)PlayerType.HUMAN);
        PlayerPrefs.SetInt("White", (int)PlayerType.HUMAN);
    }
    private void Start()
    {
        menuSelectTitle.text = $"Select Player({playerColor})";
    }

    public void SetPlayerType(int playerType)
    {
        if (playerSelectioin >= 3)
            return;
        PlayerPrefs.SetInt($"{GetPlayerColorName()}", playerType);
        playerSelectioin++;

        if (playerSelectioin >= 3)
            return;
        menuSelectTitle.text = $"Select Player({GetPlayerColorName()})";

    }

    private string GetPlayerColorName()
    {
        if (playerSelectioin == 1)
            return "Black";
        else
            return "White";
        
    }

    public void PlayGame()
    {
        
        SceneManager.LoadScene("PlayScene");
    }
}
