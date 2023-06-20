using GameEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public abstract class ControlAINodePropertyPage : UIControl
{
    protected TMP_InputField InputField_AINodeName;
    protected TMP_InputField InputField_AINodeDesc;
    protected TMP_Text Text_NodeType;
    protected TMP_Text Text_NodeDetailType;
    protected Button Button_MoveForward;
    protected Button Button_MoveBackward;
    protected Button Button_Delete;

    protected BTNode mNode;

    protected override void BindUINodes()
    {
        InputField_AINodeName = BindInputFieldNode("InputField_AINodeName", OnNameChanged);
        InputField_AINodeDesc = BindInputFieldNode("InputField_AINodeDesc", OnDescChanged);
        Text_NodeType = BindTextNode("Text_NodeType");
        Text_NodeDetailType = BindTextNode("Text_NodeDetailType");
        Button_MoveForward = BindButtonNode("Button_MoveForward", OnMoveForwardClick);
        Button_MoveBackward = BindButtonNode("Button_MoveBackward", OnMoveBackwardClick);
        Button_Delete = BindButtonNode("Button_Delete", OnDeleteClick);
    }

    private void OnDeleteClick()
    {
        BTNode parent = mNode.GetParentNode();
        if (parent is BTTree)
        {
            BTTree tree = parent as BTTree;
            tree.RemoveChildNode();
        }
        else if(parent is BTDecorNode)
        {
            BTDecorNode decor = parent as BTDecorNode;
            decor.RemoveChildNode();
        }
        else if(parent is BTCompositeNode)
        {
            BTCompositeNode composite = parent as BTCompositeNode;
            composite.RemoveChildNode(mNode);
        }

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    private void OnMoveForwardClick()
    {
        BTNode parent = mNode.GetParentNode();
        if (parent != null && parent is BTCompositeNode)
        {
            BTCompositeNode composite = parent as BTCompositeNode;
            composite.MoveNodeForward(mNode);
        }

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    private void OnMoveBackwardClick()
    {
        BTNode parent = mNode.GetParentNode();
        if (parent != null && parent is BTCompositeNode)
        {
            BTCompositeNode composite = parent as BTCompositeNode;
            composite.MoveNodeBackward(mNode);
        }

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    private void OnNameChanged(string newName)
    {
        mNode.NodeName = newName;
        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    private void OnDescChanged(string newDesc)
    {
        mNode.NodeDesc = newDesc;
        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }


    protected override void OnClose()
    {
        
    }

    protected virtual void Initialize()
    {
        if (mNode == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Property page Error, BTNode is null!", GetType());
            return;
        }

        InputField_AINodeName.text = mNode.NodeName;
        InputField_AINodeDesc.text = mNode.NodeDesc;
        Text_NodeType.text = BehaviourTreeHelper.GetNodeTypeName(mNode.GetNodeType());
        Text_NodeDetailType.text = BehaviourTreeHelper.GetNodeDetailTypeName(mNode.GetNodeDetailType());
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        mNode = openArgs["node"] as BTNode;

        Initialize();
    }
}
