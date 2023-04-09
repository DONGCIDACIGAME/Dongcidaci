using System.Collections.Generic;

public class AgentStatus_Run : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
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
        if (CommonHandleOnCmd(cmds, AgentCommandDefine.ATTACK_HARD, AgentStatusDefine.ATTACK))
            return;

        //cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.RUN);
        cmdBuffer.BindCommand(cmds, AgentCommandDefine.RUN);
        cmdBuffer.AddCommandIfContain(cmds, AgentCommandDefine.IDLE);
    }

    protected override void ActionHandleOnMeter(int meterIndex)
    {
        mCurAnimStateMeterRecord++;

        byte command = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", command, meterIndex);
        switch (command)
        {
            case AgentCommandDefine.ATTACK_HARD:
                ChangeStatus(AgentStatusDefine.ATTACK);
                return;
            case AgentCommandDefine.IDLE:
                ChangeStatus(AgentStatusDefine.IDLE);
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
