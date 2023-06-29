using GameEngine;
using TMPro;

public class ControlMoveTimeNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_MoveTime;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_MoveTime = BindInputFieldNode("InputField_MoveTime", OnEditMoveTime);
    }

    private void OnEditMoveTime(string str)
    {
        float time = float.Parse(str);

        BTMoveTimeNode node = mNode as BTMoveTimeNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveTime Error, node is not BTMoveTimeNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetTotalMoveTime(time);

        GameEventSystem.Ins.Post("UpdateAILogicArea");
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTMoveTimeNode node = mNode as BTMoveTimeNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTMoveTimeNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_MoveTime.text = node.GetTotalMoveTime().ToString();
    }
}
