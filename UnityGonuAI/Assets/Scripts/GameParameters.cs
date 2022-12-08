using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameStone
{
    BLACK = 1,
    WHITE = 2,
}

public class GameParameters
{
    public const int totalStateCount = 19560;
    public const int actionMinNumber = 0;
    public const int actionmaxNumber = 8;
    public readonly static List<int>[] endChecker = { new List<int>() { 0, 1, 2 },
                                    new List<int>() { 3, 4, 5 },
                                    new List<int>() { 6, 7, 8 },
                                    new List<int>() { 0, 3, 6 },
                                    new List<int>() { 1, 4, 7 },
                                    new List<int>() { 2, 5, 8 },
                                    new List<int>() { 0, 4, 8 },
                                    new List<int>() { 2, 4, 6 },
                                    };

    public readonly static Dictionary<int, List<int>> connectedPointsDict = new Dictionary<int, List<int>>{
                                                                        {0, new List<int> { 1, 3, 4 }},
                                                                        {1, new List<int> { 0, 2, 4 }},
                                                                        {2, new List<int> { 1, 4, 5 }},
                                                                        {3, new List<int> { 0, 4, 6 }},
                                                                        {4, new List<int> { 0, 1, 2, 3, 5, 6, 7, 8 }},
                                                                        {5, new List<int> { 2, 4, 8 }},
                                                                        {6, new List<int> { 3, 4, 7 }},
                                                                        {7, new List<int> { 4, 6, 8 }},
                                                                        {8, new List<int> { 4, 5, 7 }},
                                                                    };

}
