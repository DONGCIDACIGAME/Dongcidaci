using TMPro;

public class ControlCheckDistanceToPositionPropertyPage : ControlCheckDistancePropertyPage
{
    private TMP_InputField InputField_TagetPosition_X;
    private TMP_InputField InputField_TagetPosition_Z;

    protected override void BindUINodes()
    {
        base.BindUINodes();

        InputField_TagetPosition_X = BindInputFieldNode("InputField_TagetPosition_X", OnEditTargetPositionX);
        InputField_TagetPosition_Z = BindInputFieldNode("InputField_TagetPosition_Z", OnEditTargetPositionZ);
    }

    private void OnEditTargetPositionX(string x)
    {
        OnEditTargetPosition(x, InputField_TagetPosition_Z.text);
    }

    private void OnEditTargetPositionZ(string z)
    {
        OnEditTargetPosition(InputField_TagetPosition_X.text, z);
    }

    private void OnEditTargetPosition(string x, string z)
    {
        float xValue = float.Parse(x);
        float zValue = float.Parse(z);

        BTCheckDistanceToTargetPositionNode node = mNode as BTCheckDistanceToTargetPositionNode;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "OnEditTargetPosition Error, node is not BTCheckDistanceToTargetPositionNode, actual type:{0}", mNode.GetType());
            return;
        }
        node.SetTargetPosition(new UnityEngine.Vector3(xValue, 0, zValue));
    }

    protected override void Initialize()
    {
        base.Initialize();

    }
}
