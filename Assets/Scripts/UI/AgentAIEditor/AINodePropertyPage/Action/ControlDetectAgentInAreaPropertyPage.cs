using TMPro;
using System.Collections.Generic;

public class ControlDetectAgentInAreaPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_AgentType;
    private TMP_Dropdown Dropdown_AreaType;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        List<TMP_Dropdown.OptionData> agentTypeOps = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < AgentDefine.ALL_AGENT_TYPES.Length; i++)
        {
            agentTypeOps.Add(new TMP_Dropdown.OptionData(AgentDefine.ALL_AGENT_TYPES[i].Desc));
        }

        Dropdown_AgentType = BindDropDownNode("Dropdown_AgentType", agentTypeOps, OnSelectDetectAgentType);


        List<TMP_Dropdown.OptionData> areaTypeOps = new List<TMP_Dropdown.OptionData>();
        for(int i = 0;i< BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length;i++)
        {
            areaTypeOps.Add(new TMP_Dropdown.OptionData(BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Desc));
        }

        Dropdown_AreaType = BindDropDownNode("Dropdown_AreaType", areaTypeOps, OnSelectDetectAreaType);
    }

    private void OnSelectDetectAreaType(int index)
    {
        BTDetectAgentInAreaNode node = mNode as BTDetectAgentInAreaNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectDetectAreaType Error, node is not BTCheckDetectAgentNode, actual type:{0}", mNode.GetType());
            return;
        }

        int areaType = BTDefine.BT_LogicArea_Type_Undefine;
        if(index < BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length)
        {
            areaType = BTDefine.BT_ALL_LOGIC_AREA_TYPES[index].Type;
        }

        node.SetTargetAgentType(areaType);
    }


    private void OnSelectDetectAgentType(int index)
    {
        BTDetectAgentInAreaNode node = mNode as BTDetectAgentInAreaNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectDetectAgentType Error, node is not BTCheckDetectAgentNode, actual type:{0}", mNode.GetType());
            return;
        }

        int agentType = AgentDefine.AgentType_NotDefine;
        if(index < AgentDefine.ALL_AGENT_TYPES.Length)
        {
            agentType = AgentDefine.ALL_AGENT_TYPES[index].Type;
        }

        node.SetTargetAgentType(agentType);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTDetectAgentInAreaNode node = mNode as BTDetectAgentInAreaNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckDetectAgentNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index1 = 0;
        int targetAgentType = node.GetTargetAgentType();
        for(int i = 0;i<AgentDefine.ALL_AGENT_TYPES.Length;i++)
        {
            if(AgentDefine.ALL_AGENT_TYPES[i].Type == targetAgentType)
            {
                index1 = i;
                break;
            }
        }

        int index2 = 0;
        int targetAreaType = node.GetTargetAreaType();
        for (int i = 0; i < BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length; i++)
        {
            if (BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Type == targetAreaType)
            {
                index2 = i;
                break;
            }
        }

        Dropdown_AgentType.value = index1;
        Dropdown_AreaType.value = index2;
    }
}
