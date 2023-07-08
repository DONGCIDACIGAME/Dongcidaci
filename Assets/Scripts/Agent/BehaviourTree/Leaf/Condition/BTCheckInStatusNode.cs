public class BTCheckInStatusNode : BTLeafNode
{
    private string mTargetStatus;

    public string GetTargetStatus()
    {
        return mTargetStatus;
    }

    public void SetTargetStatus(string status)
    {
        mTargetStatus = status;
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (string.IsNullOrEmpty(mTargetStatus))
        {
            info = string.Format("Node {0} has check status is null or empty!", NodeName);
            return false;
        }

        if(!AgentStatusDefine.ALL_STATUS.Contains(mTargetStatus))
        {
            info = string.Format("Node {0} has check undefined status:{1}!", NodeName, mTargetStatus);
            return false;
        }

        if(mTargetStatus == AgentStatusDefine.EMPTY)
        {
            info = string.Format("Node {0} has check status EMPTY may not be right check logic!", NodeName);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckInStatusNode Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        string statusName = mExcutor.StatusMachine.GetCurStatusName();
        if(!mTargetStatus.Equals(statusName))
            return BTDefine.BT_ExcuteResult_Failed;

        if (mLogEnable)
        {
            PrintLog(string.Format("BTCheckInStatusNode check in status:{0} succeed", mTargetStatus));
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_string("TargetStatus", mTargetStatus);
        return new BTNodeArg[] { arg1 };
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseString(args[0], out string value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        mTargetStatus = value1;
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckInStatus;
    }
}
