using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个run指令移动一拍
/// </summary>
public class HeroStatus_RunMeter : HeroStatus
{
    private float mExitTime;
    private float mTimer;

    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN_METER;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_RunMeter(mAgent as Hero);
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
        mExitTime = 0;
        mTimer = 0;
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
            case AgentCommandDefine.ATTACK_LONG_INSTANT:
            case AgentCommandDefine.ATTACK_SHORT_INSTANT:
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.RUN_METER:
                StatusDefaultAction(cmdType, towards, triggerMeter, args, statusDefaultActionData);
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
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.ATTACK_LONG_INSTANT:
                case AgentCommandDefine.ATTACK_SHORT_INSTANT:
                case AgentCommandDefine.BE_HIT_BREAK:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.RUN_METER:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }
        }
        else
        {
            ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        mTimer += deltaTime;
        if(mTimer >= mExitTime)
        {
            ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        }
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
        float distance = time * mAgent.GetSpeed();


        // 1. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 步进式动画继续
        mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();

        mAgent.MoveControl.MoveTowards(towards, distance * 0.5f, time * 0.5f);

        mExitTime = time * 0.5f;
        mTimer = 0;
    }
}
