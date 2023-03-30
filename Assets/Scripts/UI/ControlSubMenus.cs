using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class ControlSubMenus : UIControl
{
    private GameObject Node_Menus;


    protected override void BindUINodes()
    {
        Node_Menus = BindNode("Node_Menus");
    }

    public void AllignTo(UIEntity entity)
    {

    }

    public void AddMenu(string menu, SubMenuFunc func, params KeyCode[] shortcuts)
    {

    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        
    }
}
