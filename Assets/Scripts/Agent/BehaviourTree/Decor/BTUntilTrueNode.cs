/// <summary>
/// 直到子节点返回succeed，否则一直返回running
/// </summary>
public class BTUntilTrueNode : BTDecorNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_UntilTrue;
    }
    public override int Excute(float deltaTime)
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        int ret = childNode.Excute(deltaTime);
        if (ret == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Running;

        if (ret == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            childNode.Reset();
            return BTDefine.BT_ExcuteResult_Succeed;
        }

        return InvalidExcuteResult();
    }

}
