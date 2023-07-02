public class BTCheckDistanceToTarget : BTLeafNode
{
    private float mCheckDistance;
    private int mOperator;

    public void SetDistance(float distance)
    {
        mCheckDistance = distance;
    }

    public float GetDistance()
    {
        return mCheckDistance;
    }

    public void SetOperator(int op)
    {
        mOperator = op;
    }

    public int GetOperator()
    {
        return mOperator;
    }

    public override int GetNodeArgNum()
    {
        return 2;
    }
    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        result = BehaviourTreeHelper.ParseFloat(args[1], out float value2);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetOperator(value1);
        SetDistance(value2);
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("Operator", mOperator);
        BTNodeArg arg2 = new BTNodeArg_float("CheckDistance", mCheckDistance);
        return new BTNodeArg[] { arg1, arg2 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mOperator != BTDefine.BT_Operator_Equal
            && mOperator != BTDefine.BT_Operator_GreaterThan
            && mOperator != BTDefine.BT_Operator_GreaterEqual
            && mOperator != BTDefine.BT_Operator_LessEqual
            && mOperator != BTDefine.BT_Operator_LessThan)
        {
            info = string.Format("Node {0} has Invalid Operator Type:{1}", NodeName, mOperator);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if(mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckDistanceToTarget Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if(!mContext.TryGetValue("TargetEntity", out object obj))
        {
            Log.Error(LogLevel.Normal, "BTCheckDistanceToTarget Error, no target entity!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        MapEntity target = obj as MapEntity;
        if(target == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckDistanceToTarget Error, target entity is not map entity!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        float distance = (mExcutor.GetPosition() - target.GetPosition()).magnitude;
        bool ret = false;
        switch (mOperator)
        {
            case BTDefine.BT_Operator_LessThan:
                ret = distance < mCheckDistance;
                break;
            case BTDefine.BT_Operator_LessEqual:
                ret = distance <= mCheckDistance;
                break;
            case BTDefine.BT_Operator_Equal:
                ret = distance == mCheckDistance;
                break;
            case BTDefine.BT_Operator_GreaterThan:
                ret = distance > mCheckDistance;
                break;
            case BTDefine.BT_Operator_GreaterEqual:
                ret = distance >= mCheckDistance;
                break;
            default:
                break;
        }

        if(!ret)
        {
            PrintLog(string.Format("Check failed! now dis:{0}, target dis:{1}", distance, mCheckDistance));
            return BTDefine.BT_ExcuteResult_Failed;
        }


        PrintLog(string.Format("Check distance succeed~ now dis:{0}, target dis:{1}",distance, mCheckDistance));
        return BTDefine.BT_ExcuteResult_Succeed;
    }


    public override void Reset()
    {
        
    }

    protected override void CustomDispose()
    {
        mCheckDistance = 0;
        mOperator = BTDefine.BT_Operator_Undefine;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckDistance;
    }

}
