using GameEngine;
using System.Collections.Generic;
using UnityEngine;



public class ControlAILogicArea : UIControl
{
    private GameObject Node_AILogicArea;
    private BTTree mCurEditingTree;


    protected override void BindUINodes()
    {
        Node_AILogicArea = BindNode("Node_AILogicArea");
    }


    private void BindEvents()
    {
        mEventListener.Listen("ToAIEditorDefaultPos", ToDefaultPos);
    }

    /// <summary>
    /// 移动回默认位置，默认缩放
    /// </summary>
    private void ToDefaultPos()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        BindEvents();
    }




    public void Update(BTTree tree)
    {
        mCurEditingTree = tree;
        ControlAIActionNode treeEntry = UIManager.Ins.AddControl<ControlAIActionNode>(
        this,
        "Prefabs/UI/AgentAIEditor/Ctl_AIAction",
        Node_AILogicArea,
        new Dictionary<string, object>()
        {
            { "BTNode", tree }
        });

        float treeWidth = treeEntry.GetWidth();
        float treeHeight = treeEntry.GetHeight();
        (Node_AILogicArea.transform as RectTransform).sizeDelta = new Vector2(treeWidth, treeHeight);
    }

    protected override void OnClose()
    {

    }
}
