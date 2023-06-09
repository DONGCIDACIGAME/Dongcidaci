using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public  class ControlAIOperationArea : UIControl
{
    private UIControl mCurOperationPage;

    private GameObject Node_OperationPageContain;

    protected override void BindUINodes()
    {
        Node_OperationPageContain = BindNode("Node_OperationPageContain");
    }

    protected override void BindEvents()
    {
        base.BindEvents();
        mEventListener.Listen<ControlAINode>("OnClickAddChildNode", OnClickAddChildNode);
        mEventListener.Listen<ControlAINode>("ClickAINodeCtl", OnNodeCtlClick);
        mEventListener.Listen<BTNode>("SelectNode", OnNodeSelect);
        mEventListener.Listen("ClearOperationArea", ClearOperationArea);
    }

    public void ClearOperationArea()
    {
        if(mCurOperationPage != null)
        {
            UIManager.Ins.RemoveControl(mCurOperationPage);
            mCurOperationPage = null;
        }
    }

    private void OnNodeCtlClick(ControlAINode nodeCtl)
    {
        if (nodeCtl == null)
            return;

        BTNode node = nodeCtl.GetBTNode();

        OnNodeSelect(node);
    }

    private void OnNodeSelect(BTNode node)
    {
        if (node == null)
            return;

        if (mCurOperationPage != null)
        {
            UIManager.Ins.RemoveControl(mCurOperationPage);
            mCurOperationPage = null;
        }

        ShowNodePropertyPage(node);
    }

    private void OnClickAddChildNode(ControlAINode nodeCtl)
    {
        if (nodeCtl == null)
            return;

        BTNode node = nodeCtl.GetBTNode();

        if(mCurOperationPage is ControlAddNewNodePage)
        {
            (mCurOperationPage as ControlAddNewNodePage).SetBTNode(node);
            return;
        }

        if(mCurOperationPage != null)
        {
            UIManager.Ins.RemoveControl(mCurOperationPage);
        }

        mCurOperationPage = UIManager.Ins.AddControl<ControlAddNewNodePage>(
            this,
            "Prefabs/UI/AgentAIEditor/Ctl_AddNewNode",
            Node_OperationPageContain,
            new Dictionary<string, object>
            {
                { "node", node}
            });
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }

    protected override void OnClose()
    {

    }

    private void AddPropertyPage<T>(string prefabPath, BTNode node) 
        where T: ControlAINodePropertyPage, new()
    {
        string prefabFullPath = "Prefabs/UI/AgentAIEditor/AINodePropertyPage/" + prefabPath;

        mCurOperationPage = UIManager.Ins.AddControl<T>(
            this,
            prefabFullPath,
            Node_OperationPageContain,
            new Dictionary<string, object>
            {
                    { "node", node }
            });

    }


    /// <summary>
    /// 加载节点的属性面板
    /// ---------------------------增加新节点时需要更新
    /// </summary>
    /// <param name="node"></param>
    private void ShowNodePropertyPage(BTNode node)
    {
        #region tree
        if (node is BTTreeEntry)
        {
            AddPropertyPage<ControlTreeEntryPropertyPage>("Tree/Ctl_PropertyPage_TreeEntry", node);
        }
        else if (node is BTChildTree)
        {
            AddPropertyPage<ControlChildTreePropertyPage>("Tree/Ctl_PropertyPage_ChildTree", node);
        }
        #endregion
        #region composite
        else if (node is BTSequenceNode)
        {
            AddPropertyPage<ControlSequencePropertyPage>("Composite/Ctl_PropertyPage_Sequence", node);
        }
        else if (node is BTParallelNode)
        {
            AddPropertyPage<ControlParallelPropertyPage>("Composite/Ctl_PropertyPage_Parallel", node);
        }
        else if (node is BTSelectorNode)
        {
            AddPropertyPage<ControlSelectorPropertyPage>("Composite/Ctl_PropertyPage_Selector", node);
        }
        else if (node is BTIfElseNode)
        {
            AddPropertyPage<ControlIfElsePropertyPage>("Composite/Ctl_PropertyPage_IfElse", node);
        }
        else if (node is BTWithStateSequenceNode)
        {
            AddPropertyPage<ControlWithStateSequencePropertyPage>("Composite/Ctl_PropertyPage_WithStateSequence", node);
        }
        else if (node is BTWithStateSelectorNode)
        {
            AddPropertyPage<ControlWithStateSelectorPropertyPage>("Composite/Ctl_PropertyPage_WithStateSelector", node);
        }
        #endregion
        #region decor
        else if (node is BTRepeatNode)
        {
            AddPropertyPage<ControlRepeatNodePropertyPage>("Decor/Ctl_PropertyPage_Repeat", node);
        }
        else if (node is BTInvertNode)
        {
            AddPropertyPage<ControlInvertNodePropertyPage>("Decor/Ctl_PropertyPage_Invert", node);
        }
        else if (node is BTOnceNode)
        {
            AddPropertyPage<ControlOnceNodePropertyPage>("Decor/Ctl_PropertyPage_Once", node);
        }
        else if (node is BTUntilTrueNode)
        {
            AddPropertyPage<ControlUntilTrueNodePropertyPage>("Decor/Ctl_PropertyPage_UntilTrue", node);
        }
        else if (node is BTResetNode)
        {
            AddPropertyPage<ControlResetNodePropertyPage>("Decor/Ctl_PropertyPage_Reset", node);
        }
        #endregion
        #region condition
        else if (node is BTCheckDistanceToTargetEntityNode)
        {
            AddPropertyPage<ControlCheckDistanceToEntityPropertyPage>("Condition/Ctl_PropertyPage_CheckDistanceToEntity", node);
        }
        else if (node is BTCheckDistanceToTargetPositionNode)
        {
            AddPropertyPage<ControlCheckDistanceToPositionPropertyPage>("Condition/Ctl_PropertyPage_CheckDistanceToPosition", node);
        }
        else if (node is BTCheckTargetEntityInLogicAreaNode)
        {
            AddPropertyPage<ControlCheckEntityInLogicAreaPropertyPage>("Condition/Ctl_PropertyPage_CheckEntityInLogicArea", node);
        }
        else if (node is BTCheckInStatusNode)
        {
            AddPropertyPage<ControlCheckInStatusPropertyPage>("Condition/Ctl_PropertyPage_CheckInStatus", node);
        }
        else if (node is BTCheckHasTarget)
        {
            AddPropertyPage<ControlCheckHasTargetPropertyPage>("Condition/Ctl_PropertyPage_CheckHasTarget", node);
        }
        #endregion
        #region action
        else if (node is BTWaitTimeNode)
        {
            AddPropertyPage<ControlWaitTimePropertyPage>("Action/Ctl_PropertyPage_WaitTime", node);
        }
        else if (node is BTWaitFrameNode)
        {
            AddPropertyPage<ControlWaitFramePropertyPage>("Action/Ctl_PropertyPage_WaitFrame", node);
        }
        else if (node is BTWaitMeterNode)
        {
            AddPropertyPage<ControlWaitMeterPropertyPage>("Action/Ctl_PropertyPage_WaitMeter", node);
        }
        else if (node is BTAgentChangeTowardsNode)
        {
            AddPropertyPage<ControlChangeTowardsPropertyPage>("Action/Ctl_PropertyPage_ChangeTowards", node);
        }
        else if (node is BTMoveTimeNode)
        {
            AddPropertyPage<ControlMoveTimePropertyPage>("Action/Ctl_PropertyPage_MoveTime", node);
        }
        else if (node is BTMoveMeterNode)
        {
            AddPropertyPage<ControlMoveMeterPropertyPage>("Action/Ctl_PropertyPage_MoveMeter", node);
        }
        else if (node is BTMoveDistanceNode)
        {
            AddPropertyPage<ControlMoveDistancePropertyPage>("Action/Ctl_PropertyPage_MoveDistance", node);
        }
        else if (node is BTMoveToPositionNode)
        {
            AddPropertyPage<ControlMoveToPositionPropertyPage>("Action/Ctl_PropertyPage_MoveToPosition", node);
        }
        else if (node is BTDetectAgentInAreaNode)
        {
            AddPropertyPage<ControlDetectAgentInAreaPropertyPage>("Action/Ctl_PropertyPage_DetectAgent", node);
        }
        else if (node is BTMoveOneFrameNode)
        {
            AddPropertyPage<ControlMoveOneFramePropertyPage>("Action/Ctl_PropertyPage_MoveOneFrame", node);
        }
        else if (node is BTClearTargetNode)
        {
            AddPropertyPage<ControlClearTargetPropertyPage>("Action/Ctl_PropertyPage_ClearTarget", node);
        }
        else if (node is BTIdleNode)
        {
            AddPropertyPage<ControlChangeToIdlePropertyPage>("Action/Ctl_PropertyPage_Idle", node);
        }
        else if (node is BTAttackNode)
        {
            AddPropertyPage<ControlAttackPropertyPage>("Action/Ctl_PropertyPage_Attack", node);
        }
        #endregion
    }
}
