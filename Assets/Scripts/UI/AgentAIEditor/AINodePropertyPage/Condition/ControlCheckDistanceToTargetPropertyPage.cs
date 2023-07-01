using System.Collections.Generic;
using TMPro;

public class ControlCheckDistanceToTargetPropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_OperatorType;
    private TMP_InputField InputField_Distance;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Dropdown_OperatorType = BindDropDownNode("Dropdown_OperatorType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("undefine"),
            new TMP_Dropdown.OptionData("<"),
            new TMP_Dropdown.OptionData("<="),
            new TMP_Dropdown.OptionData("="),
            new TMP_Dropdown.OptionData(">"),
            new TMP_Dropdown.OptionData(">=")
        },
        OnSelectOperatorType);

        InputField_Distance = BindInputFieldNode("InputField_Distance", OnEditDistance);

    }

    private void OnEditDistance(string value)
    {
        float distance = float.Parse(value);
        BTCheckDistanceToTarget node = mNode as BTCheckDistanceToTarget;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditDistance Error, node is not BTCheckDistanceToTarget, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetDistance(distance);
    }

    private void OnSelectOperatorType(int index)
    {
        BTCheckDistanceToTarget node = mNode as BTCheckDistanceToTarget;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnSelectDetectAgentType Error, node is not BTCheckDistanceToTarget, actual type:{0}", mNode.GetType());
            return;
        }

        int op = BTDefine.BT_Operator_Undefine;
        switch (index)
        {
            case 0:
                op = BTDefine.BT_Operator_Undefine;
                break;
            case 1:
                op = BTDefine.BT_Operator_LessThan;
                break;
            case 2:
                op = BTDefine.BT_Operator_LessEqual;
                break;
            case 3:
                op = BTDefine.BT_Operator_Equal;
                break;
            case 4:
                op = BTDefine.BT_Operator_GreaterThan;
                break;
            case 5:
                op = BTDefine.BT_Operator_GreaterEqual;
                break;
            case 6:
                op = BTDefine.BT_Operator_LessThan;
                break;
            default:
                break;

        }

        node.SetOperator(op);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTCheckDistanceToTarget node = mNode as BTCheckDistanceToTarget;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTCheckDetectAgentNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        int index = 0;
        switch (node.GetOperator())
        {
            case BTDefine.BT_Operator_LessThan:
                index = 1;
                break;
            case BTDefine.BT_Operator_LessEqual:
                index = 2;
                break;
            case BTDefine.BT_Operator_Equal:
                index = 3;
                break;
            case BTDefine.BT_Operator_GreaterThan:
                index = 4;
                break;
            case BTDefine.BT_Operator_GreaterEqual:
                index = 5;
                break;
            default:
                break;

        }

        Dropdown_OperatorType.value = index;

        InputField_Distance.text = node.GetDistance().ToString();
    }
}
