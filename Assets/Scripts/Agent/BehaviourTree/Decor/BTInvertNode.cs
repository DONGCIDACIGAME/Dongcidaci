public class BTInvertNode : BTDecorNode
{
    private int Inverse(int result)
    {
        if (mChildNode == null)
            return BTDefine.BT_CheckResult_Failed;

        if (result == BTDefine.BT_CheckResult_Failed)
            return BTDefine.BT_CheckResult_Succeed;

        if (result == BTDefine.BT_CheckResult_Running)
            return BTDefine.BT_CheckResult_Running;

        if (result == BTDefine.BT_CheckResult_Succeed)
            return BTDefine.BT_CheckResult_Failed;

        return BTDefine.BT_CheckResult_Failed;
    }

    public override int Excute(float deltaTime)
    {
        // 子节点执行
        int result = mChildNode.Excute(deltaTime);
        // 反转执行结果
        return Inverse(result);
    }
}
