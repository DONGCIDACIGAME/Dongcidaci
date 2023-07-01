using TMPro;
using GameEngine;
using System.Collections.Generic;

public class ControlDetectAgentPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_AgentType;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Dropdown_AgentType = BindDropDownNode("Dropdown_AgentType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("Undefined"),
            new TMP_Dropdown.OptionData("Hero"),
            new TMP_Dropdown.OptionData("Monster"),
            new TMP_Dropdown.OptionData("NPC")
        },
        OnSelectDetectAgentType);

    }

    private void OnSelectDetectAgentType(int index)
    {
        BTDetectAgentNode node = mNode as BTDetectAgentNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectDetectAgentType Error, node is not BTCheckDetectAgentNode, actual type:{0}", mNode.GetType());
            return;
        }

        int agentType = AgentDefine.AgentType_NotDefine;
        switch(index)
        {
            case 0:
                agentType = AgentDefine.AgentType_NotDefine;
                break;
            case 1:
                agentType = AgentDefine.AgentType_Hero;
                break;
            case 2:
                agentType = AgentDefine.AgentType_Monster;
                break;
            case 3:
                agentType = AgentDefine.AgentType_NPC;
                break;
            default:
                break;

        }

        node.SetTargetAgentType(agentType);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTDetectAgentNode node = mNode as BTDetectAgentNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckDetectAgentNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        switch (node.GetTargetAgentType())
        {
            case AgentDefine.AgentType_Hero:
                index = 1;
                break;
            case AgentDefine.AgentType_Monster:
                index = 2;
                break;
            case AgentDefine.AgentType_NPC:
                index = 3;
                break;
            default:
                break;

        }

        Dropdown_AgentType.value = index;
    }
}
