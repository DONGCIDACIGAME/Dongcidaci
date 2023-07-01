using TMPro;
using GameEngine;

public class ControlWaitTimePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_WaitTime;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_WaitTime = BindInputFieldNode("InputField_WaitTime", OnEditWaitTime);
    }

    private void OnEditWaitTime(string str)
    {
        float time = float.Parse(str);

        BTWaitTimeNode node = mNode as BTWaitTimeNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditWaitTime Error, node is not BTWaitTimeNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetWaitTime(time);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTWaitTimeNode node = mNode as BTWaitTimeNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTWaitTimeNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_WaitTime.text = node.GetWaitTime().ToString();
    }
}
