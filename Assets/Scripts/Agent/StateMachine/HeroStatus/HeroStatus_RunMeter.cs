using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个run指令移动一拍
/// </summary>
public class HeroStatus_RunMeter : HeroStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN_METER;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_RunMeter(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        StatusDefaultAction(cmdType, towards, triggerMeter, args, statusDefaultActionData);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.BE_HIT_BREAK:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(towards);
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.BE_HIT_BREAK:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.RUN:
                    StatusDefaultAction(cmdType, towards, triggerMeter, args, null);
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
        // 等待当前逻辑结束拍
        if (triggerMeter <= mCurLogicStateEndMeter)
            return;

        int nextMeter = MeterManager.Ins.GetMeterIndex(triggerMeter, 1);
        float time = MeterManager.Ins.GetTimeToMeter(nextMeter);
        Vector3 targetPos = mAgent.GetPosition() + towards * time * mAgent.GetSpeed();


        if (agentActionData == null)
        {
            // 1. 转向移动放方向
            mAgent.MoveControl.TurnTo(towards);

            // 2. 步进式动画继续
            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();

            mAgent.MoveControl.MoveToPosition(targetPos);
            return;
        }

        // 1. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 播放动画
        mCurLogicStateEndMeter = mMatchMeterCrossfadeAnimDriver.CrossFadeToState(agentActionData.stateName, agentActionData.stateName);

        mAgent.MoveControl.MoveToPosition(targetPos);
    }
}
