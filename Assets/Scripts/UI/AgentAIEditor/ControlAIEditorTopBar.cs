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

    private void OnLoadTreeClick()
    {
        GameEventSystem.Ins.Fire("OnClickLoadTree");
    }

    private void OnSaveTreeClick()
    {
        GameEventSystem.Ins.Fire("OnClickSaveTree");
    }


    private void OnBtnNewTreeClick()
    {
        GameEventSystem.Ins.Fire("OnClickNewTree");
    }

    private void OnBtnToDefaultClick()
    {
        GameEventSystem.Ins.Fire("ToAIEditorDefaultPos");
    }

    protected override void BindUINodes()
    {
        Btn_LoadTree = BindButtonNode("Button_LoadTreeFile", OnLoadTreeClick);
        Btn_SaveTree = BindButtonNode("Button_SaveTreeFile", OnSaveTreeClick);
        Btn_NewTree = BindButtonNode("Button_NewTree", OnBtnNewTreeClick);
        Btn_ToDefault = BindButtonNode("Button_ToDefault", OnBtnToDefaultClick);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
