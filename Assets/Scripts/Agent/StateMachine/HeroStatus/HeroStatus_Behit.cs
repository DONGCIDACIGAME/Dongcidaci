using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Behit : HeroStatus
{
    private float mExitTime;
    private float mTimer;

    public override void CustomInitialize()
    {
        base.CustomInitialize();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.BEHIT;
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        StatusDefaultAction(args, towards);
    }

    public override void OnExit()
    {
        base.OnExit();

        mTimer = 0;
        mExitTime = 0;
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT_BREAK:
                StatusDefaultAction(args, towards);
                break;
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
            case AgentCommandDefine.CHARGING_ATTACK:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        //// 逻辑拍结束前，不能响应缓存区指令
        //Log.Error(LogLevel.Normal, "CustomOnMeterEnter--{0}", meterIndex);
        //if (meterIndex <= mCurLogicStateEndMeter)
        //{
        //    Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
        //    return;
        //}

        //Dictionary<string, object> args = null;
        //TriggeredComboStep triggeredComboStep = null;
        //GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, triggeredComboStep);
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        Log.Error(LogLevel.Normal, "CustomOnMeterEnd--{0}", meterIndex);
    }

    public void StatusDefaultAction(Dictionary<string, object> args, Vector3 towards)
    {
        AgentActionData actionData = GetStatusDefaultActionData();
        if (args != null && args.TryGetValue("beHitAction", out object obj1))
        {
            actionData = obj1 as AgentActionData;
        }

        float moveMore = 0;
        if (args != null && args.TryGetValue("moveMore", out object obj2))
        {
            moveMore = (float)obj2;
        }

        string statusName = actionData.statusName;
        string stateName = actionData.stateName;

        // 1. 播放受击动画
        mExitTime = mDefaultCrossFadeAnimDriver.StartPlay(statusName, stateName);
        mTimer = 0;
        // 2. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.FixedTowards, towards, moveMore);
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        if (mTimer >= mExitTime)
        {
            Dictionary<string, object> args = null;
            TriggeredComboStep triggeredComboStep = null;
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, triggeredComboStep);
            return;
        }

        mTimer += deltaTime;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_BeHit(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
