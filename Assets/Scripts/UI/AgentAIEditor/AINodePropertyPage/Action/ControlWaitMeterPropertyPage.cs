using GameEngine;
using TMPro;

public class ControlWaitMeterPropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_WaitMeter;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_WaitMeter = BindInputFieldNode("InputField_WaitMeter", OnEditWaitMeterNum);
    }

    private void OnEditWaitMeterNum(string str)
    {
        int num = int.Parse(str);

        BTWaitMeterNode node = mNode as BTWaitMeterNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditWaitFrame Error, node is not BTWaitFrameNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetWaitMeterNum(num);
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTWaitMeterNode node = mNode as BTWaitMeterNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTWaitMeterNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_WaitMeter.text = node.GetWaitMeterNum().ToString();
    }


}
