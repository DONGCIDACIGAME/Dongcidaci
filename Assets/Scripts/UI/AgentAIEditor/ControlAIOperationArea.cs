using GameEngine;
using System.Collections.Generic;

public  class ControlAIOperationArea : UIControl
{
    private UIControl mCurOperationPage;

    protected override void BindUINodes()
    {
        
    }

    protected override void BindEvents()
    {
        base.BindEvents();
        mEventListener.Listen<ControlAINode>("OnClickAddChildNode", OnClickAddChildNode);
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
            GetRootObj(),
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
}
