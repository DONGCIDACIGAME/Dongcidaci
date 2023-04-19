using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Idle : AgentStatus
{
    private StepAnimDriver animDriver;

    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        animDriver = new StepAnimDriver(mAgent, GetStatusName());
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        mCurAnimStateEndMeter = animDriver.MoveNext();
    }

    public override void OnExit()
    {
        base.OnExit();

        animDriver.Reset();
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.IDLE:
                DelayToMeterExcuteCommand(cmd);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ExcuteCommand(cmdType, towards);
                    return;
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex < mCurAnimStateEndMeter)
                return;

            mCurAnimStateEndMeter = animDriver.MoveNext();
        }
    }
}
