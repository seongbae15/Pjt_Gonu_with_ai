using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField]
    private int pointNumber;

    public int GetPointNumber()
    {
        return pointNumber;
    }
}
