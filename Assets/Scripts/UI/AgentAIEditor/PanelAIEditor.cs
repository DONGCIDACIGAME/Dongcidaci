using GameEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelAIEditor : UIPanel
{
    private ControlAILogicArea Ctl_AILogicArea;
    private ControlAIEditorTopBar Ctl_TopBar;

    private BTTree mCurEditingTree;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_DYNAMIC;
    }

    protected override void BindUINodes()
    {
        Ctl_AILogicArea = BindControl<ControlAILogicArea>("Ctl_AILogicArea");
        Ctl_TopBar = BindControl<ControlAIEditorTopBar>("Ctl_TopBar");
    }


    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        mEventListener.Listen("OnClickLoadTree", OpenLoadTreeHUD);
    }


    public void OpenLoadTreeHUD()
    {
        UIManager.Ins.AddControl<ControlLoadTreeHUD>(this, "Prefabs/UI/AgentAIEditor/Ctl_LoadTreeHUD", this.mUIRoot, null);
    }


    protected override void OnClose()
    {

    }
}
