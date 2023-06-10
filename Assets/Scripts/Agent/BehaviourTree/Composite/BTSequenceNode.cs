/// <summary>
/// 顺序执行节点，只有所有子节点返回的结果都是succeed时才返回succeed
/// </summary>
public class BTSequenceNode : BTCompositeNode
{
    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_CheckResult_Failed;

        for (int i=0;i<mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            if (result == BTDefine.BT_CheckResult_Failed)
                return BTDefine.BT_CheckResult_Failed;

            if (result == BTDefine.BT_CheckResult_Running)
                return BTDefine.BT_CheckResult_Running;
        }

        return BTDefine.BT_CheckResult_Succeed;
    }
}
