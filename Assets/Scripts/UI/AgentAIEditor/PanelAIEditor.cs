using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelAIEditor : UIPanel
{
    private ControlAILogicArea Ctl_AILogicArea;
    private ControlAIEditorTopBar Ctl_TopBar;
    private ControlAIOperationArea Ctl_OperationArea;
    private GameObject Node_DynamicCtls;
    private Button Btn_Quit;

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
        Ctl_AILogicArea = BindControl<ControlAILogicArea>("Ctl_AILogicArea");
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
        BTTree tree = BehaviourTreeManager.Ins.LoadTree(filePath, true);
        string[] ret = filePath.Replace(".tree","").Split('/', System.StringSplitOptions.None);
        if(ret.Length > 0)
        {
            mTreeFileName = ret[ret.Length - 1];
        }
        mCurEditingTree = tree;
        DrawTree(mCurEditingTree);
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
        mDataChanged = true;
    }

    private void DrawTree(BTTree tree)
    {
        Ctl_AILogicArea.SetTree(tree);
        Ctl_AILogicArea.Draw();
    }

    protected override void OnClose()
    {
        mDataChanged = false;
        mCurEditingTree = null;
    }
}
