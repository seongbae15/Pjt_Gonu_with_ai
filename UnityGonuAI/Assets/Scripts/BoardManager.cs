using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private Dictionary<int, List<int>> connectionPoints;
    // Start is called before the first frame update
    void Start()
    {
        connectionPoints = InitBoardConnection();
    }


    private Dictionary<int, List<int>> InitBoardConnection()
    {
        Dictionary<int, List<int>> connectionPoints = new Dictionary<int, List<int>>();
        connectionPoints.Add(0, new List<int> { 1, 3, 4 });
        connectionPoints.Add(1, new List<int> { 0, 2, 4 });
        connectionPoints.Add(2, new List<int> { 1, 4, 5 });
        connectionPoints.Add(3, new List<int> { 0, 4, 6 });
        connectionPoints.Add(4, new List<int> { 0, 1, 2, 3, 5, 6, 7, 8 });
        connectionPoints.Add(5, new List<int> { 2, 4, 8 });
        connectionPoints.Add(6, new List<int> { 3, 4, 7 });
        connectionPoints.Add(7, new List<int> { 4, 6, 8 });
        connectionPoints.Add(8, new List<int> { 4, 5, 7 });

        return connectionPoints;
    }

    public bool IsValidConnection(int curPoint, int nextPoint)
    {
        if (connectionPoints[curPoint].Contains(nextPoint))
            return true;
        else
            return false;
    }
}
