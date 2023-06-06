using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class PanelMonsterAIEditor : UIPanel
{
    private ControlAILogicArea Ctl_AILogicArea;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_DYNAMIC;
    }

    protected override void BindUINodes()
    {
        Ctl_AILogicArea = BindControl<ControlAILogicArea>("Ctl_AILogicArea");
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
