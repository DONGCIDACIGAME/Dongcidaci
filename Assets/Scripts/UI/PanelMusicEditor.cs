using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using UnityEngine.UI;

public class PanelMusicEditor : UIPanel
{
    public override string GetPanelLayerPath()
    {
        return UIPathDef.UI_LAYER_NORMAL_DYNAMIC;
    }

    private Button Btn_File;
    //private GameObject File_SubMenus;
    //private Button Btn_Open;
    //private Button Btn_Load;
    //private Button Btn_Save;
    //private Button Btn_SaveAs;
    //private Button Btn_Exit;
    private GameObject SubMenuContainer;

    private ControlSubMenus ctl_submenus;



    protected override void BindUINodes()
    {
        Btn_File = BindButtonNode("Top/Top_Menu/Btn_File",OnBtnFileClicked);
        //File_SubMenus = BindNode("Top/Top_Menu/Btn_File/File_SubMenus");
        //Btn_Open = BindButtonNode("Top/Top_Menu/Btn_File/File_SubMenus/Btn_Open",OnBtnOpenClicked);
        //Btn_Load = BindButtonNode("Top/Top_Menu/Btn_File/File_SubMenus/Btn_Load", OnBtnLoadClicked);
        //Btn_Save = BindButtonNode("Top/Top_Menu/Btn_File/File_SubMenus/Btn_Save", OnBtnSaveClicked);
        //Btn_SaveAs = BindButtonNode("Top/Top_Menu/Btn_File/File_SubMenus/Btn_SaveAs", OnBtnSaveAsClicked);
        //Btn_Exit = BindButtonNode("Top/Top_Menu/Btn_File/File_SubMenus/Btn_Exit", OnBtnExitClicked);
        SubMenuContainer = BindNode("SubMenuContainer");
        ctl_submenus = UIManager.Ins.BindControl<ControlSubMenus>(this, SubMenuContainer);
    }

    private void OnBtnFileClicked()
    {
        //File_SubMenus.SetActive(!File_SubMenus.activeSelf);
        if(ctl_submenus != null)
        {
            ctl_submenus.AllignTo(this);
            //ctl_submenus.AddMenu();
        }

    }

    private void OnBtnOpenClicked()
    {

    }


    private void OnBtnLoadClicked()
    {

    }

    private void OnBtnSaveClicked()
    {

    }

    private void OnBtnSaveAsClicked()
    {

    }

    private void OnBtnExitClicked()
    {

    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
