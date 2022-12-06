using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPManager : LearningManager
{
    public Dictionary<int, float> stateValueFunction { get; private set; }

    private int[] pos = new int[8];

    public DPManager()
    {
        stateValueFunction = new Dictionary<int, float>();
    }
    public override void InitValueFunction()
    {
        for (int i=0; i<pos.Length; i++)
        {
            pos[i] = 0;
        }
        stateValueFunction.Clear();
        Debug.Log("Init DP");
    }

    public override void StartLearning()
    {
        Debug.Log("Start DP");

    }
}
