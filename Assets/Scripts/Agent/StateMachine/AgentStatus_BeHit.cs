using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_BeHit : AgentStatus
{
    private float mMoveMore;
    private Vector3 mMoveTowards;

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

    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> context)
    {
        base.OnEnter(cmdType, towards, triggerMeter, context);
        mMoveTowards = towards;
        //byte cmdType = (byte)context["cmdType"];
        //Vector3 towards = (Vector3)context["towards"];
        //int triggerMeter = (int)context["triggerMeter"];


        AgentActionData actionData = statusDefaultActionData;
        if (context.TryGetValue("beHitAction", out object obj1))
        {
            actionData = obj1 as AgentActionData;
        }

        if(context.TryGetValue("moveMove", out object obj2))
        {
            mMoveMore = (float)obj2;
        }

        StatusDefaultAction(cmdType, towards, triggerMeter, actionData);
    }

    public override void OnExit()
    {
        base.OnExit();
        mMoveMore = 0;
        mMoveTowards = DirectionDef.none;
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex <= mCurLogicStateEndMeter)
        {
            Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        Dictionary<string, object> args = new Dictionary<string, object>();
        //args.Add("cmdType", AgentCommandDefine.IDLE);
        //args.Add("towards", DirectionDef.none);
        //args.Add("triggerMeter", MeterManager.Ins.MeterIndex);
        GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args);
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
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;
        // 1. 播放攻击动画
        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(statusName, stateName);

        // 2. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, mMoveMore, mMoveTowards);
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_BeHit(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}
