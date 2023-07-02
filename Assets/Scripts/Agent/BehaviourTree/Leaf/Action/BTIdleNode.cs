public class BTIdleNode : BTLeafNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        // 发送idle指令
        AgentInputCommand idleCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        idleCmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, DirectionDef.none);
        mExcutor.OnCommand(idleCmd);

        PrintLog("ChangeToIdle...");
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_Idle;
    }
}
