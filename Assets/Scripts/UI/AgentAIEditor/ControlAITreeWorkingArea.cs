using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class ControlAITreeWorkingArea : UIControl
{
    private GameObject Node_AILogicArea;
    private BTTree mCurEditingTree;
    private ControlAINode mTreeEntryNode;
    private ControlAINode mCurSelectAINode;
    private GameObject Node_TreeViewport;


    protected override void BindUINodes()
    {
        Node_TreeViewport = BindNode("Node_TreeViewport");
        Node_AILogicArea = BindNode("Node_AILogicArea");
    }


    protected override void BindEvents()
    {
        base.BindEvents();
        mEventListener.Listen("ToAIEditorDefaultPos", ToDefaultPos);
        mEventListener.Listen("UpdateAILogicArea", Draw);
        mEventListener.Listen<ControlAINode>("ClickAINodeCtl", OnNodeCtlClick);
        mEventListener.Listen<ControlAINode>("OnClickAddChildNode", OnAddChildClick);
    }

    /// <summary>
    /// 移动回默认位置，默认缩放
    /// </summary>
    private void ToDefaultPos()
    {
        Node_AILogicArea.transform.localPosition = Vector3.zero;
        Node_AILogicArea.transform.localScale = Vector3.one;
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }


    public void SetTree(BTTree tree)
    {
        mCurEditingTree = tree;
    }

    private void ShowSelection(ControlAINode curSelect)
    {
        if (mCurSelectAINode != null)
        {
            mCurSelectAINode.Deselect();
        }

        if (curSelect != null)
        {
            curSelect.Select();
            mCurSelectAINode = curSelect;
        }
    }

    private void OnAddChildClick(ControlAINode nodeCtl)
    {
        ShowSelection(nodeCtl);
    }

    private void OnNodeCtlClick(ControlAINode nodeCtl)
    {
        ShowSelection(nodeCtl);
    }

    public void Draw()
    {
        if(mTreeEntryNode != null)
        {
            UIManager.Ins.RemoveControl(mTreeEntryNode);
        }

        mTreeEntryNode = UIManager.Ins.AddControl<ControlAINode>(
        this,
        "Prefabs/UI/AgentAIEditor/Ctl_AINode",
        Node_AILogicArea,
        new Dictionary<string, object>()
        {
                { "BTNode", mCurEditingTree }
        });


        float treeWidth = mTreeEntryNode.GetWidth();
        float treeHeight = mTreeEntryNode.GetHeight();
        (Node_AILogicArea.transform as RectTransform).sizeDelta = new Vector2(treeWidth, treeHeight);
    }

    private void Zoom()
    {
        // 判断鼠标是否在行为树的绘制区域内
        bool mouseInLogicRect = RectTransformUtility.RectangleContainsScreenPoint(
            Node_TreeViewport.transform as RectTransform,
            Input.mousePosition,
            CameraManager.Ins.GetUICam());


        if (mouseInLogicRect)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                float scale = Node_AILogicArea.transform.localScale.x + scroll;
                Node_AILogicArea.transform.localScale = new Vector3(scale, scale, 1);
            }
        }
    }

    protected override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        Zoom();
    }

    protected override void OnClose()
    {

    }
}
