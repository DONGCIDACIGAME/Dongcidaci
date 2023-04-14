using System.Collections.Generic;

public class AgentStatus_BeHit : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.BE_HIT;
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
        byte cmd = cmds.PeekCommand();

        if (cmd == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.ATTACK_HARD)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.RUN)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.DASH)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd == AgentCommandDefine.IDLE)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else
        {
            Log.Error(LogLevel.Info, "AgentStatus_BeHit - undefined cmd handle:{0}", cmd);
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        byte cmd = cmdBuffer.PeekCommand();
        Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmd, meterIndex);

        switch (cmd)
        {
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
                ExcuteCommand(cmd);
                return;
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }

        AnimQueueMoveOn();
    }
}
