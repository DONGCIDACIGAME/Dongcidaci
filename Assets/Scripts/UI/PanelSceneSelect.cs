using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using UnityEngine.UI;

public class PanelSceneSelect : UIPanel
{
    private Button Btn_EnterDemoScene;
    private Button Btn_EnterMonsterAIEditor;

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
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        Log.Logic(LogLevel.Info, "panel select scene open....");
    }
}
