using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGroundView))]
public class MapGroundViewEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapGroundView myScript = (MapGroundView)target;

        GUILayout.Space(20f);
        if (GUILayout.Button("生成地图索引"))
        {

            myScript.GenerateMapIndexs();

        }

        

    }
}
