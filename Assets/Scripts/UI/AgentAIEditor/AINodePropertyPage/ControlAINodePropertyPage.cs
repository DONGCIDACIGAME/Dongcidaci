using GameEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public abstract class ControlAINodePropertyPage : UIControl
{
    private TMP_InputField InputField_AINodeName;
    private TMP_InputField InputField_AINodeDesc;
    private Button Button_MoveForward;
    private Button Button_MoveBackward;

    protected BTNode mNode;

    protected override void BindUINodes()
    {
        InputField_AINodeName = BindInputFieldNode("InputField_AINodeName", OnNameChanged);
        InputField_AINodeDesc = BindInputFieldNode("InputField_AINodeDesc", OnDescChanged);
        Button_MoveForward = BindButtonNode("Button_MoveForward", OnMoveForwardClick);
        Button_MoveBackward = BindButtonNode("Button_MoveBackward", OnMoveBackwardClick);
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
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        mNode = openArgs["node"] as BTNode;

        Initialize();
    }
}
