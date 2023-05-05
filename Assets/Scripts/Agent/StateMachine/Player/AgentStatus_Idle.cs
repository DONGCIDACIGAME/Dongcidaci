using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Idle : AgentStatus
{
    //private StepLoopAnimDriver animDriver;

    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        
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
            case AgentCommandDefine.RUN:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
                ProgressWaitOnCommand(GamePlayDefine.DashMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                ExcuteCommand(cmd);
                //ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.IDLE:
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
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
                case AgentCommandDefine.ATTACK_LIGHT:
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ExcuteCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex < mCurAnimStateEndMeter)
                return;

            mCurAnimStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }
}
