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
        mEventListener.Listen<ControlAINode>("ClickAINode", OnNodeClick);
        mEventListener.Listen<BTNode>("DeleteAINode", OnDeleteNode);
    }

    private void OnDeleteNode(BTNode node)
    {
        ClearOperationArea();
    }

    public void ClearOperationArea()
    {
        if(mCurOperationPage != null)
        {
            UIManager.Ins.RemoveControl(mCurOperationPage);
            mCurOperationPage = null;
        }
    }

    private void OnNodeClick(ControlAINode nodeCtl)
    {
        if (nodeCtl == null)
            return;

        if(mCurOperationPage != null)
        {
            UIManager.Ins.RemoveControl(mCurOperationPage);
            mCurOperationPage = null;
        }

        BTNode node = nodeCtl.GetBTNode();
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
            AddPropertyPage<ControlTreeEntryNodePropertyPage>("Tree/Ctl_TreeEntryNodePropertyPage", node);
        }
        else if (node is BTChildTree)
        {
            AddPropertyPage<ControlChildTreeNodePropertyPage>("Tree/Ctl_ChildTreeNodePropertyPage", node);
        }
        #endregion
        #region composite
        else if (node is BTSequenceNode)
        {
            AddPropertyPage<ControlSequenceNodePropertyPage>("Composite/Ctl_SequenceNodePropertyPage", node);
        }
        else if (node is BTParallelNode)
        {
            AddPropertyPage<ControlParallelNodePropertyPage>("Composite/Ctl_ParallelNodePropertyPage", node);
        }
        else if (node is BTSelectNode)
        {
            AddPropertyPage<ControlSelectorPropertyPage>("Composite/Ctl_SelectorNodePropertyPage", node);
        }
        #endregion
        #region decor
        else if (node is BTRepeatNode)
        {
            AddPropertyPage<ControlRepeatNodePropertyPage>("Decor/Ctl_RepeatNodePropertyPage", node);
        }
        else if (node is BTInvertNode)
        {
            AddPropertyPage<ControlInvertNodePropertyPage>("Decor/Ctl_InvertNodePropertyPage", node);
        }
        else if (node is BTOnceNode)
        {
            AddPropertyPage<ControlOnceNodePropertyPage>("Decor/Ctl_OnceNodePropertyPage", node);
        }
        else if (node is BTUntilTrueNode)
        {
            AddPropertyPage<ControlUntilTrueNodePropertyPage>("Decor/Ctl_UntilTrueNodePropertyPage", node);
        }
        else if (node is BTResetNode)
        {
            AddPropertyPage<ControlResetNodePropertyPage>("Decor/Ctl_ResetNodePropertyPage", node);
        }
        #endregion
        #region condition
        else if (node is BTCheckDetectAgentNode)
        {
            AddPropertyPage<ControlCheckDetectAgentNodePropertyPage>("Condition/Ctl_CheckDetectAgentNodePropertyPage", node);
        }
        #endregion
        #region action
        else if (node is BTWaitTimeNode)
        {
            AddPropertyPage<ControlWaitTimeNodePropertyPage>("Action/Ctl_WaitTimeNodePropertyPage", node);
        }
        else if (node is BTWaitFrameNode)
        {
            AddPropertyPage<ControlWaitFrameNodePropertyPage>("Action/Ctl_WaitFrameNodePropertyPage", node);
        }
        else if (node is BTWaitMeterNode)
        {
            AddPropertyPage<ControlWaitMeterNodePropertyPage>("Action/Ctl_WaitMeterNodePropertyPage", node);
        }
        else if(node is BTAgentChangeTowardsNode)
        {
            AddPropertyPage<ControlChangeTowardsNodePropertyPage>("Action/Ctl_ChangeTowardsNodePropertyPage", node);
        }
        else if (node is BTMoveTimeNode)
        {
            AddPropertyPage<ControlMoveTimeNodePropertyPage>("Action/Ctl_MoveTimeNodePropertyPage", node);
        }
        else if (node is BTMoveMeterNode)
        {
            AddPropertyPage<ControlMoveMeterNodePropertyPage>("Action/Ctl_MoveMeterNodePropertyPage", node);
        }
        else if (node is BTMoveDistanceNode)
        {
            AddPropertyPage<ControlMoveDistanceNodePropertyPage>("Action/Ctl_MoveDistanceNodePropertyPage", node);
        }
        else if (node is BTMoveToPositionNode)
        {
            AddPropertyPage<ControlMoveToPositionNodePropertyPage>("Action/Ctl_MoveToPositionNodePropertyPage", node);
        }
        #endregion
    }
}
