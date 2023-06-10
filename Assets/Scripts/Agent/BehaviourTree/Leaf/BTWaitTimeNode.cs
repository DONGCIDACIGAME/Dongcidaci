public class BTWaitTimeNode : BTLeafNode
{
    private float mHasWaitTime;
    private float mTotalWaitTime;

    public BTWaitTimeNode(float time)
    {
        mTotalWaitTime = time;
        mHasWaitTime = 0;
    }

    public override int Excute(float deltaTime)
    {
        mHasWaitTime += deltaTime;

        if(mHasWaitTime >= mTotalWaitTime)
        {
            return BTDefine.BT_CheckResult_Succeed;
        }
        else
        {
            return BTDefine.BT_CheckResult_Running;
        }
    }

    public override void Reset()
    {
        mHasWaitTime = 0;
    }
}
