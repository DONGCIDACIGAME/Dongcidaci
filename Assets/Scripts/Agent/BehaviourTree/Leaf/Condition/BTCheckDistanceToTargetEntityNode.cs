public class BTCheckDistanceToTargetEntityNode : BTCheckDistanceNode
{
    public override int GetNodeArgNum()
    {
        return 2;
    }
    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        return base.ParseNodeArgs(args);
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("Operator", mOperator);
        BTNodeArg arg2 = new BTNodeArg_float("CheckDistance", mCheckDistance);
        return new BTNodeArg[] { arg1, arg2 };
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
        base.Reset();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckDistanceToEntity;
    }

}
