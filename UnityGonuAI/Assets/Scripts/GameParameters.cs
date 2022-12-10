using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStone
{
    BLACK = 1,
    WHITE = 2,
}

public class GameParameters : MonoBehaviour
{
    private static GameParameters instance = null;

    public static GameParameters Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameParameters>();
            }
            return instance;
        }
    }
    
    public const int totalStateCount = 19560;
    public const int actionMinNumber = 1;
    public const int actionMaxNumber = 9;
    //public readonly static List<int>[] endChecker = { new List<int>() { 0, 1, 2 },
    //                                new List<int>() { 3, 4, 5 },
    //                                new List<int>() { 6, 7, 8 },
    //                                new List<int>() { 0, 3, 6 },
    //                                new List<int>() { 1, 4, 7 },
    //                                new List<int>() { 2, 5, 8 },
    //                                new List<int>() { 0, 4, 8 },
    //                                new List<int>() { 2, 4, 6 },
    //                                };

    public readonly static Dictionary<int, List<int>> connectedPointsDict = new Dictionary<int, List<int>>{
                                                                        {1, new List<int> { 2, 4, 5 }},
                                                                        {2, new List<int> { 1, 3, 5 }},
                                                                        {3, new List<int> { 2, 5, 6 }},
                                                                        {4, new List<int> { 1, 5, 7 }},
                                                                        {5, new List<int> { 1, 2, 3, 4, 6, 7, 8, 9 }},
                                                                        {6, new List<int> { 3, 5, 9 }},
                                                                        {7, new List<int> { 4, 5, 8 }},
                                                                        {8, new List<int> { 5, 7, 9 }},
                                                                        {9, new List<int> { 5, 6, 8 }},
                                                                    };
    [SerializeField]
    private List<Transform> pointTransforms;

    public Transform GetPointTransform(int pointNumber)
    {
        return pointTransforms[pointNumber];
    }
}
