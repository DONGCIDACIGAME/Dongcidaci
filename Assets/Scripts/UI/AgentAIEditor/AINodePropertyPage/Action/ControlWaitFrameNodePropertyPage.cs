using GameEngine;
using TMPro;

public class ControlWaitFrameNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_WaitFrame;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_WaitFrame = BindInputFieldNode("InputField_WaitFrame", OnEditWaitFrame);
    }

    private void OnEditWaitFrame(string str)
    {
        int frameCnt = int.Parse(str);

        BTWaitFrameNode node = mNode as BTWaitFrameNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditWaitFrame Error, node is not BTWaitFrameNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetWaitFrame(frameCnt);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTWaitFrameNode node = mNode as BTWaitFrameNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTWaitFrameNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_WaitFrame.text = node.GetWaitFrame().ToString();
    }


}
