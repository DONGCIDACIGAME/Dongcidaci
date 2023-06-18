using TMPro;
using GameEngine;

public class ControlRepeatNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_RepeatTime;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_RepeatTime = BindInputFieldNode("InputField_RepeatTime",OnEditRepeatTime);
    }

    private void OnEditRepeatTime(string str)
    {
        int time = int.Parse(str);

        BTRepeatNode node = mNode as BTRepeatNode;

        if(node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditRepeatTime Error, node is not BTRepeatNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetRepeatTime(time);

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTRepeatNode node = mNode as BTRepeatNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTRepeatNode, actual type:{0}", GetType(),  mNode.GetType());
            return;
        }

        InputField_RepeatTime.text = node.GetRepeatTime().ToString();
    }
}
