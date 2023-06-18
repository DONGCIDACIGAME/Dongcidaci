/// <summary>
/// 等待帧
/// </summary>
public class BTWaitFrameNode : BTLeafNode
{
    private int mHasWaitFrame;
    private int mTotalWaitFrame;

    public void SetWaitFrame(int frameCnt)
    {
        mTotalWaitFrame = frameCnt;
        mHasWaitFrame = 0;
    }

    public int GetWaitFrame()
    {
        return mTotalWaitFrame;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_WaitFrame;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value);
        if (result == BTDefine.BT_ExcuteResult_Succeed)
        {
            SetWaitFrame(value);
        }

        return result;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg = new BTNodeArg();
        arg.ArgName = "WaitFrame";
        arg.ArgType = BTDefine.BT_ArgType_int;
        arg.ArgContent = mTotalWaitFrame.ToString();
        return new BTNodeArg[] { arg };
    }

    public override int Excute(float deltaTime)
    {
        mHasWaitFrame++;

        if (mHasWaitFrame >= mTotalWaitFrame)
        {
            return BTDefine.BT_ExcuteResult_Succeed;
        }
        else
        {
            return BTDefine.BT_ExcuteResult_Running;
        }
    }

    public override void Reset()
    {
        mHasWaitFrame = 0;
    }
}
