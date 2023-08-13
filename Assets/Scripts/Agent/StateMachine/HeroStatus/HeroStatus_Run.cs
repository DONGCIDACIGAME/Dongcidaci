using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Run : HeroStatus
{
    private bool mRunning;

    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);
        mRunning = false;
        StatusDefaultAction(cmdType, towards, triggerMeter, args, GetStatusDefaultActionData());
    }

    public override void OnExit()
    {
        base.OnExit();
        mRunning = false;
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.ACCUMULATING:
            case AgentCommandDefine.ACCUMULATING_ATTACK:
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.RUN:
                StatusDefaultAction(cmdType, towards, triggerMeter, args, GetStatusDefaultActionData());
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.METER_ATTACK:
                case AgentCommandDefine.ACCUMULATING:
                case AgentCommandDefine.ACCUMULATING_ATTACK:
                case AgentCommandDefine.BE_HIT_BREAK:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, comboStep);
                    break;
                case AgentCommandDefine.RUN:
                    StatusDefaultAction(cmdType, towards, triggerMeter, args, GetStatusDefaultActionData());
                    break;
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }
        }
        // 退出逻辑在update里
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        if(!mRunning)
        {
            ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
            return;
        }

        mAgent.MoveControl.Move(deltaTime);
        mRunning = false;
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
    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        // 1. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 步进式动画继续
        if(triggerMeter > mCurLogicStateEndMeter)
        {
            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }

        mRunning = true;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_Run(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
