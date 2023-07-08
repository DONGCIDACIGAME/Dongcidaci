
using UnityEngine;

public class BTMoveDistanceNode : BTMoveNode
{
    private float mTotalMoveDistance;
    private float mHasMoveDistance;

    private float mMoveMaxTime;
    private float mHasMoveTime;


    private Vector3 mLastPos;

    public void SetTotalMoveDistance(float moveDistance)
    {
        mTotalMoveDistance = moveDistance;
    }

    public float GetTotalMoveDistance()
    {
        return mTotalMoveDistance;
    }

    public void SetMoveMaxTime(float maxTime)
    {
        mMoveMaxTime = maxTime;
    }

    public float GetMoveMaxTime()
    {
        return mMoveMaxTime;
    }

    public override int GetNodeArgNum()
    {
        return 2;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseFloat(args[0], out float value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        result = BehaviourTreeHelper.ParseFloat(args[1], out float value2);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetTotalMoveDistance(value1);
        SetMoveMaxTime(value2);
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_float("TotalMoveDistance", mTotalMoveDistance);
        BTNodeArg arg2 = new BTNodeArg_float("MoveMaxTime", mMoveMaxTime);
        return new BTNodeArg[] { arg1, arg2 };
    }
    public override bool BTNodeDataCheck(ref string info)
    {
        if (mTotalMoveDistance <= 0)
        {
            info = string.Format("Node {0} total move distance must greater than 0! ", NodeName);
            return false;
        }

        if(mMoveMaxTime <= 0)
        {
            info = string.Format("Node {0} move max time must greater than 0! ", NodeName);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTMoveDistanceNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }


        mHasMoveTime += deltaTime;
        if(mHasMoveTime >= mMoveMaxTime)
        {
            if (mLogEnable)
            {
                PrintLog("move end! over time! ret: Succeed");
            }
            return BTDefine.BT_ExcuteResult_Succeed;
        }


        if (mLastPos != Vector3.zero)
        {
            mHasMoveDistance += (mExcutor.GetPosition() - mLastPos).magnitude;
        }

        if(mHasMoveDistance >= mTotalMoveDistance)
        {
            Move();
            mLastPos = mExcutor.GetPosition();
            if (mLogEnable)
            {
                PrintLog(string.Format("Moving... hasMoved:{0}, totalDis:{1}, ret: Succeed", mHasMoveDistance, mTotalMoveDistance));
            }
            return BTDefine.BT_ExcuteResult_Running;
        }

        StopMove();
        if (mLogEnable)
        {
            PrintLog("move end! distance coverd! ret: Succeed");
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override void Reset()
    {
        mHasMoveTime = 0;
        mHasMoveDistance = 0;
    }

    protected override void CustomDispose()
    {
        mTotalMoveDistance = 0;
        mMoveMaxTime = 0;
        mHasMoveDistance = 0;
        mHasMoveTime = 0;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_MoveDistance;
    }
}
