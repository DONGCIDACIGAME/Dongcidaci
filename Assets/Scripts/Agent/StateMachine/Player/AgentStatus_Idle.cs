using System.Collections.Generic;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        StartAnimQueue();
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetAnimQueue();
    }

    public override void OnCommands(AgentCommandBuffer cmds)
    {
        if(cmds.HasCommand(AgentCommandDefine.ATTACK_HARD))
        {
            ChangeStatus(AgentStatusDefine.ATTACK_HARD);
            return;
        }

        if (cmds.HasCommand(AgentCommandDefine.RUN))
        {
            ChangeStatus(AgentStatusDefine.RUN);
            return;
        }

        cmdBuffer.AddCommandIfHas(cmds, AgentCommandDefine.IDLE);
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
