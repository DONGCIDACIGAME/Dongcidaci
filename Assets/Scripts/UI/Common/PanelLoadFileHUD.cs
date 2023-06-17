using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PanelLoadFileHUD : UIPanel
{
    private Button Btn_Quit;
    private Button Button_LoadFile;
    private GameObject Content_AllFiles;

    private ControlFileOption mCurSelectFile;
    private List<ControlFileOption> mAllFileOptions;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_TOP_DYNAMIC;
    }

    private void OnQuitClick()
    {
        UIManager.Ins.ClosePanel<PanelLoadFileHUD>();
    }

    private void OnLoadFileClick()
    {
        if (mCurSelectFile == null)
            return;

        string dirPath = mCurSelectFile.GetDirPath();
        string fileName = mCurSelectFile.GetFileName();

        string fullPath = dirPath + "/" + fileName;
        GameEventSystem.Ins.Fire("OnLoadFileClick", fullPath);

        OnQuitClick();
    }

    protected override void BindUINodes()
    {
        Btn_Quit = BindButtonNode("Button_QuitHUD", OnQuitClick);
        Button_LoadFile = BindButtonNode("Button_LoadFile", OnLoadFileClick);
        Content_AllFiles = BindNode("Content_AllFiles");
    }

    private void GetAllFilesUnder(DirectoryInfo di, string ext, List<FileInfo> allFiles)
    {
        FileInfo[] childFiles = di.GetFiles();
        for(int i = 0; i<childFiles.Length;i++)
        {
            if(childFiles[i].Name.EndsWith(ext))
            {
                allFiles.Add(childFiles[i]);
            }
        }

        DirectoryInfo[] childDis = di.GetDirectories();
        for(int i = 0; i < childDis.Length;i++)
        {
            DirectoryInfo childId = childDis[i];
            GetAllFilesUnder(childId, ext, allFiles);
        }
    }

    private void LoadAllAIFiles(string rootDirPath, string ext)
    {
        DirectoryInfo di = new DirectoryInfo(rootDirPath);
        if(di == null)
        {
            Log.Error(LogLevel.Normal, "LoadAllAIFiles Failed, no directory at path:{0}", rootDirPath);
            return;
        }

        List<FileInfo> allFiles = new List<FileInfo>();
       GetAllFilesUnder(di, ext, allFiles);

        for (int i = 0; i < allFiles.Count; i++)
        {
            FileInfo fi = allFiles[i];
            ControlFileOption option = UIManager.Ins.AddControl<ControlFileOption>(this,
                "Prefabs/UI/Common/Ctl_FileOption",
                Content_AllFiles,
                new Dictionary<string, object>
                {
                    {"directory", di.FullName },
                    {"fileName", fi.Name },
                    {"ext", ext }
                });

            if(option != null)
            {
                mAllFileOptions.Add(option);
            }
        }
    }

    private void OnFileOptionSelect(ControlFileOption selectFile)
    {
        mCurSelectFile = selectFile;

        foreach(ControlFileOption option in mAllFileOptions)
        {
            option.Deselect();
        }

        selectFile.Select();
    }

    protected override void BindEvents()
    {
        base.BindEvents();
        mEventListener.Listen<ControlFileOption>("LoadFileHUDSelectFile", OnFileOptionSelect);
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        string rootDir = openArgs["root_dir"] as string;
        string fileExt = openArgs["ext"] as string;

        mAllFileOptions = new List<ControlFileOption>();
        LoadAllAIFiles(rootDir, fileExt);
    }

    protected override void OnClose()
    {

    }
}
