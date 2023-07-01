public class ControlTreeEntryPropertyPage : ControlAINodePropertyPage
{
    protected override void Initialize()
    {
        base.Initialize();

        BTNode parent = mNode.GetParentNode();
        if (parent != null && parent is BTChildTree)
        {
            Button_Delete.gameObject.SetActive(false);
        }
    }
}
