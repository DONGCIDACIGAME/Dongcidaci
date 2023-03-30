using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Editor_MusicEdit : EditorWindow
{
    private static float width = Screen.currentResolution.width * 0.8f;
    private static float height = Screen.currentResolution.height * 0.8f;

    public static void Open(string musicFullPath)
    {
        Editor_MusicEdit window = GetWindow(typeof(Editor_MusicEdit)) as Editor_MusicEdit;
        if(window != null)
        {
            window.Show();
            window.Focus();
            
            window.minSize = new Vector2(width, height);
            window.maxSize = new Vector2(width, height);
            window.Initialize(musicFullPath);
        }
    }

    private string musicFilePath;
    private AudioClip clip;

    public void Initialize(string musicFullPath)
    {
        musicFilePath = musicFullPath;

        //clip = Resources.Load

    }

    private void OnGUI()
    {
        


    }

}
