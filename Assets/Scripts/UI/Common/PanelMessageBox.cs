using GameEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class PanelMessageBox : UIPanel
{
    private Button Button_Quit;
    private TMP_Text Text_Title;
    private TMP_Text Text_Content;
    private Button Button_OK;

    private Action mOkAction;

    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_TOP_STATIC;
    }

    protected override void BindUINodes()
    {
        Button_Quit = BindButtonNode("Button_Quit", OnClickBtnQuit);
        Button_OK = BindButtonNode("Button_OK", OnClickBtnOk);
        Text_Title = BindTextNode("Text_Title");
        Text_Content = BindTextNode("Text_Content");
    }

    private void OnClickBtnQuit()
    {
        UIManager.Ins.ClosePanel<PanelMessageBox>();
    }

    private void OnClickBtnOk()
    {
        if(mOkAction != null)
        {
            mOkAction();
        }
        OnClickBtnQuit();
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        string title = openArgs["title"] as string;
        string content = openArgs["content"] as string;

        Text_Title.text = title;
        Text_Content.text = content;
    }
}
