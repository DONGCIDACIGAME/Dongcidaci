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


    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetWaitFrame(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg();
        arg1.ArgName = "WaitFrame";
        arg1.ArgType = BTDefine.BT_ArgType_int;
        arg1.ArgContent = mTotalWaitFrame.ToString();
        return new BTNodeArg[] { arg1 };
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

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalWaitFrame < 0)
        {
            info = string.Format("Node {0} wait frame must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    protected override void CustomDispose()
    {
        mHasWaitFrame = 0;
        mTotalWaitFrame = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_WaitFrame;
    }
}
