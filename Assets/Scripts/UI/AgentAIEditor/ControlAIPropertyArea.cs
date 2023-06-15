using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAIPropertyArea : UIControl
{
    private ControlAddNewNodePage ctl_AddNewNodePage;

    protected override void BindUINodes()
    {
        ctl_AddNewNodePage = BindControl<ControlAddNewNodePage>("Ctl_AddNewNode");
    }

    private void BindEvents()
    {
        mEventListener.Listen<BTNode>("OnClickAddChildNode", OnClickAddChildNode);
    }

    private void OnClickAddChildNode(BTNode node)
    {
        ctl_AddNewNodePage.SetCurSelectNode(node);
        ctl_AddNewNodePage.SetVisible(true);
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        BindEvents();
    }

    protected override void OnClose()
    {

    }
}
