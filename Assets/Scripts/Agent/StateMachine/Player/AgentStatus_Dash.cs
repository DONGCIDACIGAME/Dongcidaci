using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Dash : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.DASH;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Dash(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        StartAnimQueue();

        float meterTime = MeterManager.Ins.GetCurrentMeterTime();
        mAgent.MoveControl.Dash(mAgent.GetDashDistance(), GamePlayDefine.DashMeterTime * meterTime);
    }

    public override void OnExit()
    {
        base.OnExit();

        ResetAnimQueue();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                ProgressWaitOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd);
                break;
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
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
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.IDLE:
                    ExcuteCommand(cmdType, towards);
                    return;
                case AgentCommandDefine.DASH:
                    mAgent.MoveControl.TurnTo(towards);
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            AnimQueueMoveOn();
        }
    }
}
