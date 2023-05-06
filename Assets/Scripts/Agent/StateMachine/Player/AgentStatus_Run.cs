using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Run : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Run(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        if(context == null)
        {
            Log.Error(LogLevel.Critical, "AgentStatus_Run OnEnter Error, 传入的参数为空，请检查!");
        }

        if(context.TryGetValue("towards", out object arg))
        {
            Vector3 towards = (Vector3)arg;
            mAgent.MoveControl.TurnTo(towards);
        }

        mCurAnimStateEndMeter = mStepLoopAnimDriver.MoveNext();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.IDLE:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
                ProgressWaitOnCommand(GamePlayDefine.DashMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(cmd.Towards);
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if(cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.ATTACK_LIGHT:
                case AgentCommandDefine.ATTACK_HARD:
                    ExcuteCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.RUN:
                    mAgent.MoveControl.TurnTo(towards);
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex < mCurAnimStateEndMeter)
                return;

            mCurAnimStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }


    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }
}
