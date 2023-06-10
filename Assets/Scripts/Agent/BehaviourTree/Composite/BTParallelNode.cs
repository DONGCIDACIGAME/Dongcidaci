/// <summary>
/// 并行执行节点，所有节点并行执行，
/// </summary>
public class BTParallelNode : BTCompositeNode
{
    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_CheckResult_Failed;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            // 执行中的节点不会影响其他节点的继续执行
            if (result == BTDefine.BT_CheckResult_Running)
                continue;

            if (result == BTDefine.BT_CheckResult_Failed)
                return BTDefine.BT_CheckResult_Failed;
        }

        return BTDefine.BT_CheckResult_Succeed;
    }
}