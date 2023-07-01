/// <summary>
/// 选择执行节点，当遇到第一个执行结果为succeed时即返回succeed
/// </summary>
public class BTSelectNode : BTCompositeNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_DetailType_Composite_Selector;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_ExcuteResult_Failed;

        for (int i = 0;i<mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            if (result == BTDefine.BT_ExcuteResult_Succeed)
                return BTDefine.BT_ExcuteResult_Succeed;

            if (result == BTDefine.BT_ExcuteResult_Running)
                return BTDefine.BT_ExcuteResult_Running;
        }

        return BTDefine.BT_ExcuteResult_Failed;
    }
}