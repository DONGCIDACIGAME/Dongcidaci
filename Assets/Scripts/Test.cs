using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test: MonoBehaviour
{
    [ContextMenu("SortTest")]
    public void SortTest()
    {
        List<int> testList = new List<int>{ 2, 54, 6, 2, 7, 8, 332, 3, 1 };

        for (int i = 0; i < testList.Count - 1; i++)
        {
            for (int j = i+1; j < testList.Count; j++)
            {
                int step1 = testList[i];
                int step2 = testList[j];

                if (step1 > step2)
                {
                    testList[i] = step2;
                    testList[j] = step1;
                }
            }
        }

        for(int i = 0;i<testList.Count;i++)
        {
            Debug.Log(testList[i]);
        }
    }
}
