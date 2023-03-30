using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_Music : EditorWindow
{
    private Rect TopArea = new Rect(0, 0, 1000, 100);
    private Rect MusicArea = new Rect(0, 100, 1000, 1000);

    private GUIStyle style;

    public void Initialize()
    {
        style = new GUIStyle();
        style.fixedHeight = 0;
        style.fixedWidth = 0;
        style.stretchHeight = true;
        style.stretchWidth = true;
    }

    private void OnGUI()
    {
        #region 选择音频文件
        GUILayout.BeginArea(new Rect(0,0,minSize.x, minSize.y),style );
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("选择音频文件:",EditorLayoutDef.Label_AutoExpand_LayoutOP);
        EditorGUILayout.TextField("", EditorLayoutDef.Text_AutoExpend_LayoutOP);
        bool selectMusic = GUILayout.Button("Select", EditorLayoutDef.Btn_AutoExpend_LayoutOP);
        if(selectMusic)
        {
            Editor_SelectMusic.Open();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

        GUILayout.BeginArea(MusicArea);

        GUILayout.EndArea();

        #endregion

    }
}
