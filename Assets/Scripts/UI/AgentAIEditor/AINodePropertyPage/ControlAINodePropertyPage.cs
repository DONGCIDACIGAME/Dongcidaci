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
    protected Button Button_Copy;
    protected Button Button_Cut;
    protected Button Button_PasteChild;
    protected Button Button_Delete;

    protected BTNode mNode;

    private AIEditorClipboard mClipboard;

    protected override void BindUINodes()
    {
        InputField_AINodeName = BindInputFieldNode("InputField_AINodeName", OnNameChanged);
        InputField_AINodeDesc = BindInputFieldNode("InputField_AINodeDesc", OnDescChanged);
        Text_NodeType = BindTextNode("Text_NodeType");
        Text_NodeDetailType = BindTextNode("Text_NodeDetailType");
        Button_MoveForward = BindButtonNode("Button_MoveForward", OnMoveForwardClick);
        Button_MoveBackward = BindButtonNode("Button_MoveBackward", OnMoveBackwardClick);
        Button_Copy = BindButtonNode("Button_Copy", OnCopyClick);
        Button_Cut = BindButtonNode("Button_Cut", OnCutClick);
        Button_PasteChild = BindButtonNode("Button_PasteChild", OnPasteChildClick);
        Button_Delete = BindButtonNode("Button_Delete", OnDeleteClick);

    }

    /// <summary>
    /// 复制节点
    /// </summary>
    private void OnCopyClick()
    {
        if (mClipboard == null)
            return;

        mClipboard.Copy(mNode);
    }

    /// <summary>
    /// 剪切节点
    /// </summary>
    private void OnCutClick()
    {
        if (mClipboard == null)
            return;

        mClipboard.Cut(mNode);
    }

    private bool RecuriveCheck(BTNode parent, BTNode child)
    {
        BTNode temp = child;
        while (temp != null)
        {
            if (temp.Equals(parent))
                return true;

            temp = temp.GetParentNode();
        }

        return false;
    }


    /// <summary>
    /// 粘贴子节点
    /// </summary>
    private void OnPasteChildClick()
    {
        if (mClipboard == null)
            return;

        if (mNode == null)
            return;

        BTNode node = mClipboard.GetNode();
        AIEditorCacheType cacheType = mClipboard.GetCacheType();
        if (node == null)
            return;

        if (node.Equals(mNode))
            return;

        if (cacheType == AIEditorCacheType.Cut)
        {
            // 如果产生了递归
            if (RecuriveCheck(node, mNode))
            {
                UIManager.Ins.OpenPanel<PanelMessageBox>("Prefabs/UI/Common/Panel_MessageBox",
                    new Dictionary<string, object>
                    {
                    {"title", "行为树保存失败"},
                    {"content", "子节点发生了递归，请重新选择!" }
                    });
                return;
            }

            BTNode parent = node.GetParentNode();
            // 删除原来的父节点对这个节点的引用
            if (parent is BTTree)
            {
                (parent as BTTree).UnpackChildNode();
            }
            else if (parent is BTCompositeNode)
            {
                (parent as BTCompositeNode).UnpackChildNode(node);
            }
            else if (parent is BTDecorNode)
            {
                (parent as BTDecorNode).UnpackChildNode();
            }

            // 将节点设置为当前节点的子节点
            if (mNode is BTTree)
            {
                (mNode as BTTree).SetChildNode(node);
            }
            else if (mNode is BTCompositeNode)
            {
                (mNode as BTCompositeNode).AddChildNode(node);
            }
            else if (parent is BTDecorNode)
            {
                (mNode as BTDecorNode).SetChildNode(node);
            }
        }
        else if(cacheType == AIEditorCacheType.Copy)
        {
            BTNode copy = node.Copy();
            // 将节点设置为当前节点的子节点
            if (mNode is BTTree)
            {
                (mNode as BTTree).SetChildNode(copy);
            }
            else if (mNode is BTCompositeNode)
            {
                (mNode as BTCompositeNode).AddChildNode(copy);
            }
            else if (mNode is BTDecorNode)
            {
                (mNode as BTDecorNode).SetChildNode(copy);
            }
        }

        GameEventSystem.Ins.Post("UpdateAILogicArea");
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
        GameEventSystem.Ins.Post("DeleteAINode", mNode);
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
        GameEventSystem.Ins.Post("SelectNode", mNode);
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
        GameEventSystem.Ins.Post("SelectNode", mNode);
    }

    private void OnNameChanged(string newName)
    {
        mNode.NodeName = newName;
        GameEventSystem.Ins.Post("UpdateNodeDisplayName", mNode);
    }

    private void OnDescChanged(string newDesc)
    {
        mNode.NodeDesc = newDesc;
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

        PanelAIEditor editorPanel = UIManager.Ins.GetPanel<PanelAIEditor>();
        if(editorPanel != null)
        {
            mClipboard = editorPanel.ClipBoard;
        }
    }
}
