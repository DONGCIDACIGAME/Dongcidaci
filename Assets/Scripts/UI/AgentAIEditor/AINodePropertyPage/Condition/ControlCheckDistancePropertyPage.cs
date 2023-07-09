using System.Collections.Generic;
using TMPro;

public abstract class ControlCheckDistancePropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_OperatorType;
    private TMP_InputField InputField_Distance;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        List<TMP_Dropdown.OptionData> operatorOps = new List<TMP_Dropdown.OptionData>();
        for(int i = 0;i< BTDefine.BT_ALL_OPERATORS.Length;i++)
        {
            operatorOps.Add(new TMP_Dropdown.OptionData(BTDefine.BT_ALL_OPERATORS[i].Desc));
        }
        Dropdown_OperatorType = BindDropDownNode("Dropdown_OperatorType", operatorOps, OnSelectOperatorType);

        InputField_Distance = BindInputFieldNode("InputField_Distance", OnEditDistance);

    }

    private void OnEditDistance(string value)
    {
        float distance = float.Parse(value);
        BTCheckDistanceNode node = mNode as BTCheckDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditDistance Error, node is not BTCheckDistanceToTarget, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetDistance(distance);
    }

    private void OnSelectOperatorType(int index)
    {
        BTCheckDistanceNode node = mNode as BTCheckDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectDetectAgentType Error, node is not BTCheckDistanceToTarget, actual type:{0}", mNode.GetType());
            return;
        }

        int op = BTDefine.BT_Operator_Undefine;
        if(index < BTDefine.BT_ALL_OPERATORS.Length)
        {
            op = BTDefine.BT_ALL_OPERATORS[index].Type;
        }

        node.SetOperator(op);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTCheckDistanceNode node = mNode as BTCheckDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckDetectAgentNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        int operatorType = node.GetOperator();
        for(int i = 0;i<BTDefine.BT_ALL_OPERATORS.Length;i++)
        {
            if(operatorType == BTDefine.BT_ALL_OPERATORS[i].Type)
            {
                index = i;
                break;
            }
        }

        Dropdown_OperatorType.value = index;
        InputField_Distance.text = node.GetDistance().ToString();
    }
}
