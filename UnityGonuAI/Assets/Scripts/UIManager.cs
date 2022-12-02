using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance = null;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private TextMeshProUGUI phaseText;
    [SerializeField]
    private TextMeshProUGUI[] playerNames = new TextMeshProUGUI[2];
    [SerializeField]
    private GameObject endDisp;
    [SerializeField]
    private TextMeshProUGUI endText;

    // Start is called before the first frame update
    void Start()
    {
        UpdatePhase(1);
        endDisp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePhase(int phase)
    {
        phaseText.text = $"Phase : {phase}";
    }

    public void DisplayGameEndScreen(int turn)
    {
        string winnerName = "";
        if (turn % 2 == 0)
        {
            winnerName = "White";
        }
        else
        {
            winnerName = "Black";
        }
        endText.text = $"{winnerName} is Winner!!!!";
        endDisp.SetActive(true);

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
