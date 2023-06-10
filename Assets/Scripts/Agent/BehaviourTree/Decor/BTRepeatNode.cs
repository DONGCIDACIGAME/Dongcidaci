/// <summary>
/// 重复节点，将会重复执行子节点n次
/// </summary>
public class BTRepeatNode : BTDecorNode
{
    private int mTotalTime;
    private int mHasRepeateTime;
    public BTRepeatNode(int repeateTime)
    {
        mTotalTime = repeateTime;
        mHasRepeateTime = 0;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_CheckResult_Failed;

        int result = mChildNode.Excute(deltaTime);

        if (result == BTDefine.BT_CheckResult_Failed)
            return BTDefine.BT_CheckResult_Failed;

        if (result == BTDefine.BT_CheckResult_Running)
            return BTDefine.BT_CheckResult_Running;

        if (result == BTDefine.BT_CheckResult_Succeed)
        {
            mHasRepeateTime++;

            if (mHasRepeateTime >= mTotalTime)
            {
                return BTDefine.BT_CheckResult_Succeed;
            }
            else
            {
                return BTDefine.BT_CheckResult_Running;
            }
        }

        return BTDefine.BT_CheckResult_Failed;
    }
}