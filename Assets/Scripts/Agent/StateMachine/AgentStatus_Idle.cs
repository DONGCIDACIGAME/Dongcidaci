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
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> context)
    {
        base.OnEnter(cmdType, towards, triggerMeter, context);

        //byte cmdType = (byte)context["cmdType"];
        //Vector3 towards = (Vector3)context["towards"];
        //int triggerMeter = (int)context["triggerMeter"];

        StatusDefaultAction(cmdType, towards, triggerMeter, statusDefaultActionData);
    }

    public override void OnExit()
    {
        base.OnExit();
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
                    break ;
                case AgentCommandDefine.IDLE:
                    StatusDefaultAction(cmdType, towards, triggerMeter, null);
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
        // 等待当前逻辑结束拍
        if (triggerMeter <= mCurLogicStateEndMeter)
            return;

        if (agentActionData == null)
        {
            // 1. 转向移动放方向
            mAgent.MoveControl.TurnTo(towards);

            // 2. 步进式动画继续
            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
            return;
        }

        // 1. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 播放动画
        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(agentActionData.stateName, agentActionData.stateName);
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}
