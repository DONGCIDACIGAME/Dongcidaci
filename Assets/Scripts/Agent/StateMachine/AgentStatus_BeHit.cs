using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_BeHit : AgentStatus
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

    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        AgentActionData actionData = statusDefaultActionData;
        if (args != null && args.TryGetValue("beHitAction", out object obj1))
        {
            actionData = obj1 as AgentActionData;
        }
        StatusDefaultAction(cmdType, towards, triggerMeter, args, actionData) ;
    }

    public override void OnExit()
    {
        base.OnExit();

        mTimer = 0;
        mExitTime = 0;
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.BE_HIT_BREAK:
                AgentActionData actionData = statusDefaultActionData;
                if (args != null && args.TryGetValue("beHitAction", out object obj1))
                {
                    actionData = obj1 as AgentActionData;
                }
                StatusDefaultAction(cmdType, towards, triggerMeter, args, actionData);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
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

    /// <summary>
    /// 受击状态默认逻辑
    /// 1. 播放受击动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        float moveMore = 0;
        if (args != null && args.TryGetValue("moveMove", out object obj2))
        {
            moveMore = (float)obj2;
        }

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 1. 播放受击动画
        AgentAnimStateInfo animStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent,statusName, stateName);
        if (animStateInfo != null)
        {
            mTimer = 0;
            mDefaultCrossAnimDriver.CrossFadeToState(statusName, stateName);
            mExitTime = animStateInfo.animLen;
        }
        // 2. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.FixedTowards, towards, moveMore);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(mTimer >= mExitTime)
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
        mInputHandle = new KeyboardInputHandle_BeHit(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}
