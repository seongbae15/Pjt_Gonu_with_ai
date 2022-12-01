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

    // Start is called before the first frame update
    void Start()
    {
        UpdatePhase(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePhase(int phase)
    {
        phaseText.text = $"Phase : {phase}";
    }
}
