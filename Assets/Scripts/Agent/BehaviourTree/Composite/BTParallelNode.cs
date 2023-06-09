/// <summary>
/// 并行执行节点，所有节点并行执行，
/// </summary>
public class BTParallelNode : BTCompositeNode
{
    public BTParallelNode(BTNode[] childNodes) : base(childNodes)
    {
    }

    public override int Excute()
    {
        for (int i = 0; i < mChildNodes.Length; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute();

            // 执行中的节点不会影响其他节点的继续执行
            if (result == BTDefine.BT_CheckResult_Keep)
                continue;

            if (result == BTDefine.BT_CheckResult_Failed)
                return BTDefine.BT_CheckResult_Failed;
        }

        return BTDefine.BT_CheckResult_Succeed;
    }
}