using System.Collections.Generic;
using UnityEngine;

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

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        if (cmd.CmdType == AgentCommandDefine.BE_HIT)
        {
            ExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.ATTACK_HARD)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.RUN)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.DASH)
        {
            DelayToMeterExcuteCommand(cmd);
        }
        else if (cmd.CmdType == AgentCommandDefine.IDLE)
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

        if (_cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.RUN:
                    ExcuteCommand(cmdType, towards);
                    return;
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            AnimQueueMoveOn();
        }
    }
}
