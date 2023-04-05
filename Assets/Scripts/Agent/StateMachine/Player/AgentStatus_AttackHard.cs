public class AgentStatus_AttackHard : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK_HARD;
    }

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        if (cmds.HasCommand(AgentCommandDefine.ATTACK_HARD))
        {
            cmdBuffer.AddCommand(AgentCommandDefine.ATTACK_HARD);
        }
        else if (cmds.HasCommand(AgentCommandDefine.RUN))
        {
            cmdBuffer.AddCommand(AgentCommandDefine.RUN);
        }
        else if (cmds.HasCommand(AgentCommandDefine.IDLE))
        {
            cmdBuffer.AddCommand(AgentCommandDefine.IDLE);
        }
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = cmdBuffer.PeekCommand();
        switch(command)
        {
            case AgentCommandDefine.IDLE:
                ChangeStatus(AgentStatusDefine.IDLE);
                return;
            case AgentCommandDefine.RUN:
                ChangeStatus(AgentStatusDefine.RUN);
                return;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        AnimQueueMoveOn();
    }
}
