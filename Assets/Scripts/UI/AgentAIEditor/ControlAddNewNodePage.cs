using GameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 添加子节点的操作界面
/// ---------------------------增加新节点时需要更新
/// </summary>
public class ControlAddNewNodePage : UIControl
{
    private Button Btn_ChildTree;
    private Button Btn_Sequence;
    private Button Btn_Selector;
    private Button Btn_Parallel;
    private Button Btn_Invert;
    private Button Btn_Repeat;
    private Button Btn_WaitTime;
    private Button Btn_WaitFrame;
    private Button Btn_WaitMeter;

    private BTNode mNode;

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<string>("OnLoadChildTreeFromFile", AddChildTreeFromFile);
    }

    private void AddChildTreeFromFile(string childTreeFilePath)
    {
        BTChildTree childTree = BehaviourTreeManager.Ins.CreateBTNode(BTDefine.BT_Node_Type_Tree, BTDefine.BT_Node_Type_Tree_ChildTree) as BTChildTree;
        string childTreeName = BehaviourTreeHelper.FileFullPathToTreeName(childTreeFilePath);
        childTree.SetChildTreeName(childTreeName);
        // 从文件加载的子树默认是浅拷贝
        childTree.SetCopyType(BTDefine.BT_ChildTreeCopyType_Reference);
        BTTreeEntry treeEntry = BehaviourTreeManager.Ins.LoadTree(childTreeFilePath, true);
        childTree.SetChildNode(treeEntry);
        AddChildNode(childTree);
    }

    protected override void BindUINodes()
    {
        Btn_ChildTree = BindButtonNode("Contain_TreeNodes/Button_ChildTree", () => {
            UIManager.Ins.OpenPanel<PanelLoadFileHUD>(
                "Prefabs/UI/Common/Panel_LoadFileHUD",
                new Dictionary<string, object>
                {
                        { "root_dir", PathDefine.AI_TREE_DATA_DIR_PATH },
                        { "ext", ".tree"},
                        { "loadEvent", "OnLoadChildTreeFromFile"}
                });
        });
        Btn_Sequence = BindButtonNode("Contain_CompositeNodes/Button_Sequence", ()=> { AddChildNode(new BTSequenceNode()); });
        Btn_Selector = BindButtonNode("Contain_CompositeNodes/Button_Selector", () => { AddChildNode(new BTSelectNode()); });
        Btn_Parallel = BindButtonNode("Contain_CompositeNodes/Button_Parallel", () => { AddChildNode(new BTParallelNode()); });
        Btn_Invert = BindButtonNode("Contain_DecorNodes/Button_Invert", () => { AddChildNode(new BTInvertNode()); });
        Btn_Repeat = BindButtonNode("Contain_DecorNodes/Button_Repeat", () => { AddChildNode(new BTRepeatNode()); });
        Btn_WaitTime = BindButtonNode("Contain_ActionNodes/Button_WaitTime", () => { AddChildNode(new BTWaitTimeNode()); });
        Btn_WaitFrame = BindButtonNode("Contain_ActionNodes/Button_WaitFrame", () => { AddChildNode(new BTWaitFrameNode()); });
        Btn_WaitMeter = BindButtonNode("Contain_ActionNodes/Button_WaitMeter", () => { AddChildNode(new BTWaitMeterNode()); });
    }

    public void SetBTNode(BTNode node)
    {
        mNode = node;
    }

    private void AddChildNode(BTNode childNode)
    {
        if (mNode == null)
            return;

        if(mNode is BTCompositeNode)
        {
            BTCompositeNode composite = mNode as BTCompositeNode;
            composite.AddChildNode(childNode);
            GameEventSystem.Ins.Fire("UpdateAILogicArea");
        }
        else if(mNode is BTDecorNode)
        {
            BTDecorNode decor = mNode as BTDecorNode;
            if(decor.GetChildNode() == null)
            {
                decor.SetChildNode(childNode);
                GameEventSystem.Ins.Fire("UpdateAILogicArea");
            }
        }
        else if (mNode is BTTree)
        {
            BTTree tree = mNode as BTTree;
            if (tree.GetChildNode() == null)
            {
                tree.SetChildNode(childNode);
                GameEventSystem.Ins.Fire("UpdateAILogicArea");
            }
        }
        else if(mNode is BTLeafNode)
        {

        }

        GameEventSystem.Ins.Fire("SelectNode", childNode);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        BTNode node = openArgs["node"] as BTNode;
        SetBTNode(node);
    }
}
