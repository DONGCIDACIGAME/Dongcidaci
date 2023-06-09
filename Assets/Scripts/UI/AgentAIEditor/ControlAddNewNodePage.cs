using GameEngine;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// 添加子节点的操作界面
/// ---------------------------增加新节点时需要更新
/// </summary>
public class ControlAddNewNodePage : UIControl
{
    /// <summary>
    ///  tree
    /// </summary>
    private Button Btn_ChildTree;

    /// <summary>
    /// compite
    /// </summary>
    private Button Btn_Sequence;
    private Button Btn_Selector;
    private Button Btn_Parallel;
    private Button Btn_IfElse;
    private Button Btn_WithStateSequence;
    private Button Btn_WithStateSelector;

    /// <summary>
    /// decor
    /// </summary>
    private Button Btn_Invert;
    private Button Btn_Repeat;
    private Button Btn_Once;
    private Button Btn_Reset;
    private Button Btn_UntilTrue;


    /// <summary>
    /// action
    /// </summary>
    private Button Btn_WaitTime;
    private Button Btn_WaitFrame;
    private Button Btn_WaitMeter;
    private Button Btn_ChangeTowards;
    private Button Btn_MoveTime;
    private Button Btn_MoveMeter;
    private Button Btn_MoveToPosition;
    private Button Btn_MoveDistance;
    private Button Btn_DetectAgentInArea;
    private Button Btn_MoveOneFrame;
    private Button Btn_ClearTarget;
    private Button Btn_Idle;
    private Button Btn_Attack;


    /// <summary>
    /// condition
    /// </summary>
    private Button Btn_CheckDistanceToTargetEntity;
    private Button Btn_CheckDistanceToTargetPosition;
    private Button Btn_CheckTargetEntityInLogicArea;
    private Button Btn_CheckInStatus;
    private Button Btn_CheckHasTarget;


    private BTNode mNode;

    protected override void BindEvents()
    {
        base.BindEvents();

        mEventListener.Listen<string>("OnLoadChildTreeFromFile", AddChildTreeFromFile);
    }

