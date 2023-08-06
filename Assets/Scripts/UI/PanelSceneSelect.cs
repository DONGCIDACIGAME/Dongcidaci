using System.Collections.Generic;
using GameEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelSceneSelect : UIPanel
{
    private Button Btn_EnterDemoScene;
    private Button Btn_EnterMonsterAIEditor;
    private TMP_InputField InputField_AudioSpeed;
    private Button Btn_MusicMeterEditor;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_STATIC;
    }

    private void OnClickDemoSceen()
    {
        Dictionary<string, object> args = new Dictionary<string, object>()
        {
            { "root_dir", "Data/Meter"},
            { "ext",".meter"},
            { "loadEvent", "OnLoadMeterFile"}
        };

        UIManager.Ins.OpenPanel<PanelLoadFileHUD>("Prefabs/UI/Common/Panel_LoadFileHUD", args);
    }

    private void OnClickMonsterAIEditor()
    {
        UIManager.Ins.OpenPanel<PanelAIEditor>("Prefabs/UI/AgentAIEditor/Panel_AgentAIEditor");
    }

    private void OnClickMusicMeterEditor()
    {
        GameSceneManager.Ins.LoadAndSwitchToScene(SceneDefine.MusicEditor);
    }

    protected override void BindUINodes()
    {
        Btn_EnterDemoScene = BindButtonNode("Container/Btn_DemoScene", OnClickDemoSceen);
        Btn_EnterMonsterAIEditor = BindButtonNode("Container/Btn_MonsterAIEditor", OnClickMonsterAIEditor);
        InputField_AudioSpeed = BindInputFieldNode("InputField_AudioSpeed", OnAudioSppedChanged);
        Btn_MusicMeterEditor = BindButtonNode("Container/Btn_MusicMeterEditor", OnClickMusicMeterEditor);
    }

    private void OnAudioSppedChanged(string value)
    {
        float speed = float.Parse(value);
        AudioManager.Ins.ChangeBGMSpeed(speed);
    }


    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        Log.Logic(LogLevel.Info, "panel select scene open....");
    }

    private void OnLoadMeterFile(string fileFullPath)
    {
        string[] temp = fileFullPath.Split("/");
        string fileName = temp[temp.Length - 1].Replace(".meter", "");
        Dictionary<string, object> args = new Dictionary<string, object>()
        {
            { "music",fileName }
        };
        GameSceneManager.Ins.LoadAndSwitchToScene(SceneDefine.Demo, args);
        UIManager.Ins.ClosePanel<PanelSceneSelect>();
    }

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<string>("OnLoadMeterFile", OnLoadMeterFile);
    }
}
