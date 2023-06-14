using GameEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PanelAIEditor : UIPanel
{
    private ControlAILogicArea Ctl_AILogicArea;
    private ControlAIEditorTopBar Ctl_TopBar;
    private ControlAIPropertyArea Ctl_PropertyArea; 
    private Button Btn_Quit;

    /// <summary>
    /// 当前正在编辑中的行为树
    /// </summary>
    private BTTree mCurEditingTree;

    /// <summary>
    /// 当前行为树的名字
    /// </summary>
    private string mCurTreeFileName;

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
        Ctl_PropertyArea = BindControl<ControlAIPropertyArea>("Ctl_PropertyArea");
        Btn_Quit = BindButtonNode("Button_Quit", OnClickBtnQuit);
    }


    private void BindEvents()
    {
        mEventListener.Listen("OnClickLoadTree", OpenLoadTreeHUD);
        mEventListener.Listen("OnClickSaveTree", SaveTree);
        mEventListener.Listen("OnClickNewTree", CreateNewTree);
        mEventListener.Listen<string>("LoadNewTree", LoadTree);
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        BindEvents();
    }


    private void OpenLoadTreeHUD()
    {
        UIManager.Ins.AddControl<ControlLoadTreeHUD>(this, "Prefabs/UI/AgentAIEditor/Ctl_LoadTreeHUD", this.mUIRoot, null);
    }

    private void OnClickBtnQuit()
    {
        if(mDataChanged)
        {
            // 提示数据未保存
        }
        else
        {
            UIManager.Ins.ClosePanel<PanelAIEditor>();
        }
    }

    private void LoadTree(string fileName)
    {
        if(string.IsNullOrEmpty(fileName))
        {
            Log.Error(LogLevel.Normal, "PanelAIEditor Load Tree Failed,file name is null or empty!");
            return;
        }

        mCurTreeFileName = fileName;
        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(fileName);
        mCurEditingTree = BehaviourTreeManager.Ins.LoadTree(fileFullPath);
        DrawTree(mCurEditingTree);
    }

    private void SaveTree()
    {
        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(mCurTreeFileName);
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
        Ctl_AILogicArea.Update(tree);
    }

    protected override void OnClose()
    {
        mDataChanged = false;
        mCurEditingTree = null;
    }
}