    private void AddChildTreeFromFile(string childTreeFilePath)
    {
        BTChildTree childTree = BehaviourTreeHelper.CreateBTNode(BTDefine.BT_Node_Type_Tree, BTDefine.BT_Node_Type_Tree_ChildTree) as BTChildTree;
        string childTreeName = BehaviourTreeHelper.FileFullPathToTreeName(childTreeFilePath);
        childTree.SetChildTreeName(childTreeName);
        // 从文件加载的子树默认是浅拷贝
        childTree.SetCopyType(BTDefine.BT_ChildTreeCopyType_Reference);
        BTTreeEntry treeEntry = DataCenter.Ins.BehaviourTreeCenter.LoadTreeWithFileFullPath(childTreeFilePath, true);
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


        Btn_Sequence = BindButtonNode("Contain_CompositeNodes/Button_Sequence", () => { AddChildNode(new BTSequenceNode()); });
        Btn_Selector = BindButtonNode("Contain_CompositeNodes/Button_Selector", () => { AddChildNode(new BTSelectorNode()); });
        Btn_Parallel = BindButtonNode("Contain_CompositeNodes/Button_Parallel", () => { AddChildNode(new BTParallelNode()); });
        Btn_IfElse = BindButtonNode("Contain_CompositeNodes/Button_IfElse", () => { AddChildNode(new BTIfElseNode()); });
        Btn_WithStateSequence = BindButtonNode("Contain_CompositeNodes/Button_WithStateSequence", () => { AddChildNode(new BTWithStateSequenceNode()); });
        Btn_WithStateSelector = BindButtonNode("Contain_CompositeNodes/Button_WithStateSelector", () => { AddChildNode(new BTWithStateSelectorNode()); });


        Btn_Invert = BindButtonNode("Contain_DecorNodes/Button_Invert", () => { AddChildNode(new BTInvertNode()); });
        Btn_Repeat = BindButtonNode("Contain_DecorNodes/Button_Repeat", () => { AddChildNode(new BTRepeatNode()); });
        Btn_Once = BindButtonNode("Contain_DecorNodes/Button_Once", () => { AddChildNode(new BTOnceNode()); });
        Btn_Reset = BindButtonNode("Contain_DecorNodes/Button_Reset", () => { AddChildNode(new BTResetNode()); });
        Btn_UntilTrue = BindButtonNode("Contain_DecorNodes/Button_UntilTrue", () => { AddChildNode(new BTUntilTrueNode()); });


        Btn_WaitTime = BindButtonNode("Contain_ActionNodes/Button_WaitTime", () => { AddChildNode(new BTWaitTimeNode()); });
        Btn_WaitFrame = BindButtonNode("Contain_ActionNodes/Button_WaitFrame", () => { AddChildNode(new BTWaitFrameNode()); });
        Btn_WaitMeter = BindButtonNode("Contain_ActionNodes/Button_WaitMeter", () => { AddChildNode(new BTWaitMeterNode()); });
        Btn_ChangeTowards = BindButtonNode("Contain_ActionNodes/Button_ChangeTowards", () => { AddChildNode(new BTAgentChangeTowardsNode()); });
        Btn_MoveTime = BindButtonNode("Contain_ActionNodes/Button_MoveTime", () => { AddChildNode(new BTMoveTimeNode()); });
        Btn_MoveMeter = BindButtonNode("Contain_ActionNodes/Button_MoveMeter", () => { AddChildNode(new BTMoveMeterNode()); });
        Btn_MoveToPosition = BindButtonNode("Contain_ActionNodes/Button_MoveToPosition", () => { AddChildNode(new BTMoveToPositionNode()); });
        Btn_MoveDistance = BindButtonNode("Contain_ActionNodes/Button_MoveDistance", () => { AddChildNode(new BTMoveDistanceNode()); });
        Btn_DetectAgentInArea = BindButtonNode("Contain_ActionNodes/Button_DetectAgentInArea", () => { AddChildNode(new BTDetectAgentInAreaNode()); });
        Btn_MoveOneFrame = BindButtonNode("Contain_ActionNodes/Button_MoveOneFrame", () => { AddChildNode(new BTMoveOneFrameNode()); });
        Btn_ClearTarget = BindButtonNode("Contain_ActionNodes/Button_ClearTarget", () => { AddChildNode(new BTClearTargetNode()); });
        Btn_Idle = BindButtonNode("Contain_ActionNodes/Button_Idle", () => { AddChildNode(new BTIdleNode()); });
        Btn_Attack = BindButtonNode("Contain_ActionNodes/Button_Attack", () => { AddChildNode(new BTAttackNode()); });


        Btn_CheckDistanceToTargetEntity = BindButtonNode("Contain_ConditionNodes/Button_CheckDistanceToEntity", () => { AddChildNode(new BTCheckDistanceToTargetEntityNode()); });
        Btn_CheckDistanceToTargetPosition = BindButtonNode("Contain_ConditionNodes/Button_CheckDistanceToPosition", () => { AddChildNode(new BTCheckDistanceToTargetPositionNode()); });
        Btn_CheckTargetEntityInLogicArea = BindButtonNode("Contain_ConditionNodes/Button_CheckTargetEntityInLogicArea", () => { AddChildNode(new BTCheckTargetEntityInLogicAreaNode()); });
        Btn_CheckInStatus = BindButtonNode("Contain_ConditionNodes/Button_CheckInStatus", () => { AddChildNode(new BTCheckInStatusNode()); });
        Btn_CheckHasTarget = BindButtonNode("Contain_ConditionNodes/Button_CheckHasTarget", () => { AddChildNode(new BTCheckHasTarget()); });
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
        }
        else if(mNode is BTDecorNode)
        {
            BTDecorNode decor = mNode as BTDecorNode;
            if(decor.GetChildNode() == null)
            {
                decor.SetChildNode(childNode);
            }
        }
        else if (mNode is BTTree)
        {
            BTTree tree = mNode as BTTree;
            if (tree.GetChildNode() == null)
            {
                tree.SetChildNode(childNode);
            }
        }
        else if(mNode is BTLeafNode)
        {
            Log.Error(LogLevel.Normal, "AddChildNode Error, BTLeafNode can't add child node!");
        }

        GameEventSystem.Ins.Fire("UpdateAILogicArea");

        GameEventSystem.Ins.Post("SelectNode", childNode);
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
