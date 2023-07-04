/// <summary>
/// 带状态的选择执行节点
/// </summary>
public class BTWithStateSelectorNode : BTCompositeNode
{
    private int mLastRunningNodeIndex = -1;

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Composite_WithStateSelector;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_ExcuteResult_Failed;

        int startIndex = 0;
        if (mLastRunningNodeIndex >= 0)
        {
            startIndex = mLastRunningNodeIndex;
        }

        int ret = BTDefine.BT_ExcuteResult_Failed;
        for (int i = startIndex; i < mChildNodes.Count; i++)
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
                mLastRunningNodeIndex = i;
                break;
            }
        }

        if (ret != BTDefine.BT_ExcuteResult_Running)
        {
            for (int i = 0; i < mChildNodes.Count; i++)
            {
                BTNode node = mChildNodes[i];
                node.Reset();
            }
            mLastRunningNodeIndex = 0;
        }

        return ret;
    }
}