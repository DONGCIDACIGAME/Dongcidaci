using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Idle : AgentStatus
{
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

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        // 进入idle状态会打断combo，即combo要从头开始触发
        mAgent.ComboTrigger.ResetAllCombo();
        StatusDefaultAction();
    }

    public override void OnExit()
    {
        base.OnExit();
        mStepLoopAnimDriver.Reset();
    }

    protected override void CustomOnNormalCommand(byte cmdType, Vector3 towards, int triggerMeter)
    {
        base.CustomOnNormalCommand(cmdType, towards, triggerMeter);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, null);
                break;
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmdType, towards, triggerMeter, triggeredComboAction);

        // 按照目前的设计，idle是不会触发combo的，所以执行到这里，肯定是其他指令类型，立即响应，切换到其他状态去处理
        ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboAction);
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboAction);
                    return;
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex <= mCurLogicStateEndMeter)
                return;

            StatusDefaultAction();
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    public override void StatusDefaultAction()
    {
        mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
    }
}
