public class AgentStatus_Run : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        if (cmds.HasCommand(AgentCommandDefine.ATTACK_HARD))
        {
            ChangeStatus(AgentStatusDefine.ATTACK_HARD);
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
        if(command == AgentCommandDefine.IDLE)
        {
            ChangeStatus(AgentStatusDefine.IDLE);
            return;
        }

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        AnimQueueMoveOn();
    }

}
