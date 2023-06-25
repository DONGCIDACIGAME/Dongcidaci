using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ControlChildTreeNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_Text Text_ChildTreeName;
    private TMP_Dropdown Dropdown_CopyType;

    private static List<TMP_Dropdown.OptionData> CopyTypeOptions = new List<TMP_Dropdown.OptionData>
    {
        new TMP_Dropdown.OptionData(BTDefine.BT_ChildTreeCopyType_New),
        new TMP_Dropdown.OptionData(BTDefine.BT_ChildTreeCopyType_Reference)
    };

    private int GetCopyTypeIndex(string copyType)
    {
        if (copyType == BTDefine.BT_ChildTreeCopyType_New)
        {
            return 0;
        }
        else if (copyType == BTDefine.BT_ChildTreeCopyType_Reference)
        {
            return 1;
        }

        return 0;
    }

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Text_ChildTreeName = BindTextNode("Text_ChildTreeName");
        Dropdown_CopyType = BindDropDownNode("Dropdown_CopyType", CopyTypeOptions, OnSelectCopyType);
    }

    private void OnSelectCopyType(int index)
    {
        if (mNode is not BTChildTree)
            return;

        BTChildTree childTree = mNode as BTChildTree;

        if(index == 0) // 选择了深拷贝
        {
            childTree.SetCopyType(BTDefine.BT_ChildTreeCopyType_New);
        }
        else if(index == 1) // 选择了浅拷贝
        {
            childTree.SetCopyType(BTDefine.BT_ChildTreeCopyType_Reference);
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        if (mNode == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Property page Error, BTNode is null!", GetType());
            return;
        }

        BTChildTree childTree = mNode as BTChildTree;
        Text_ChildTreeName.text = childTree.GetChildTreeFileName();
        Dropdown_CopyType.value = GetCopyTypeIndex(childTree.GetCopyType());
    }
}
