using TMPro;
using System.Collections.Generic;
using GameEngine;

public class ControlCompositeNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_ChangeType;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        Dropdown_ChangeType = BindDropDownNode("Dropdown_ChangeType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("Sequence"),
            new TMP_Dropdown.OptionData("Selector"),
            new TMP_Dropdown.OptionData("Parallel"),
            new TMP_Dropdown.OptionData("If-Else"),
            new TMP_Dropdown.OptionData("WithState-Sequence"),
            new TMP_Dropdown.OptionData("WithState-Selector")
        }, OnChangeType);
    }

    private int ToDetailType(int index)
    {
        switch(index)
        {
            case 0:
                return BTDefine.BT_Node_Type_Composite_Sequence;
            case 1:
                return BTDefine.BT_Node_Type_Composite_Selector;
            case 2:
                return BTDefine.BT_Node_Type_Composite_Parallel;
            case 3:
                return BTDefine.BT_Node_Type_Composite_IfElse;
            case 4:
                return BTDefine.BT_Node_Type_Composite_WithStateSequence;
            case 5:
                return BTDefine.BT_Node_Type_Composite_WithStateSelector;
            default:
                return BTDefine.BT_Node_Type_Composite_Sequence;
        }
    }

    private int ToIndex(int detailType)
    {
        switch (detailType)
        {
            case BTDefine.BT_Node_Type_Composite_Sequence:
                return 0;
            case BTDefine.BT_Node_Type_Composite_Selector:
                return 1;
            case BTDefine.BT_Node_Type_Composite_Parallel:
                return 2;
            case BTDefine.BT_Node_Type_Composite_IfElse:
                return 3;
            case BTDefine.BT_Node_Type_Composite_WithStateSequence:
                return 4;
            case BTDefine.BT_Node_Type_Composite_WithStateSelector:
                return 5;
            default:
                return 0;
        }
    }


    private void OnChangeType(int index)
    {
        int detailType = ToDetailType(index);
        if (detailType == mNode.GetNodeDetailType())
            return;

        BTCompositeNode oldNode = mNode as BTCompositeNode;
        BTCompositeNode newNode = BehaviourTreeHelper.CreateBTNode(BTDefine.BT_Node_Type_Composite, detailType) as BTCompositeNode;
        newNode.NodeName = oldNode.NodeName;
        newNode.NodeDesc = oldNode.NodeDesc;

        // 父节点重新绑定
        BTNode parent = mNode.GetParentNode();
        if(parent != null)
        {
            parent.UnpackChildNode(oldNode);
            parent.AddChildNode(newNode);
        }

        // 重新绑定子节点
        BTNode[] childs = oldNode.GetChildNodes().ToArray();
        for(int i = 0;i<childs.Length;i++)
        {
            BTNode child = childs[i];
            oldNode.UnpackChildNode(child);
            newNode.AddChildNode(child);
        }

        mNode = newNode;
        GameEventSystem.Ins.Post("UpdateNode", oldNode, newNode);


        oldNode.Dispose();
        GameEventSystem.Ins.Post("SelectNode", mNode);
    }

    protected override void Initialize()
    {
        base.Initialize();

        Dropdown_ChangeType.value = ToIndex(mNode.GetNodeDetailType());
    }
}
