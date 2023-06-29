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
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        int ret = mChildNode.Excute(deltaTime);
        if (ret == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Running;

        if (ret == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (ret == BTDefine.BT_ExcuteResult_Succeed)
            return BTDefine.BT_ExcuteResult_Succeed;

        return InvalidExcuteResult();
    }

}
