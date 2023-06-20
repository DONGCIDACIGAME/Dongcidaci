using TMPro;

public class ControlChildTreeNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_Text Text_ChildTreeName;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        Text_ChildTreeName = BindTextNode("Text_ChildTreeName");
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
    }
}
