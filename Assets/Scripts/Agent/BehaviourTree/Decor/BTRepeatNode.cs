/// <summary>
/// 重复节点，将会重复执行子节点n次
/// </summary>
public class BTRepeatNode : BTDecorNode
{
    private int mTotalTime;
    private int mHasRepeatTime;

    public void SetRepeatTime(int repeatTime)
    {
        mTotalTime = repeatTime;
        mHasRepeatTime = repeatTime;
    }

    public int GetRepeatTime()
    {
        return mTotalTime;
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
        if (result == BTDefine.BT_ExcuteResult_Succeed)
        {
            SetRepeatTime(value);
        }

        return result;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg = new BTNodeArg();
        arg.ArgName = "RepeatTime";
        arg.ArgType = BTDefine.BT_ArgType_int;
        arg.ArgContent = mTotalTime.ToString();
        return new BTNodeArg[] { arg };
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

            if (mHasRepeatTime >= mTotalTime)
            {
                return BTDefine.BT_ExcuteResult_Succeed;
            }
            else
            {
                return BTDefine.BT_ExcuteResult_Running;
            }
        }

        return BTDefine.BT_ExcuteResult_Failed;
    }
}