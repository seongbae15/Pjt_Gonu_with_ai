using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName { get; private set; }
    public int onStoneCount { get; private set; }

    [SerializeField]
    private GameObject stone;

    // Start is called before the first frame update
    void Start()
    {
        playerName = " ";    
    }

    public void PlaceStone(Transform stonePlaceTransform)
    {
        Instantiate(stone, stonePlaceTransform);
        Debug.Log("Create Stone");
        UpdateStoneCount();
    }

    private void UpdateStoneCount()
    {
        onStoneCount++;
    }
}
