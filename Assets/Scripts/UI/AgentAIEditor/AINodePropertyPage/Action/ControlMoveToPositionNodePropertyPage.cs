using TMPro;
using GameEngine;

public class ControlMoveToPositionNodePropertyPage : ControlAINodePropertyPage
{
    private TMP_InputField InputField_MoveToPosition_X;
    private TMP_InputField InputField_MoveToPosition_Z;

    protected override void BindUINodes()
    {
        base.BindUINodes();
        InputField_MoveToPosition_X = BindInputFieldNode("InputField_MoveToPosition_X", OnEditMoveToPositionX);
        InputField_MoveToPosition_Z = BindInputFieldNode("InputField_MoveToPosition_Z", OnEditMoveToPositionZ);
    }

    private void OnEditMoveToPositionX(string str)
    {
        float x = float.Parse(str);
        float z = float.Parse(InputField_MoveToPosition_Z.text);

        BTMoveToPositionNode node = mNode as BTMoveToPositionNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveToPositionX Error, node is not BTMoveToPositionNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetTargetPosition(new UnityEngine.Vector3(x, 0, z));

        GameEventSystem.Ins.Post("UpdateAILogicArea");
    }

    private void OnEditMoveToPositionZ(string str)
    {
        float x = float.Parse(InputField_MoveToPosition_X.text);
        float z = float.Parse(str);

        BTMoveToPositionNode node = mNode as BTMoveToPositionNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditMoveToPositionZ Error, node is not BTMoveToPositionNode, actual type:{0}", mNode.GetType());
            return;
        }

        node.SetTargetPosition(new UnityEngine.Vector3(x, 0, z));

        GameEventSystem.Ins.Post("UpdateAILogicArea");
    }

    protected override void Initialize()
    {
        base.Initialize();

        BTMoveToPositionNode node = mNode as BTMoveToPositionNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "{0} Initialize Error, node is not BTMoveToPositionNode, actual type:{0}", GetType(), mNode.GetType());
            return;
        }

        UnityEngine.Vector3 moveToPosition = node.GetTargetPosition();
        InputField_MoveToPosition_X.text = moveToPosition.x.ToString();
        InputField_MoveToPosition_Z.text = moveToPosition.z.ToString();
    }
}
