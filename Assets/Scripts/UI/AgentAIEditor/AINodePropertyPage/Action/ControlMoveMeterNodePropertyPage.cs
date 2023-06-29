using TMPro;
using GameEngine;

public class ControlMoveMeterNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_MoveMeter;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_MoveMeter = BindInputFieldNode("InputField_MoveMeter", OnEditMoveMeter);
    }

    private void OnEditMoveMeter(string str)
    {
        int meterNum = int.Parse(str);

        BTMoveMeterNode node = mNode as BTMoveMeterNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveMeter Error, node is not BTMoveMeterNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetTotalMoveMeter(meterNum);

        GameEventSystem.Ins.Post("UpdateAILogicArea");
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTMoveMeterNode node = mNode as BTMoveMeterNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTMoveMeterNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_MoveMeter.text = node.GetTotalMoveMeter().ToString();
    }
}
