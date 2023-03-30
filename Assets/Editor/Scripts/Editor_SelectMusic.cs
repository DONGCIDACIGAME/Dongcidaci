using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Editor_SelectMusic : EditorWindow
{
    private string musicRootPath = "Assets/Audio/Music";
    private string[] allMusicPaths;
    private string[] musicNames;
    
    public static void Open()
    {
        Editor_SelectMusic window = GetWindow(typeof(Editor_SelectMusic), true, "Please select a music below") as Editor_SelectMusic;
        if(window != null)
        {
            window.Show();
            window.minSize = new Vector2(600, 700);
            window.maxSize = new Vector2(600, 700);
            window.Focus();

            window.Initialize() ;

        }
    }

    private void Initialize()
    {
        string[] files = AssetDatabase.FindAssets("t:AudioClip", new string[] { musicRootPath });
        allMusicPaths = new string[files.Length];
        musicNames = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string fullPath = AssetDatabase.GUIDToAssetPath(files[i]);
            allMusicPaths[i] = fullPath;

            int sepIndex = fullPath.LastIndexOf('/')+1;
            musicNames[i] = fullPath.Substring(sepIndex,fullPath.Length-sepIndex);
        }
    }

    private Vector2 scrollroot;
    private int selectIndex = -1;
    private Rect musicAssetArea = new Rect(0, 0, 600, 600);

    private int lastSelectIndex = -1;
    private float lastClickTime = 0;

    private void EnterMusicEdit(int index)
    {
        Editor_MusicEdit.Open(allMusicPaths[index]);
        this.Close();
    }

    private void OnMusicClick(int index)
    {
        if (!SelectionCheck())
            return;


        if (lastSelectIndex == selectIndex && Time.realtimeSinceStartup - lastClickTime < 0.5f)
        {
            EnterMusicEdit(selectIndex);
        }

        lastClickTime = Time.realtimeSinceStartup;
        lastSelectIndex = selectIndex;
    }


    private void Draw_SelectMusicArea()
    {
        GUILayout.BeginArea(musicAssetArea);
        scrollroot = GUILayout.BeginScrollView(scrollroot, true, true);
        for(int i=0;i<musicNames.Length;i++)
        {
            if(selectIndex == i)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.white;
            }
            bool clicked = GUILayout.Button(musicNames[i], EditorLayoutDef.Btn_FullWidth_LayoutOP);
            if(clicked)
            {
                selectIndex = i;
                OnMusicClick(selectIndex);
            }
        }
        //selectIndex = GUILayout.SelectionGrid(selectIndex, musicNames, 1);
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    private bool SelectionCheck()
    {
        return selectIndex >= 0 && selectIndex < allMusicPaths.Length;
    }

    private Rect musicInfoArea = new Rect(0, 600, 500, 100);
    private Rect menuBtnArea = new Rect(500, 600, 100, 100);
    private int lastShowMusicInfoIndex = -1;
    private bool editSelectMusic;
    private void Draw_SelectMusicInfo()
    {
        GUILayout.BeginArea(musicInfoArea);
        // index check
        if(!SelectionCheck())
        {
            GUILayout.EndArea();
            return;
        }

        string musicFullPath = allMusicPaths[selectIndex];

        //GUILayout.BeginArea(selectedMusicInfoArea, style);
        EditorGUILayout.LabelField(musicFullPath);

        // 如果选择的音乐没有变，不在重复读取
        if(lastShowMusicInfoIndex == selectIndex)
        {
            GUILayout.EndArea();
            return;
        }

        lastShowMusicInfoIndex = selectIndex;
        // 读取meta文件
        string metaFile = PathDefine.ROOT_PATH + "/" + musicFullPath + ".meta";

        if (File.Exists(metaFile))
        {
            //string[] metaInfo = File.ReadAllLines(metaFile);
            //for (int i = 0; i < metaInfo.Length; i++)
            //{
            //    Debug.Log(metaInfo[i]);
            //}

        }
        GUILayout.EndArea();
    }



    private void Draw_MenuBtn()
    {
        GUILayout.BeginArea(menuBtnArea);
        editSelectMusic = GUILayout.Button("Edit", EditorLayoutDef.Btn_FullWidth_LayoutOP);
        if (editSelectMusic)
        {
            EnterMusicEdit(selectIndex);
        }
        GUILayout.EndArea();


    }

    private void OnGUI()
    {
        Draw_SelectMusicArea();

        GUI.backgroundColor = Color.white;
        GUILayout.BeginArea(new Rect(0, 0, 100, 100));
        GUILayout.EndArea();

        GUILayout.BeginHorizontal();
        Draw_SelectMusicInfo();
        Draw_MenuBtn();
        GUILayout.EndHorizontal();
    }
}
