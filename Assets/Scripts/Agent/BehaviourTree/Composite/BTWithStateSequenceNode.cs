/// <summary>
/// 带状态的顺序执行节点
/// </summary>
public class BTWithStateSequenceNode : BTCompositeNode
{
    private int mLastRunningNodeIndex = 0;

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Composite_WithStateSequence;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return BTDefine.BT_ExcuteResult_Failed;

        int startIndex = 0;
        if(mLastRunningNodeIndex > 0)
        {
            startIndex = mLastRunningNodeIndex;
        }

        int ret = BTDefine.BT_ExcuteResult_Succeed;
        for (int i = startIndex; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute(deltaTime);

            if (result == BTDefine.BT_ExcuteResult_Failed)
            {
                ret = BTDefine.BT_ExcuteResult_Failed;
                break;
            }

            if (result == BTDefine.BT_ExcuteResult_Running)
            {
                mLastRunningNodeIndex = i;
                ret = BTDefine.BT_ExcuteResult_Running; ;
                break;
            }
        }

        if(ret != BTDefine.BT_ExcuteResult_Running)
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
