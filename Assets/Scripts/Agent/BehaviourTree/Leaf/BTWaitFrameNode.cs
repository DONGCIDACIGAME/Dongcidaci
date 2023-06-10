/// <summary>
/// 等待帧
/// </summary>
public class BTWaitFrameNode : BTLeafNode
{
    private int mHasWaitFrame;
    private int mTotalWaitFrame;

    public BTWaitFrameNode(int frameCnt)
    {
        mTotalWaitFrame = frameCnt;
        mHasWaitFrame = 0;
    }

    public override int Excute(float deltaTime)
    {
        mHasWaitFrame++;

        if (mHasWaitFrame >= mTotalWaitFrame)
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
        mHasWaitFrame = 0;
    }
}
