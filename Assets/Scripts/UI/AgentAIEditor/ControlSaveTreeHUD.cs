using GameEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ControlSaveTreeHUD : UIControl
{
    private Button Button_Quit;
    private Button Button_Save;
    private TMP_InputField InputField_TreeName;
    private TMP_Text Text_Hint;


    protected override void BindUINodes()
    {
        Button_Quit = BindButtonNode("Button_Quit", OnButtonQuitClick);
        Button_Save = BindButtonNode("Button_Save", OnButtonSaveClick);
        InputField_TreeName = BindInputFieldNode("InputField_TreeName", OnTreeNameEdit);
        Text_Hint = BindTextNode("Text_Hint");
    }

    private void OnButtonQuitClick()
    {
        UIManager.Ins.RemoveControl(this);
    }

    private void OnButtonSaveClick()
    {
        GameEventSystem.Ins.Fire("SaveTree", InputField_TreeName.text);
        UIManager.Ins.RemoveControl(this);
    }

   private void CheckSave(string treeName)
    {
        // 检查文件名
        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(treeName);
        if (FileHelper.FileExist(fileFullPath))
        {
            Text_Hint.text = "该行为树文件已存在，如果保存将覆盖原文件，请谨慎保存";
        }
    }

    private void OnTreeNameEdit(string treeName)
    {
        InputField_TreeName.text = treeName;

        CheckSave(treeName);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnOpen(Dictionary<string, object> openArgs)
    {
        string defaultFileName = openArgs["treeName"] as string;
        InputField_TreeName.text = defaultFileName;
        CheckSave(defaultFileName);
    }

}
