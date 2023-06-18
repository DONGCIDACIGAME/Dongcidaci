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

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_WaitTime;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseFloat(args[0], out float value);
        if (result == BTDefine.BT_ExcuteResult_Succeed)
        {
            SetWaitTime(value);
        }

        return result;
    }


    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg = new BTNodeArg();
        arg.ArgName = "WaitTime";
        arg.ArgType = BTDefine.BT_ArgType_float;
        arg.ArgContent = mTotalWaitTime.ToString();
        return new BTNodeArg[] { arg };
    }

    public override int Excute(float deltaTime)
    {
        mHasWaitTime += deltaTime;

        if(mHasWaitTime >= mTotalWaitTime)
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
        mHasWaitTime = 0;
    }
}
