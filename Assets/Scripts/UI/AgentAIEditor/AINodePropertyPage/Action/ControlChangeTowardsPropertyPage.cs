using TMPro;
using System.Collections.Generic;
using GameEngine;

public class ControlChangeTowardsPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_ChangeTowards;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Dropdown_ChangeTowards = BindDropDownNode("Dropdown_ChangeTowards", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("Undefined"),
            new TMP_Dropdown.OptionData("Random"),
            new TMP_Dropdown.OptionData("Invert"),
            new TMP_Dropdown.OptionData("GivenTarget")
        },
        OnSelectChangeTowardsType);

    }

    private void OnSelectChangeTowardsType(int index)
    {
        BTAgentChangeTowardsNode node = mNode as BTAgentChangeTowardsNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectChangeTowardsType Error, node is not BTAgentChangeTowardsNode, actual type:{0}", mNode.GetType());
            return;
        }

        int agentType = AgentDefine.AgentType_NotDefine;
        switch (index)
        {
            case 0:
                agentType = BTDefine.BT_ChangeTowardsTo_NotDefine;
                break;
            case 1:
                agentType = BTDefine.BT_ChangeTowardsTo_Random;
                break;
            case 2:
                agentType = BTDefine.BT_ChangeTowardsTo_Invert;
                break;
            case 3:
                agentType = BTDefine.BT_ChangeTowardsTo_GivenTarget;
                break;
            default:
                break;

        }

        node.SetChangeTowardsType(agentType);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTAgentChangeTowardsNode node = mNode as BTAgentChangeTowardsNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTAgentChangeTowardsNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        switch (node.GetChangeTowardsType())
        {
            case BTDefine.BT_ChangeTowardsTo_Random:
                index = 1;
                break;
            case BTDefine.BT_ChangeTowardsTo_Invert:
                index = 2;
                break;
            case BTDefine.BT_ChangeTowardsTo_GivenTarget:
                index = 3;
                break;
            default:
                break;

        }

        Dropdown_ChangeTowards.value = index;
    }
}
