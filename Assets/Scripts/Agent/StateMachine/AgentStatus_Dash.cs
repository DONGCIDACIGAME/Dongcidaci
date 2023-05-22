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

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    private void Dash()
    {
        float meterTime = MeterManager.Ins.GetCurrentMeterTime();
        mAgent.MoveControl.Dash(mAgent.GetDashDistance(), GamePlayDefine.DashMeterTime * meterTime);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        Dash();
        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(AgentAnimDefine.DefaultAnimName_Dash);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (MeterManager.Ins.MeterIndex >= mCurLogicStateEndMeter)
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

            // 超过输入的容差时间，进入idle
            if (!inInputTime && !cmdBuffer.HasCommand())
            {
                ChangeToIdle();
            }
        }
    }

    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnNormalCommand(cmd);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ConditionalChangeStatusOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd, null);
                break;
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmd, triggeredComboAction);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ConditionalChangeStatusOnCommand(GamePlayDefine.AttackMeterProgressWait, cmd, triggeredComboAction);
                break;
            case AgentCommandDefine.DASH:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, triggeredComboAction);
                break;
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (meterIndex < mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            mAgent.MoveControl.TurnTo(towards);

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ChangeStatusOnNormalCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.ATTACK_SHORT:
                    ChangeStatusOnComboCommand(cmdType, towards, meterIndex, mCurTriggeredComboAction);
                    mCurTriggeredComboAction = null;
                    break;
                case AgentCommandDefine.ATTACK_LONG:
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }
}
