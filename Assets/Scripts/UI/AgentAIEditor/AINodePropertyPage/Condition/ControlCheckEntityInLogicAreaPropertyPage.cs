using System.Collections.Generic;
using TMPro;

public class ControlCheckEntityInLogicAreaPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_OperatorType;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        List<TMP_Dropdown.OptionData> areaOps = new List<TMP_Dropdown.OptionData>();
        for(int i = 0;i<BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length;i++)
        {
            areaOps.Add(new TMP_Dropdown.OptionData(BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Desc));
        }

        Dropdown_OperatorType = BindDropDownNode("Dropdown_LogicAreaType", areaOps, OnSelectLogicAreaType);

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
        if(index < BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length)
        {
            logicAreaType = BTDefine.BT_ALL_LOGIC_AREA_TYPES[index].Type;
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
        int logicAreaType = node.GetLogicAreaType();
        for(int i =0;i<BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length;i++)
        {
            if(BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Type == logicAreaType)
            {
                index = i;
                break;
            }
        }

        Dropdown_OperatorType.value = index;
    }
}
