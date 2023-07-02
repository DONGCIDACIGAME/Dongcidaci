public class BTInvertNode : BTDecorNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_Invert;
    }

    private int Inverse(int result)
    {

        if (result == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Succeed;

        if (result == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (result == BTDefine.BT_ExcuteResult_Succeed)
            return BTDefine.BT_ExcuteResult_Failed;

        return InvalidExcuteResult();
    }

    public override int Excute(float deltaTime)
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        // 子节点执行
        int result = childNode.Excute(deltaTime);
        // 反转执行结果
        int finalRet = Inverse(result);

        if(finalRet == BTDefine.BT_ExcuteResult_Succeed)
        {
            childNode.Reset();
        }

        return finalRet;
    }
}
