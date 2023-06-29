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
            mCurOperationPage = UIManager.Ins.AddControl<ControlTreeEntryNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Tree/Ctl_TreeEntryNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
        { "node", node }
});
        }
        else if (node is BTChildTree)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlChildTreeNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Tree/Ctl_ChildTreeNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
    {"node", node }
}
                );
        }
        #endregion
        #region composite
        else if (node is BTSequenceNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlSequenceNodePropertyPage>(
    this,
    "Prefabs/UI/AgentAIEditor/AINodePropertyPage/Composite/Ctl_SequenceNodePropertyPage",
    Node_OperationPageContain,
    new Dictionary<string, object>
    {
        { "node", node }
    });
        }
        else if (node is BTParallelNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlParallelNodePropertyPage>(
    this,
    "Prefabs/UI/AgentAIEditor/AINodePropertyPage/Composite/Ctl_ParallelNodePropertyPage",
    Node_OperationPageContain,
    new Dictionary<string, object>
    {
                    { "node", node }
    });
        }
        else if (node is BTSelectNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlSelectorPropertyPage>(
    this,
    "Prefabs/UI/AgentAIEditor/AINodePropertyPage/Composite/Ctl_SelectorNodePropertyPage",
    Node_OperationPageContain,
    new Dictionary<string, object>
    {
                    { "node", node }
    });
        }
        #endregion
        #region decor
        else if (node is BTRepeatNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlRepeatNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Decor/Ctl_RepeatNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTInvertNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlInvertNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Decor/Ctl_InvertNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTOnceNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlOnceNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Decor/Ctl_OnceNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTUntilTrueNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlUntilTrueNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Decor/Ctl_UntilTrueNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTResetNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlResetNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Decor/Ctl_ResetNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        #endregion
        #region condition
        else if (node is BTCheckDetectAgentNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlCheckDetectAgentNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Condition/Ctl_CheckDetectAgentNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        #endregion
        #region action
        else if (node is BTWaitTimeNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlWaitTimeNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_WaitTimeNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTWaitFrameNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlWaitFrameNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_WaitFrameNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTWaitMeterNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlWaitMeterNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_WaitMeterNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if(node is BTAgentChangeTowardsNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlChangeTowardsNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_ChangeTowardsNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTMoveTimeNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlMoveTimeNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_MoveTimeNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTMoveMeterNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlMoveMeterNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_MoveMeterNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTMoveDistanceNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlMoveDistanceNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_MoveDistanceNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        else if (node is BTMoveToPositionNode)
        {
            mCurOperationPage = UIManager.Ins.AddControl<ControlMoveToPositionNodePropertyPage>(
this,
"Prefabs/UI/AgentAIEditor/AINodePropertyPage/Action/Ctl_MoveToPositionNodePropertyPage",
Node_OperationPageContain,
new Dictionary<string, object>
{
                    { "node", node }
});
        }
        #endregion
    }
}
