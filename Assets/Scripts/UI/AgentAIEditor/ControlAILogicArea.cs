using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class ControlAILogicArea : UIControl
{
    private GameObject Node_AILogicArea;

    protected override void BindUINodes()
    {
        Node_AILogicArea = BindNode("Node_AILogicArea");
    }


    private void BindEvents()
    {
        mEventListener.Listen("ToAIEditorDefaultPos", ToDefaultPos);
    }

    /// <summary>
    /// 移动回默认位置，默认缩放
    /// </summary>
    private void ToDefaultPos()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        BindEvents();
    }


    protected override void OnClose()
    {

    }
}
