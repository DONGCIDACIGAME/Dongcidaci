using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using UnityEngine.UI;

public class PanelSceneSelect : UIPanel
{
    private Button Btn_EnterDemoScene;
    private Button Btn_EnterAudioEditorScene;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_STATIC;
    }

    private void OnClickDemoSceen()
    {
        GameSceneManager.Ins.SwitchToScene(SceneDefine.Demo);
    }

    private void OnClickAudioEditorScene()
    {

    }

    protected override void BindUINodes()
    {
        Btn_EnterDemoScene = BindButtonNode("Container/Btn_DemoScene", OnClickDemoSceen);
        Btn_EnterAudioEditorScene = BindButtonNode("Container/Btn_AudioEditorScene", OnClickAudioEditorScene);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        Log.Logic("panel select scene open....");
    }
}
