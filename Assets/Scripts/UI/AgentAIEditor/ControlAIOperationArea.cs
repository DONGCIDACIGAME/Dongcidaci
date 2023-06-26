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
        #endregion
        #region condition

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
            #endregion
    }
}
