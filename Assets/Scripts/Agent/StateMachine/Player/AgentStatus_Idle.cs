using System.Collections.Generic;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        if(cmds.HasCommand(AgentCommandDefine.ATTACK_HARD))
        {
            ChangeStatus(AgentStatusDefine.ATTACK_HARD);
        }
        else if (cmds.HasCommand(AgentCommandDefine.RUN))
        {
            ChangeStatus(AgentStatusDefine.RUN);
        }
        else if(cmds.HasCommand(AgentCommandDefine.IDLE))
        {
            cmdBuffer.AddCommand(AgentCommandDefine.IDLE);
        }
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        if (mCurAnimStateMeterRecord < mCurAnimStateMeterLen)
            return;

        // ½ÚÅÄ¼ÇÂ¼¹éÁã
        mCurAnimStateMeterRecord = 0;

        if (cmdBuffer.HasCommand(AgentCommandDefine.IDLE))
        {
            AnimQueueMoveOn();
        }
    }
}
