using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAIEditorTopBar : UIControl
{
    private Button Btn_LoadTree;
    private Button Btn_SaveTree;
    private Button Btn_NewTree;
    private Button Btn_ToDefault;
    private Button Btn_Quit;

    private void OnBtnQuitClick()
    {
        UIManager.Ins.ClosePanel<PanelAIEditor>();
    }


    private void OnLoadTreeClick()
    {
        GameEventSystem.Ins.Fire("OnClickLoadTree");
    }

    private void OnSaveTreeClick()
    {
        
    }


    private void OnBtnNewTreeClick()
    {
        
    }

    private void OnBtnToDefaultClick()
    {

    }

    protected override void BindUINodes()
    {
        Btn_LoadTree = BindButtonNode("Button_LoadTreeFile", OnLoadTreeClick);
        Btn_SaveTree = BindButtonNode("Button_SaveTreeFile", OnSaveTreeClick);
        Btn_NewTree = BindButtonNode("Button_NewTree", OnBtnNewTreeClick);
        Btn_ToDefault = BindButtonNode("Button_ToDefault", OnBtnToDefaultClick);
        Btn_Quit = BindButtonNode("Button_Quit", OnBtnQuitClick);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
