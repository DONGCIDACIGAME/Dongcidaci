using GameEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAddNewNodePage : UIControl
{
    private Button Btn_Sequence;
    private Button Btn_Selector;
    private Button Btn_Parallel;
    private Button Btn_Invert;
    private Button Btn_Repeat;
    private Button Btn_WaitTime;
    private Button Btn_WaitFrame;

    private BTNode mCurSelectNode;

    protected override void BindUINodes()
    {
        Btn_Sequence = BindButtonNode("Contain_CompositeNodes/Button_Sequence", ()=> { AddChildNode(new BTSequenceNode()); });
        Btn_Selector = BindButtonNode("Contain_CompositeNodes/Button_Selector", () => { AddChildNode(new BTSelectNode()); });
        Btn_Parallel = BindButtonNode("Contain_CompositeNodes/Button_Parallel", () => { AddChildNode(new BTParallelNode()); });
        Btn_Invert = BindButtonNode("Contain_DecorNodes/Button_Invert", () => { AddChildNode(new BTInvertNode()); });
        Btn_Repeat = BindButtonNode("Contain_DecorNodes/Button_Repeat", () => { AddChildNode(new BTRepeatNode()); });
        Btn_WaitTime = BindButtonNode("Contain_ActionNodes/Button_WaitTime", () => { AddChildNode(new BTWaitTimeNode()); });
        Btn_WaitFrame = BindButtonNode("Contain_ActionNodes/Button_WaitFrame", () => { AddChildNode(new BTWaitFrameNode()); });
    }

    public void SetCurSelectNode(BTNode node)
    {
        mCurSelectNode = node;
    }

    private void AddChildNode(BTNode childNode)
    {
        if (mCurSelectNode == null)
            return;

        if(mCurSelectNode is BTCompositeNode)
        {
            BTCompositeNode composite = mCurSelectNode as BTCompositeNode;
            composite.AddChildNode(childNode);
            GameEventSystem.Ins.Fire("UpdateAILogicArea");
        }
        else if(mCurSelectNode is BTDecorNode)
        {
            BTDecorNode decor = mCurSelectNode as BTDecorNode;
            if(decor.GetChildNode() == null)
            {
                decor.SetChildNode(childNode);
                GameEventSystem.Ins.Fire("UpdateAILogicArea");
            }
        }
        else if (mCurSelectNode is BTTree)
        {
            BTTree tree = mCurSelectNode as BTTree;
            if (tree.GetChildNode() == null)
            {
                tree.SetChildNode(childNode);
                GameEventSystem.Ins.Fire("UpdateAILogicArea");
            }
        }
        else if(mCurSelectNode is BTLeafNode)
        {

        }
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
