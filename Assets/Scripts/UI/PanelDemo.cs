using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using UnityEngine.UI;
using TMPro;


public class PanelDemo : UIPanel
{
    private Button Btn_Start;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_NORMAL_STATIC;
    }

    private void OnClickStart()
    {
        AudioManager.Ins.PlayBgm(true);
    }

    protected override void BindUINodes()
    {
        Btn_Start = BindButtonNode("Btn_Start", OnClickStart);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
