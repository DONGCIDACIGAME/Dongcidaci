using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 攻击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    /// <summary>
    /// 初始化
    /// </summary>
    public override void CustomInitialize()
    {
        base.CustomDispose();
    }

    /// <summary>
    /// 释放
    /// </summary>
    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="context"></param>
    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        ConditionalExcute(cmdType, towards, triggerMeter, args, triggeredComboStep);
    }

    /// <summary>
    /// 状态退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }


    /// <summary>
    /// 常规指令直接处理逻辑
    /// </summary>
    /// <param name="cmd"></param>
    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);

        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ConditionalExcute(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
                break;
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
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex <= mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        // 缓存区取指令
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.BE_HIT_BREAK:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.IDLE:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                    if(mCurTriggeredComboStep != null)
                    {
                        ExcuteCombo(cmdType, towards, triggerMeter, args, ref mCurTriggeredComboStep);
                    }
                    else
                    {
                        StatusDefaultAction(cmdType, towards, triggerMeter, args, statusDefaultActionData);
                    }
                    break;
                case AgentCommandDefine.EMPTY:
                case AgentCommandDefine.BE_HIT:
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

        // 在逻辑结束拍之后
        if (MeterManager.Ins.MeterIndex > mCurLogicStateEndMeter)
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

            // 超过输入的容差时间，且当前指令缓存区里没有指令（没有待执行指令）
            if (!inInputTime && !cmdBuffer.HasCommand())
            {
                ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
            }
        }
        //Log.Logic(LogLevel.Info, "cur anim state:{0}, progress:{1}", mAgent.AnimPlayer.CurStateName, mAgent.AnimPlayer.CurStateProgress);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
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
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        // 1. 转向被攻击的方向
        mAgent.MoveControl.TurnTo(towards);

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放攻击动画
        mCurLogicStateEndMeter = mCrossfadeByMeterAnimDriver.PlayAnimStateWithCut(statusName, stateName);

        // 3. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, mAgent.GetTowards(), 0);

    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}