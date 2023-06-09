public class BTInvertNode : BTDecorNode
{
    public BTInvertNode(BTNode childNode) : base(childNode)
    {
    }

    private int Inverse(int result)
    {
        if (result == BTDefine.BT_CheckResult_Failed)
            return BTDefine.BT_CheckResult_Succeed;

        if (result == BTDefine.BT_CheckResult_Keep)
            return BTDefine.BT_CheckResult_Keep;

        if (result == BTDefine.BT_CheckResult_Succeed)
            return BTDefine.BT_CheckResult_Failed;

        return BTDefine.BT_CheckResult_Unknown;
    }

    public override int Excute()
    {
        // 子节点执行
        int result = mChildNode.Excute();
        // 反转执行结果
        return Inverse(result);
    }
}
