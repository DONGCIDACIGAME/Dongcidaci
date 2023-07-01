public class BTMoveTimeNode : BTMoveNode
{
    private float mTotalMoveTime;
    private float mHasMoveTime;

    public void SetTotalMoveTime(float moveTime)
    {
        mTotalMoveTime = moveTime;
    }

    public float GetTotalMoveTime()
    {
        return mTotalMoveTime;
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

        SetTotalMoveTime(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_float("TotalMoveTime", mTotalMoveTime);
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalMoveTime <= 0)
        {
            info = string.Format("Node {0} wait time must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if(mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTMoveTimeNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if(mHasMoveTime < mTotalMoveTime)
        {
            Move();
            return BTDefine.BT_ExcuteResult_Running;
        }

        //StopMove();
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override void Reset()
    {
        mHasMoveTime = 0;
    }

    protected override void CustomDispose()
    {
        mTotalMoveTime = 0;
        mHasMoveTime = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_MoveTime;
    }
}
