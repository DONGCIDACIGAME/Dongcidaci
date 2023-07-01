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
        }, OnChangeType);
    }

    private int ToDetailType(int index)
    {
        switch(index)
        {
            case 0:
                return BTDefine.BT_Node_DetailType_Composite_Sequence;
            case 1:
                return BTDefine.BT_Node_DetailType_Composite_Selector;
            case 2:
                return BTDefine.BT_Node_DetailType_Composite_Parallel;
            default:
                return BTDefine.BT_Node_DetailType_Composite_Sequence;
        }
    }

    private int ToIndex(int detailType)
    {
        switch (detailType)
        {
            case BTDefine.BT_Node_DetailType_Composite_Sequence:
                return 0;
            case BTDefine.BT_Node_DetailType_Composite_Selector:
                return 1;
            case BTDefine.BT_Node_DetailType_Composite_Parallel:
                return 2;
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
        BTCompositeNode newNode = BehaviourTreeManager.Ins.CreateBTNode(BTDefine.BT_Node_Type_Composite, detailType) as BTCompositeNode;
        newNode.NodeName = oldNode.NodeName;
        newNode.NodeDesc = oldNode.NodeDesc;

        BTNode parent = mNode.GetParentNode();
        if (parent is BTTree)
        {
            BTTree tree = parent as BTTree;
            tree.UnpackChildNode();
            tree.SetChildNode(newNode);
        }
        else if (parent is BTCompositeNode)
        {
            BTCompositeNode composite = parent as BTCompositeNode;
            composite.UnpackChildNode(oldNode);
            composite.AddChildNode(newNode);
        }
        else if (parent is BTDecorNode)
        {
            BTDecorNode decor = parent as BTDecorNode;
            decor.UnpackChildNode();
            decor.SetChildNode(newNode);
        }


        BTNode[] childs = oldNode.GetChildNodes().ToArray();
        for(int i =0;i<childs.Length;i++)
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
