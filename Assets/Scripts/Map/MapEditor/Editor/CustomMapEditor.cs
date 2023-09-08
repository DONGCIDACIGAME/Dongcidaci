using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomMap))]
public class CustomMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomMap myScript = (CustomMap)target;
        this.serializedObject.Update();

        #region MAP GRID CONFIG
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("gridColCount"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("gridRowCount"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("gridCellWidth"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("gridCellHeight"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("drawGridGizmos"));

        if (GUILayout.Button("刷新地图网格数据"))
        {
            myScript.CaculateGridCells();
        }
        EditorGUILayout.Separator();
        #endregion

        #region NAVI Info
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("naviSubLevel"));
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("drawNaviGrids"));


        if (GUILayout.Button("烘焙导航网格数据"))
        {
            myScript.CaculateNaviGridCells();
        }
        EditorGUILayout.Separator();
        #endregion




        this.serializedObject.ApplyModifiedProperties();


        /**
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
            myScript.SaveMapEventData();
        }
        

        if (GUILayout.Button("保存地图数据"))
        {
            myScript.SaveMapDataToDisk();
        }
        */
        

    }
}
