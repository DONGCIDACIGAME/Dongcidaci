using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_InstantAttack : HeroStatus
{
    private float mExitTime;
    private float mTimer;
    private Vector3 mAttackTowards;

    private ComboStep mComboStep;
    public override string GetStatusName()
    {
        return AgentStatusDefine.INSTANT_ATTACK;
    }

    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="context"></param>
    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        StatusDefaultAction(towards, triggerMeter, triggeredComboStep);
    }

    /// <summary>
    /// 状态退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();

        mTimer = 0;
        mExitTime = 0;
        mAttackTowards = DirectionDef.none;
    }


    /// <summary>
    /// 常规指令直接处理逻辑
    /// </summary>
    /// <param name="cmd"></param>
    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        // 只响应打断型受击状态
        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
            case AgentCommandDefine.DASH:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                Log.Error(LogLevel.Normal, "Invalid Cmd, get charging attack on {0}!", GetStatusName());
                break;
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }


    /// <summary>
    /// 节拍开始逻辑
    /// </summary>
    /// <param name="meterIndex"></param>
    protected override void CustomOnMeterEnter(int meterIndex)
    {
        //// 逻辑拍结束前，不能响应缓存区指令
        //if (meterIndex <= mCurLogicStateEndMeter)
        //{
        //    //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
        //    return;
        //}

        //// 缓存区取指令
        //if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        //{
        //    cmdBuffer.ClearCommandBuffer();
        //    Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

        //    switch (cmdType)
        //    {
        //        case AgentCommandDefine.BE_HIT_BREAK:
        //        case AgentCommandDefine.DASH:
        //        case AgentCommandDefine.INSTANT_ATTACK:
        //        case AgentCommandDefine.METER_ATTACK:
        //        case AgentCommandDefine.CHARGING:
        //        case AgentCommandDefine.CHARGING_ATTACK:
        //            //ExcuteComboTriggerCmd(cmdType, towards, triggerMeter, args, comboStep);
        //            break;
        //        case AgentCommandDefine.IDLE:
        //        case AgentCommandDefine.RUN:
        //        case AgentCommandDefine.EMPTY:
        //        case AgentCommandDefine.BE_HIT:
        //        default:
        //            break;
        //    }
        //}
        //else
        //{
        //    ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        //}

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        mTimer += deltaTime;

        if (mTimer >= mExitTime)
        {
            // 缓存区取指令
            if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
            {
                cmdBuffer.ClearCommandBuffer();
                switch (cmdType)
                {
                    case AgentCommandDefine.BE_HIT_BREAK:
                    case AgentCommandDefine.DASH:
                    case AgentCommandDefine.INSTANT_ATTACK:
                    case AgentCommandDefine.CHARGING:
                        StatusDefaultAction(towards, triggerMeter, comboStep);
                        return;
                    case AgentCommandDefine.RUN:
                    case AgentCommandDefine.IDLE:
                    case AgentCommandDefine.EMPTY:
                    case AgentCommandDefine.BE_HIT:
                    default:
                        break;
                }
            }
            //else
            //{
            //    ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
            //}

            Dictionary<string, object> _args = new Dictionary<string, object>();
            _args.Add("transitionAction", mComboStep.transitionData);
            TriggeredComboStep _triggeredComboStep = null;
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetEntityId(), AgentStatusDefine.ATTACK_TRANSITION, AgentCommandDefine.EMPTY, mAttackTowards, MeterManager.Ins.MeterIndex, _args, _triggeredComboStep);
        }
    }

    /// <summary>
    /// attack的默认逻辑
    /// 1. 转向攻击方向
    /// 2. 播放攻击动画
    /// 3. 造成伤害
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public void StatusDefaultAction(Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        // 1. 转向
        mAgent.MoveControl.TurnTo(towards);

        AgentActionData agentActionData = GetStatusDefaultActionData();
        if (triggeredComboStep != null)
        {
            agentActionData = triggeredComboStep.comboStep.attackActionData;
            ExcuteCombo(triggerMeter, 0, triggeredComboStep);
            mComboStep = triggeredComboStep.comboStep;
        }

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放攻击动画
        mExitTime = mDefaultCrossFadeAnimDriver.StartPlay(statusName, stateName);

        // 3. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);

        // 攻击动作的时长
        mTimer = 0;
        mAttackTowards = towards;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_InstantAttack(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
