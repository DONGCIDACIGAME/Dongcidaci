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

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
