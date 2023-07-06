public class BTWaitTimeNode : BTLeafNode
{
    private float mHasWaitTime;
    private float mTotalWaitTime;

    public void SetWaitTime(float time)
    {
        mTotalWaitTime = time;
        mHasWaitTime = 0;
    }

    public float GetWaitTime()
    {
        return mTotalWaitTime;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseFloat(args[0], out float value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetWaitTime(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }


    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_float("WaitTime", mTotalWaitTime);
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalWaitTime < 0)
        {
            info = string.Format("Node {0} wait time must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }


    public override int Excute(float deltaTime)
    {
        mHasWaitTime += deltaTime;

        if(mHasWaitTime < mTotalWaitTime)
        {
            //PrintLog(string.Format("waiting! has wait time:{0}, total wait time:{1}", mHasWaitTime, mTotalWaitTime));
            return BTDefine.BT_ExcuteResult_Running;
        }

        PrintLog(string.Format("wait time {0} end, ret:succeed~", mTotalWaitTime));
        return BTDefine.BT_ExcuteResult_Succeed;
    }


    public override void Reset()
    {
        mHasWaitTime = 0;
    }

    protected override void CustomDispose()
    {
        mHasWaitTime = 0;
        mTotalWaitTime = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_WaitTime;
    }
}
