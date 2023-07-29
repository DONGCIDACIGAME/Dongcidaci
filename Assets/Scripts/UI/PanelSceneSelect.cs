using System.Collections.Generic;
using GameEngine;
using UnityEngine.UI;
using TMPro;

public class PanelSceneSelect : UIPanel
{
    private Button Btn_EnterDemoScene;
    private Button Btn_EnterMonsterAIEditor;
    private TMP_InputField InputField_AudioSpeed;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_STATIC;
    }

    private void OnClickDemoSceen()
    {
        GameSceneManager.Ins.LoadAndSwitchToScene(SceneDefine.Demo);
    }

    private void OnClickMonsterAIEditor()
    {
        UIManager.Ins.OpenPanel<PanelAIEditor>("Prefabs/UI/AgentAIEditor/Panel_AgentAIEditor");
    }

    protected override void BindUINodes()
    {
        Btn_EnterDemoScene = BindButtonNode("Container/Btn_DemoScene", OnClickDemoSceen);
        Btn_EnterMonsterAIEditor = BindButtonNode("Container/Btn_MonsterAIEditor", OnClickMonsterAIEditor);
        InputField_AudioSpeed = BindInputFieldNode("InputField_AudioSpeed", OnAudioSppedChanged);
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
}
