using TMPro;
using GameEngine;
using System.Collections.Generic;

public class ControlDecorNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_Dropdown Dropdown_ChangeType;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        Dropdown_ChangeType = BindDropDownNode("Dropdown_ChangeType", new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData("Invert"),
            new TMP_Dropdown.OptionData("Once"),
            new TMP_Dropdown.OptionData("Repeat"),
            new TMP_Dropdown.OptionData("Reset"),
            new TMP_Dropdown.OptionData("UntileTrue"),
        }, OnChangeType);
    }

    private int ToDetailType(int index)
    {
        switch (index)
        {
            case 0:
                return BTDefine.BT_Node_Type_Decor_Invert;
            case 1:
                return BTDefine.BT_Node_Type_Decor_Once;
            case 2:
                return BTDefine.BT_Node_Type_Decor_Repeat;
            case 3:
                return BTDefine.BT_Node_Type_Decor_Reset;
            case 4:
                return BTDefine.BT_Node_Type_Decor_UntilTrue;
            default:
                return BTDefine.BT_Node_Type_Decor_Invert;
        }
    }

    private int ToIndex(int detailType)
    {
        switch (detailType)
        {
            case BTDefine.BT_Node_Type_Decor_Invert:
                return 0;
            case BTDefine.BT_Node_Type_Decor_Once:
                return 1;
            case BTDefine.BT_Node_Type_Decor_Repeat:
                return 2;
            case BTDefine.BT_Node_Type_Decor_Reset:
                return 3;
            case BTDefine.BT_Node_Type_Decor_UntilTrue:
                return 4;
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
        if (parent != null)
        {
            parent.UnpackChildNode(oldNode);
            parent.AddChildNode(newNode);
        }

        // 重新绑定子节点
        BTNode[] childs = oldNode.GetChildNodes().ToArray();
        for (int i = 0; i < childs.Length; i++)
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
