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

        mCurAnimStateEndMeter = mStepLoopAnimDriver.MoveNext();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }
    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.ATTACK_LIGHT:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.BE_HIT:
                    ExcuteCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            mCurAnimStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }
}
