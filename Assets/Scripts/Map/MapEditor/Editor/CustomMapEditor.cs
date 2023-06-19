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
        

        if (GUILayout.Button("生成地面索引值"))
        {
            

        }

        GUILayout.Space(20f);

        if (GUILayout.Button("生成地图数据"))
        {
            if (EditorUtility.DisplayDialog("重新导入tile", "确认清除原数据?", "YES", "NO"))
            {

            }
            else
            {

            }

        }

        

    }
}
