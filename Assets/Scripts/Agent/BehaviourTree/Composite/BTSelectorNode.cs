/// <summary>
/// 选择执行节点，当遇到第一个执行结果为succeed时即返回succeed
/// </summary>
public class BTSelectorNode : BTCompositeNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Composite_Selector;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_ExcuteResult_Failed;

        int ret = BTDefine.BT_ExcuteResult_Failed;
        for (int i = 0;i<mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            if (result == BTDefine.BT_ExcuteResult_Succeed)
            {
                ret = BTDefine.BT_ExcuteResult_Succeed;
                break;
            }

            if (result == BTDefine.BT_ExcuteResult_Running)
            {
                ret = BTDefine.BT_ExcuteResult_Running;
                break;
            }
        }

        if(ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            for (int i = 0; i < mChildNodes.Count; i++)
            {
                BTNode node = mChildNodes[i];
                node.Reset();
            }
        }

        return ret;
    }
}