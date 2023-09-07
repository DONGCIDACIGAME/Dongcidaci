using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_Behit : MonsterStatus
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

        StatusDefaultAction(towards, args);
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
                StatusDefaultAction(towards, args);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                Log.Error(LogLevel.Normal, "Invalid Cmd, get charging attack on {0}!", GetStatusName());
                break;
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    /// <summary>
    /// 受击状态默认逻辑
    /// 1. 播放受击动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public void StatusDefaultAction(Vector3 towards, Dictionary<string, object> args)
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
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetEntityId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, triggeredComboStep);
            return;
        }

        mTimer += deltaTime;
    }
}
