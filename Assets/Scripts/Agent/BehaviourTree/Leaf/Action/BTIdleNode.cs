public class BTIdleNode : BTLeafNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTIdleNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        // 发送idle指令
        AgentCommand idleCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        idleCmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, DirectionDef.none);
        mExcutor.OnCommand(idleCmd);

        if (mLogEnable)
        {
            PrintLog("ChangeToIdle...");
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }



    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_Idle;
    }

    public override int GetNodeArgNum()
    {
        return 0;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        return null;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        return BTDefine.BT_LoadNodeResult_Succeed;
    }
}
