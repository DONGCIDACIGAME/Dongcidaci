using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using TMPro;
using UnityEngine.UI;

public class ControlFileOption : UIControl
{
    private TMP_Text Text_FileName;
    private GameObject Image_SelectBackground;
    private Button Button_SelectFile;

    private string mDirectoryPath;
    private string mFileName;


    protected override void BindUINodes()
    {
        Text_FileName = BindTextNode("Text_FileName");
        Image_SelectBackground = BindNode("Image_SelectBackground");
        Button_SelectFile = BindButtonNode("Button_SelectFile", OnClickButtonSelectFile);
    }

    public string GetDirPath()
    {
        return mDirectoryPath;
    }

    public string GetFileName()
    {
        return mFileName;
    }

    private void OnClickButtonSelectFile()
    {
        GameEventSystem.Ins.Fire("LoadFileHUDSelectFile", this);
    }

    public void Select()
    {
        Image_SelectBackground.SetActive(true);
    }

    public void Deselect()
    {
        Image_SelectBackground.SetActive(false);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        mDirectoryPath = openArgs["directory"] as string;
        mFileName = openArgs["fileName"] as string;
        string ext = openArgs["ext"] as string;

        Text_FileName.text = mFileName.Replace(ext, "");
        Image_SelectBackground.SetActive(false);
    }
}
