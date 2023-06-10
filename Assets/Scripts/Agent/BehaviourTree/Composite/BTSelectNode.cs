/// <summary>
/// 选择执行节点，当遇到第一个执行结果为succeed时即返回succeed
/// </summary>
public class BTSelectNode : BTCompositeNode
{
    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_CheckResult_Failed;

        for (int i = 0;i<mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            if (result == BTDefine.BT_CheckResult_Succeed)
                return BTDefine.BT_CheckResult_Succeed;

            if (result == BTDefine.BT_CheckResult_Running)
                return BTDefine.BT_CheckResult_Running;
        }

        return BTDefine.BT_CheckResult_Failed;
    }
}