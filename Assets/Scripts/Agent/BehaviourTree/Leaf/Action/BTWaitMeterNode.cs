public class BTWaitMeterNode : BTLeafNode
{
    private int mTotalWaitMeterNum;
    private int mHasWaitMeterNum;

    private bool mStartRecord = false;
    private int mStartMeter;
    private int mWaitingMeter;

    public void SetWaitMeterNum(int meterNum)
    {
        mTotalWaitMeterNum = meterNum;
    }

    public int GetWaitMeterNum()
    {
        return mTotalWaitMeterNum;
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

        SetWaitMeterNum(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }


    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("WaitMeterNum", mTotalWaitMeterNum);
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalWaitMeterNum < 0)
        {
            info = string.Format("Node {0} wait meter cnt must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        // 如果在本拍的前50%，则从当前排开始记拍，否则从下一拍开始记拍
        // 如果在本拍的前50%，则从当前排开始记拍，否则从下一拍开始记拍
        if (!mStartRecord)
        {
            if (MeterManager.Ins.GetCurrentMeterProgress() <= 0.5f)
            {
                mStartRecord = true;
                mStartMeter = MeterManager.Ins.MeterIndex;
            }
            else
            {
                mWaitingMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, 1);
            }
        }

        if (mWaitingMeter > 0 && MeterManager.Ins.MeterIndex < mWaitingMeter)
        {
            if (mLogEnable)
            {
                PrintLog(string.Format("waiting start meter...curMeterIndex:{0}, startMoveMeterIndex:{1}", MeterManager.Ins.MeterIndex, mWaitingMeter));
            }
            return BTDefine.BT_ExcuteResult_Running;
        }

        // 计算已经过去了多少拍
        mHasWaitMeterNum = MeterManager.Ins.GetMeterOffset(mStartMeter, MeterManager.Ins.MeterIndex);

        if (mHasWaitMeterNum < mTotalWaitMeterNum)
        {
            if (mLogEnable)
            {
                PrintLog(string.Format("waiting! has wait meter:{0}, total wait meter:{1}", mHasWaitMeterNum, mTotalWaitMeterNum));
            }
            return BTDefine.BT_ExcuteResult_Running;
        }

        if (mLogEnable)
        {
            PrintLog("wait end, ret:succeed~");
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    protected override void CustomDispose()
    {
        mTotalWaitMeterNum = 0;
        mHasWaitMeterNum = 0;
        mStartMeter = 0;
        mWaitingMeter = 0;
        mStartRecord = false;
    }

    public override void Reset()
    {
        mHasWaitMeterNum = 0;
        mStartRecord = false;
        mStartMeter = 0;
        mWaitingMeter = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_WaitMeter;
    }
}
