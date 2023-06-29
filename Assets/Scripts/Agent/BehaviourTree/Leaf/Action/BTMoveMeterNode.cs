public class BTMoveMeterNode : BTLeafNode
{
    private int mTotalMoveMeter;
    private int mHasMoveMeter;

    private bool mStartRecord = false;
    private int mStartMeter;

    public void SetTotalMoveMeter(int totalMoveMeter)
    {
        mTotalMoveMeter = totalMoveMeter;
    }

    public int GetTotalMoveMeter()
    {
        return mTotalMoveMeter;
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

        SetTotalMoveMeter(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg();
        arg1.ArgName = "TotalMoveMeter";
        arg1.ArgType = BTDefine.BT_ArgType_int;
        arg1.ArgContent = mTotalMoveMeter.ToString();
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalMoveMeter <= 0)
        {
            info = string.Format("Node {0} total move meter num must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTMoveMeterNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        // 如果在本拍的前50%，则从当前排开始记拍，否则从下一拍开始记拍
        if (!mStartRecord && MeterManager.Ins.GetCurrentMeterProgress() <= 0.5f)
        {
            mStartRecord = true;
            mStartMeter = MeterManager.Ins.MeterIndex;
        }
        else
        {
            return BTDefine.BT_ExcuteResult_Running;
        }

        // 计算已经过去了多少拍
        mHasMoveMeter = MeterManager.Ins.GetMeterOffset(mStartMeter, MeterManager.Ins.MeterIndex);

        if (mHasMoveMeter < mTotalMoveMeter)
        {
            mExcutor.MoveControl.Move(deltaTime);
            return BTDefine.BT_ExcuteResult_Running;
        }

        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override void Reset()
    {
        mHasMoveMeter = 0;   
    }

    protected override void CustomDispose()
    {
        mTotalMoveMeter = 0;
        mHasMoveMeter = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_MoveMeter;
    }
}
