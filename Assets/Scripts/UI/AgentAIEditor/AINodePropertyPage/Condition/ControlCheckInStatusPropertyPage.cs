using System.Collections.Generic;
using TMPro;

public class ControlCheckInStatusPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_StatusType;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Dropdown_StatusType = BindDropDownNode("Dropdown_StatusType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(AgentStatusDefine.EMPTY),
            new TMP_Dropdown.OptionData(AgentStatusDefine.IDLE),
            new TMP_Dropdown.OptionData(AgentStatusDefine.RUN),
            new TMP_Dropdown.OptionData(AgentStatusDefine.DASH),
            new TMP_Dropdown.OptionData(AgentStatusDefine.TRANSITION),
            new TMP_Dropdown.OptionData(AgentStatusDefine.ATTACK),
            new TMP_Dropdown.OptionData(AgentStatusDefine.BEHIT),
            new TMP_Dropdown.OptionData(AgentStatusDefine.DEAD),
        }, OnSelectTargetStatus) ;



    }

    private void OnSelectTargetStatus(int index)
    {
        BTCheckInStatusNode node = mNode as BTCheckInStatusNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectTargetStatus Error, node is not BTCheckInStatusNode, actual type:{0}", mNode.GetType());
            return;
        }

        string targetStatus = AgentStatusDefine.EMPTY;
        switch(index)
        {
            case 0:
                targetStatus = AgentStatusDefine.EMPTY;
                break;
            case 1:
                targetStatus = AgentStatusDefine.IDLE;
                break;
            case 2:
                targetStatus = AgentStatusDefine.RUN;
                break;
            case 3:
                targetStatus = AgentStatusDefine.DASH;
                break;
            case 4:
                targetStatus = AgentStatusDefine.TRANSITION;
                break;
            case 5:
                targetStatus = AgentStatusDefine.ATTACK;
                break;
            case 6:
                targetStatus = AgentStatusDefine.BEHIT;
                break;
            case 7:
                targetStatus = AgentStatusDefine.DEAD;
                break;
            default:
                break;
        }

        node.SetTargetStatus(targetStatus);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTCheckInStatusNode node = mNode as BTCheckInStatusNode;

        if (node == null)
    {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckInStatusNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        string targetStatus = node.GetTargetStatus();
        if(AgentStatusDefine.EMPTY.Equals(targetStatus))
        {
            index = 0;
        }
        else if(AgentStatusDefine.IDLE.Equals(targetStatus))
        {
            index = 1;
        }
        else if (AgentStatusDefine.RUN.Equals(targetStatus))
        {
            index = 2;
        }
        else if (AgentStatusDefine.DASH.Equals(targetStatus))
        {
            index = 3;
        }
        else if (AgentStatusDefine.TRANSITION.Equals(targetStatus))
        {
            index = 4;
        }
        else if (AgentStatusDefine.ATTACK.Equals(targetStatus))
        {
            index = 5;
        }
        else if (AgentStatusDefine.BEHIT.Equals(targetStatus))
        {
            index = 6;
        }
        else if (AgentStatusDefine.DEAD.Equals(targetStatus))
        {
            index = 7;
        }


        Dropdown_StatusType.value = index;
    }
}
