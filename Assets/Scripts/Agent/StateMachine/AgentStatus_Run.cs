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

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        if(context == null)
        {
            Log.Error(LogLevel.Critical, "AgentStatus_Run OnEnter Error, 传入的参数为空，请检查!");
        }

        byte cmdType = (byte)context["cmdType"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

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
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(towards);
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if(cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.RUN:
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

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }

    /// <summary>
    /// Run的默认逻辑
    /// 1. 转向移动的方向
    /// 2. 播放动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        // 等待当前逻辑结束拍
        if (triggerMeter <= mCurLogicStateEndMeter)
            return;

        if (agentActionData == null)
        {
            // 1. 步进式动画继续
            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();

            // 2. 转向移动放方向
            mAgent.MoveControl.TurnTo(towards);

            return;
        }

        // 1. 播放动画
        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(agentActionData.stateName, agentActionData.stateName);

        // 2. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);
    }

}
