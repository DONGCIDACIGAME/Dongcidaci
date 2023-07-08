using UnityEngine;

public class BTCheckDistanceToTargetPositionNode : BTCheckDistanceNode
{
    private Vector3 mTargetPos;

    public void SetTargetPosition(Vector3 targetPos)
    {
        mTargetPos = targetPos;
    }

    public Vector3 GetTargetPosition()
    {
        return mTargetPos;
    }

    public override int GetNodeArgNum()
    {
        return 3;
    }
    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = base.ParseNodeArgs(args);

        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        result = BehaviourTreeHelper.ParseVector3(args[2], out Vector3 value3);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;
        SetTargetPosition(value3);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("Operator", mOperator);
        BTNodeArg arg2 = new BTNodeArg_float("CheckDistance", mCheckDistance);
        BTNodeArg arg3 = new BTNodeArg_Vector3("TargetPosition", mTargetPos);
        return new BTNodeArg[] { arg1, arg2, arg3 };
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckDistanceToTarget Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        float distance = (mExcutor.GetPosition() - mTargetPos).magnitude;
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

        if (!ret)
        {
            if (mLogEnable)
            {
                PrintLog(string.Format("Check failed! now dis:{0}, target dis:{1}", distance, mCheckDistance));
            }
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if (mLogEnable)
        {
            PrintLog(string.Format("Check distance succeed~ now dis:{0}, target dis:{1}", distance, mCheckDistance));
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }


    public override void Reset()
    {
        base.Reset();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        mTargetPos = Vector3.zero;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckDistanceToPosition;
    }
}
