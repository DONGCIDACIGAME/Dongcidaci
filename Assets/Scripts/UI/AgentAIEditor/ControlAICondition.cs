using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AI行为的连接条件
/// </summary>
public class ControlAICondition : UIControl
{
    private Button Button_Condition;

    private void OnButtonConditionClick()
    {

    }

    protected override void BindUINodes()
    {
        Button_Condition = BindButtonNode("Button_Condition", OnButtonConditionClick);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }
}
