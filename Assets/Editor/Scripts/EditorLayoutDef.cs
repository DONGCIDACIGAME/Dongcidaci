using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorLayoutDef : MonoBehaviour
{
    public static GUILayoutOption[] Label_AutoExpand_LayoutOP = new GUILayoutOption[]
    {
        GUILayout.ExpandWidth(true),
        GUILayout.Width(100),
    };

    public static GUILayoutOption[] ResourceLable_AutoExpand_LayoutOP = new GUILayoutOption[]
    {
        GUILayout.ExpandWidth(true)
    };

    public static GUILayoutOption[] Text_AutoExpend_LayoutOP = new GUILayoutOption[]
    {
        GUILayout.ExpandWidth(true),
        GUILayout.MinWidth(200),
    };

    public static GUILayoutOption[] Btn_AutoExpend_LayoutOP = new GUILayoutOption[]
    {
            GUILayout.ExpandWidth(true),
            GUILayout.Width(50),
    };

    public static GUILayoutOption[] Btn_FullWidth_LayoutOP = new GUILayoutOption[]
    {
        GUILayout.ExpandWidth(true),
    };

    public static GUILayoutOption[] ScrollView_AutoExpend_LayoutOP = new GUILayoutOption[]
    {
        GUILayout.ExpandWidth(true),
        GUILayout.ExpandHeight(true),
        GUILayout.Width(100),
        GUILayout.Height(200),
    };
}
