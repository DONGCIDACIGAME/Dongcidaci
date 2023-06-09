/// <summary>
/// 重复节点，将会重复执行子节点n次
/// </summary>
public class BTRepeatNode : BTDecorNode
{
    private int mTotalTime;
    private int mHasRepeateTime;
    public BTRepeatNode(BTNode childNode, int repeateTime) : base(childNode)
    {
        mTotalTime = repeateTime;
        mHasRepeateTime = 0;
    }

    public override int Excute()
    {
        int result = mChildNode.Excute();

        if (result == BTDefine.BT_CheckResult_Failed)
            return BTDefine.BT_CheckResult_Failed;

        if (result == BTDefine.BT_CheckResult_Keep)
            return BTDefine.BT_CheckResult_Keep;

        if (result == BTDefine.BT_CheckResult_Succeed)
        {
            mHasRepeateTime++;

            if (mHasRepeateTime >= mTotalTime)
            {
                return BTDefine.BT_CheckResult_Succeed;
            }
            else
            {
                return BTDefine.BT_CheckResult_Keep;
            }
        }

        return BTDefine.BT_CheckResult_Unknown;
    }
}