using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomMap))]
public class CustomMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CustomMap myScript = (CustomMap)target;

        if (GUILayout.Button("刷新地图网格"))
        {
            myScript.CaculateGridCells();

        }

        GUILayout.Space(20f);
        

        if (GUILayout.Button("生成所有地面索引值"))
        {
            myScript.SaveGridIndexToGround();

        }

        GUILayout.Space(20f);

        if (GUILayout.Button("填充事件数据"))
        {
            //myScript.SaveMapDataToDisk();
        }

        if (GUILayout.Button("保存地图数据"))
        {
            myScript.SaveMapDataToDisk();
        }

        

    }
}
