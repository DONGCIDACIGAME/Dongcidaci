using TMPro;
using System.Collections.Generic;
using GameEngine;

public class ControlChangeTowardsPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_ChangeTowards;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        List<TMP_Dropdown.OptionData> changeTowardsOps = new List<TMP_Dropdown.OptionData>();
        for(int i = 0; i<BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES.Length;i++)
        {
            changeTowardsOps.Add(new TMP_Dropdown.OptionData(BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES[i].Desc));
        }

        Dropdown_ChangeTowards = BindDropDownNode("Dropdown_ChangeTowards", changeTowardsOps,OnSelectChangeTowardsType);

    }

    private void OnSelectChangeTowardsType(int index)
    {
        BTAgentChangeTowardsNode node = mNode as BTAgentChangeTowardsNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectChangeTowardsType Error, node is not BTAgentChangeTowardsNode, actual type:{0}", mNode.GetType());
            return;
        }

        int changeTowardsType = BTDefine.BT_ChangeTowardsTo_NotDefine;
        if(index < BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES.Length)
        {
            changeTowardsType = BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES[index].Type;
        }

        node.SetChangeTowardsType(changeTowardsType);
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
        int changeTowardsType = node.GetChangeTowardsType();
        for (int i = 0; i < BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES.Length; i++)
        {
            if (changeTowardsType == BTDefine.BT_ALL_CHANGE_TOWARDS_TYPES[i].Type)
            {
                index = i;
                break;
            }
        }

        Dropdown_ChangeTowards.value = index;
    }
}
