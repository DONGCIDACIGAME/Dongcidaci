using GameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private BTNode mNode;

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<string>("OnLoadChildTree", AddChildTree);
    }

    private void AddChildTree(string childTreeFilePath)
    {
        BTNodeData childTreeData = BehaviourTreeManager.Ins.LoadTreeData(childTreeFilePath, true);
        BTChildTree tree = BehaviourTreeManager.Ins.CreateBTNode(childTreeData) as BTChildTree;
        tree.LoadFromBTNodeData(childTreeData);
        AddChildNode(tree);
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
                        { "loadEvent", "OnLoadChildTree"}
                });
        });
        Btn_Sequence = BindButtonNode("Contain_CompositeNodes/Button_Sequence", ()=> { AddChildNode(new BTSequenceNode()); });
        Btn_Selector = BindButtonNode("Contain_CompositeNodes/Button_Selector", () => { AddChildNode(new BTSelectNode()); });
        Btn_Parallel = BindButtonNode("Contain_CompositeNodes/Button_Parallel", () => { AddChildNode(new BTParallelNode()); });
        Btn_Invert = BindButtonNode("Contain_DecorNodes/Button_Invert", () => { AddChildNode(new BTInvertNode()); });
        Btn_Repeat = BindButtonNode("Contain_DecorNodes/Button_Repeat", () => { AddChildNode(new BTRepeatNode()); });
        Btn_WaitTime = BindButtonNode("Contain_ActionNodes/Button_WaitTime", () => { AddChildNode(new BTWaitTimeNode()); });
        Btn_WaitFrame = BindButtonNode("Contain_ActionNodes/Button_WaitFrame", () => { AddChildNode(new BTWaitFrameNode()); });
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
