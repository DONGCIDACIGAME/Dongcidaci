using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAIEditor : UIPanel
{
    private ControlAITreeWorkingArea Ctl_TreeWorkingArea;
    private ControlAIEditorTopBar Ctl_TopBar;
    private ControlAIOperationArea Ctl_OperationArea;
    private GameObject Node_DynamicCtls;
    private Button Btn_Quit;

    public AIEditorClipboard ClipBoard { get; private set; }

    /// <summary>
    /// 当前正在编辑中的行为树
    /// </summary>
    private BTTree mCurEditingTree;

    /// <summary>
    /// 当前行为树的文件名
    /// </summary>
    private string mTreeFileName;

    /// <summary>
    /// 是否编辑过数据
    /// </summary>
    private bool mDataChanged;


    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_BOTTOM_DYNAMIC;
    }

    protected override void BindUINodes()
    {
        Ctl_TreeWorkingArea = BindControl<ControlAITreeWorkingArea>("Ctl_TreeWorkingArea");
        Ctl_TopBar = BindControl<ControlAIEditorTopBar>("Ctl_TopBar");
        Ctl_OperationArea = BindControl<ControlAIOperationArea>("Ctl_OperationArea");
        Node_DynamicCtls = BindNode("Node_DynamicCtls");
        Btn_Quit = BindButtonNode("Button_Quit", OnClickBtnQuit);
    }


    protected override void BindEvents()
    {
        mEventListener.Listen("OnClickSaveTree", ShowSaveTreeHUD);
        mEventListener.Listen("OnClickNewTree", CreateNewTree);
        mEventListener.Listen<string>("OnLoadTree", LoadTree);
        mEventListener.Listen<string>("SaveTree", SaveTree);
    }


    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        ClipBoard = new AIEditorClipboard();
    }

    private void OnClickBtnQuit()
    {
        //if(mDataChanged)
        //{
        //    // 提示数据未保存
        //}
        //else
        //{
        //    UIManager.Ins.ClosePanel<PanelAIEditor>();
        //}

        UIManager.Ins.ClosePanel<PanelAIEditor>();
    }

    private void LoadTree(string filePath)
    {
        BTTree tree = BehaviourTreeManager.Ins.LoadTreeWithFileFullPath(filePath, true);
        mTreeFileName = BehaviourTreeHelper.FileFullPathToTreeName(filePath);
        mCurEditingTree = tree;
        DrawTree(mCurEditingTree);
        Ctl_OperationArea.ClearOperationArea();
    }

    private void ShowSaveTreeHUD()
    {
        if (mCurEditingTree == null)
            return;

        UIManager.Ins.AddControl<ControlSaveTreeHUD>(this,
            "Prefabs/UI/AgentAIEditor/Ctl_SaveTreeHUD",
            Node_DynamicCtls,
            new Dictionary<string, object>{
                {"treeName",mTreeFileName }
            });
    }


    private void SaveTree(string treeName)
    {
        string info = string.Empty;
        BTNode node = BehaviourTreeHelper.FindFirstInvalidNode(mCurEditingTree, ref info);

        if(node != null)
        {
            UIManager.Ins.OpenPanel<PanelMessageBox>("Prefabs/UI/Common/Panel_MessageBox",
                new Dictionary<string, object>
                {
                                {"title", "行为树保存失败"},
                                {"content", info }
                });

            GameEventSystem.Ins.Fire("SelectNode", node);
            return;
        }

        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(treeName);
        if(!string.IsNullOrEmpty(fileFullPath))
        {
            BehaviourTreeManager.Ins.SaveTree(fileFullPath, mCurEditingTree);
            mDataChanged = false;
        }
    }

    private void CreateNewTree()
    {
        mCurEditingTree = BehaviourTreeManager.Ins.CreateBTNode(BTDefine.BT_Node_Type_Tree, BTDefine.BT_Node_Type_Tree_Entry) as BTTree;
        DrawTree(mCurEditingTree);
        mTreeFileName = string.Empty;
        GameEventSystem.Ins.Fire("SelectNode", mCurEditingTree);
        mDataChanged = true;
    }

    private void DrawTree(BTTree tree)
    {
        Ctl_TreeWorkingArea.SetTree(tree);
        Ctl_TreeWorkingArea.Draw();
    }

    protected override void OnClose()
    {
        mDataChanged = false;
        mCurEditingTree = null;
        if(ClipBoard != null)
        {
            ClipBoard.Dispose();
            ClipBoard = null;
        }
    }
}
