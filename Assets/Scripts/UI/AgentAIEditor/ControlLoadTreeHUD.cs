using GameEngine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlLoadTreeHUD : UIControl
{
    private Button Btn_Quit;
    private Button Btn_LoadTree;

    private void OnQuitClick()
    {
        UIManager.Ins.RemoveControl(this);
    }

    private void OnLoadTreeClick()
    {
        GameEventSystem.Ins.Fire("LoadNewTree","testTree");
        UIManager.Ins.RemoveControl(this);
    }

    protected override void BindUINodes()
    {
        Btn_Quit = BindButtonNode("Button_QuitLoadTreeHUD", OnQuitClick);
        Btn_LoadTree = BindButtonNode("Button_LoadTree", OnLoadTreeClick);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {


    }

}
