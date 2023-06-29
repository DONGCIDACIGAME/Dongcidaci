/// <summary>
/// 重复节点，将会重复执行子节点n次
/// </summary>
public class BTRepeatNode : BTDecorNode
{
    private int mTotalRepeatTime;
    private int mHasRepeatTime;

    public void SetRepeatTime(int repeatTime)
    {
        mTotalRepeatTime = repeatTime;
        mHasRepeatTime = repeatTime;
    }

    public int GetRepeatTime()
    {
        return mTotalRepeatTime;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_Repeat;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetRepeatTime(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg_int arg1 = new BTNodeArg_int("RepeatTime", mTotalRepeatTime);
        return new BTNodeArg[] { arg1 };
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        int result = mChildNode.Excute(deltaTime);

        if (result == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Failed;

        if (result == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (result == BTDefine.BT_ExcuteResult_Succeed)
        {
            mHasRepeatTime++;

            if (mHasRepeatTime >= mTotalRepeatTime)
            {
                return BTDefine.BT_ExcuteResult_Succeed;
            }
            else
            {
                return BTDefine.BT_ExcuteResult_Running;
            }
        }

        return InvalidExcuteResult();
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if(!base.BTNodeDataCheck(ref info))
        {
            return false;
        }

        if(mTotalRepeatTime < 0)
        {
            info = string.Format("Node {0} repeate time must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    public override void Reset()
    {
        base.Reset();
        mHasRepeatTime = 0;
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        mHasRepeatTime = 0;
        mTotalRepeatTime = 0;
    }
}