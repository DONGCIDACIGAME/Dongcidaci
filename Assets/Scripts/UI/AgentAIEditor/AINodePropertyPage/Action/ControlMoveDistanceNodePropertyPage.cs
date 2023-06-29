using TMPro;
using GameEngine;

public class ControlMoveDistanceNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_MoveDistance;
    private TMP_InputField InputField_MaxMoveTime;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_MoveDistance = BindInputFieldNode("InputField_MoveDistance", OnEditMoveDistance);
        InputField_MaxMoveTime = BindInputFieldNode("InputField_MaxMoveTime", OnEditMaxMoveTime);
    }

    private void OnEditMoveDistance(string str)
    {
        float distance = float.Parse(str);

        BTMoveDistanceNode node = mNode as BTMoveDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveDistance Error, node is not BTMoveDistanceNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetTotalMoveDistance(distance);

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    private void OnEditMaxMoveTime(string str)
    {
        float maxMoveTime = float.Parse(str);

        BTMoveDistanceNode node = mNode as BTMoveDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveDistance Error, node is not BTMoveDistanceNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetMoveMaxTime(maxMoveTime);

        GameEventSystem.Ins.Fire("UpdateAILogicArea");
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTMoveDistanceNode node = mNode as BTMoveDistanceNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTMoveDistanceNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        InputField_MoveDistance.text = node.GetTotalMoveDistance().ToString();
        InputField_MaxMoveTime.text = node.GetMoveMaxTime().ToString();
    }
}
