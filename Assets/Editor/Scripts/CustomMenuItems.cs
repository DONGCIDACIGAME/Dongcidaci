using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomMenuItems : MonoBehaviour
{
    [MenuItem("Tools/Music", false, 10)]
    public static void OpenMusicEditor()
    {
        Editor_Music window = EditorWindow.GetWindow(typeof(Editor_Music), true, "Music Editor version 0.0.1") as Editor_Music;
        if(window != null)
        {
            window.Initialize();
            window.Show();
            window.Focus();
        }
    }
}
