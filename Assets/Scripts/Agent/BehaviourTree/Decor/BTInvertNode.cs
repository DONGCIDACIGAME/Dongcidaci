public class BTInvertNode : BTDecorNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_Invert;
    }

    private int Inverse(int result)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        if (result == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Succeed;

        if (result == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (result == BTDefine.BT_ExcuteResult_Succeed)
            return BTDefine.BT_ExcuteResult_Failed;

        return BTDefine.BT_ExcuteResult_Failed;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        // 子节点执行
        int result = mChildNode.Excute(deltaTime);
        // 反转执行结果
        return Inverse(result);
    }
}
