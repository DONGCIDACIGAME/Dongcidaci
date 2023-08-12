using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Dash : HeroStatus
{
    private float mExitTime;
    private float mTimer;
    private Vector3 mAttackTowards;
    private ComboStep mComboStep;

    public override string GetStatusName()
    {
        return AgentStatusDefine.DASH;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        //ConditionalExcute(cmdType, towards, triggerMeter, args, triggeredComboStep);
        ExcuteComboTriggerCmd(cmdType, towards, triggerMeter, args, triggeredComboStep);
        mComboStep = triggeredComboStep.comboStep;
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        //if (MeterManager.Ins.MeterIndex > mCurLogicStateEndMeter)
        //{
        //    // 是否在输入的容差时间内
        //    bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

        //    // 超过输入的容差时间，进入idle
        //    if (!inInputTime && !cmdBuffer.HasCommand())
        //    {
        //        ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        //    }
        //}

        mTimer += deltaTime;

        if (mTimer >= mExitTime)
        {
            Dictionary<string, object> _args = new Dictionary<string, object>();
            _args.Add("transitionAction", mComboStep.transitionData);
            TriggeredComboStep _triggeredComboStep = null;
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.TRANSITION, AgentCommandDefine.EMPTY, mAttackTowards, MeterManager.Ins.MeterIndex, _args, _triggeredComboStep);
        }
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        //switch (cmdType)
        //{
        //    // 接收到受击指令，马上切换到受击状态
        //    case AgentCommandDefine.BE_HIT_BREAK:
        //        ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
        //        break;
        //    // 根据节拍进度冲刺
        //    case AgentCommandDefine.DASH:
        //        ConditionalExcute(cmdType, towards, triggerMeter, args, triggeredComboStep);
        //        break;
        //    // 其他指令类型，都要等本次冲刺结束后执行，先放入指令缓存区
        //    case AgentCommandDefine.RUN:
        //    case AgentCommandDefine.RUN_METER:
        //    case AgentCommandDefine.IDLE:
        //    case AgentCommandDefine.ATTACK_LONG:
        //    case AgentCommandDefine.ATTACK_SHORT:
        //    case AgentCommandDefine.ATTACK_LONG_INSTANT:
        //    case AgentCommandDefine.ATTACK_SHORT_INSTANT:
        //        PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
        //        break;
        //    case AgentCommandDefine.BE_HIT:// 冲刺状态下，非打断的受击指令不处理
        //    case AgentCommandDefine.EMPTY:
        //    default:
        //        break;
        //}

        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.ACCUMULATING_ATTACK_START:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        //// 逻辑拍结束前，不能响应缓存区指令
        //if (meterIndex <= mCurLogicStateEndMeter)
        //{
        //    Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
        //    return;
        //}

        //// 缓存区取指令
        //if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        //{
        //    Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
        //    cmdBuffer.ClearCommandBuffer();

        //    switch (cmdType)
        //    {
        //        case AgentCommandDefine.BE_HIT_BREAK:
        //        case AgentCommandDefine.RUN:
        //        case AgentCommandDefine.IDLE:
        //        case AgentCommandDefine.ATTACK_SHORT:
        //        case AgentCommandDefine.ATTACK_LONG:
        //        case AgentCommandDefine.ATTACK_LONG_INSTANT:
        //        case AgentCommandDefine.ATTACK_SHORT_INSTANT:
        //            ChangeStatusOnCommand(cmdType, towards, meterIndex, args, comboStep);
        //            break;
        //        case AgentCommandDefine.DASH:
        //            ExcuteComboTriggerCmd(cmdType, towards, meterIndex, args, comboStep);
        //            break;
        //        case AgentCommandDefine.BE_HIT:// 冲刺状态下，非打断的受击指令不处理
        //        case AgentCommandDefine.EMPTY:
        //        default:
        //            break;
        //    }
        //}
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    /// <summary>
    /// dash的默认逻辑
    /// 1. 播放冲刺动画
    /// 3. 向当前方向冲刺一段距离
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        // 1. 转向
        mAgent.MoveControl.TurnTo(towards);

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放冲刺动画
        //mCurLogicStateEndMeter = mMatchMeterCrossfadeAnimDriver.CrossFadeToState(statusName, stateName);
        mDefaultCrossFadeAnimDriver.CrossFadeToState(statusName, stateName);

        if (args != null 
            && args.TryGetValue("distance", out object distanceObj) 
            && args.TryGetValue("startTime",out object startTimeObj)
            && args.TryGetValue("endTime", out object endTimeObj))
        {
            float dashDistance = (float)distanceObj;
            float startTime = (float)startTimeObj;
            float endTime = (float)endTimeObj;
            mExitTime = endTime - startTime;
            // 3. 处理动画相关的位移
            mAgent.MovementExcutorCtl.Start(startTime, endTime, DirectionDef.RealTowards, DirectionDef.none, dashDistance);
        }
        else
        {
            // 3. 处理动画相关的位移
            mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);
            AgentAnimStateInfo animStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
            // 攻击动作的时长
            mExitTime = animStateInfo.animLen;
        }

        mTimer = 0;
        mAttackTowards = towards;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_Dash(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
