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

        byte cmdType = (byte)context["cmdType"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        StatusDefaultAction(cmdType, towards, triggerMeter, null);
    }

    public override void OnExit()
    {
        base.OnExit();
        mStepLoopAnimDriver.Reset();
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
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
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboStep);
                    return;
                case AgentCommandDefine.IDLE:
                    StatusDefaultAction(cmdType, towards, meterIndex, mCurTriggeredComboStep.comboStep.agentActionData);
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex <= mCurLogicStateEndMeter)
                return;

            StatusDefaultAction(cmdType, towards, triggerMeter, mCurTriggeredComboStep.comboStep.agentActionData);
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    /// <summary>
    /// Idle状态的默认逻辑
    /// 播放动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboStep"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        if(!string.IsNullOrEmpty(agentActionData.statusName) && !string.IsNullOrEmpty(agentActionData.stateName))
        {
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(agentActionData.stateName,agentActionData.stateName);
        }
        else
        {
            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }
}
