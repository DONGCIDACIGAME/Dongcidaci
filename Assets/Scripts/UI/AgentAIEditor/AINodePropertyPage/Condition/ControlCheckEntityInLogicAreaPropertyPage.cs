using System.Collections.Generic;
using TMPro;

public class ControlCheckEntityInLogicAreaPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_OperatorType;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Dropdown_OperatorType = BindDropDownNode("Dropdown_LogicAreaType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("Undefined"),
            new TMP_Dropdown.OptionData("Attack"),
            new TMP_Dropdown.OptionData("Interact")
        },
        OnSelectLogicAreaType);

    }

    private void OnSelectLogicAreaType(int index)
    {
        BTCheckTargetEntityInLogicAreaNode node = mNode as BTCheckTargetEntityInLogicAreaNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectLogicAreaType Error, node is not BTCheckTargetEntityInLogicAreaNode, actual type:{0}", mNode.GetType());
            return;
        }

        int logicAreaType = BTDefine.BT_LogicArea_Type_Undefine;
        switch (index)
        {
            case 0:
                logicAreaType = BTDefine.BT_LogicArea_Type_Undefine;
                break;
            case 1:
                logicAreaType = BTDefine.BT_LogicArea_Type_Attack;
                break;
            case 2:
                logicAreaType = BTDefine.BT_LogicArea_Type_Interact;
                break;
            default:
                break;

        }

        node.SetLogicAreaType(logicAreaType);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTCheckTargetEntityInLogicAreaNode node = mNode as BTCheckTargetEntityInLogicAreaNode;

        if (node == null)
    {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckTargetEntityInLogicAreaNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        switch (node.GetLogicAreaType())
        {
            case BTDefine.BT_LogicArea_Type_Attack:
                index = 1;
                break;
            case BTDefine.BT_LogicArea_Type_Interact:
                index = 2;
                break;
            default:
                break;

        }

        Dropdown_OperatorType.value = index;
    }
}
