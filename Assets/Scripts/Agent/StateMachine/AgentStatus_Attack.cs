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
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
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
    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        byte cmdType = (byte)context["cmdType"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        TriggeredComboStep triggeredComboStep = null;
        if (context.TryGetValue("comboStep", out object obj))
        {
            triggeredComboStep = obj as TriggeredComboStep;
        }
        ConditionalExcute(cmdType, towards, triggerMeter, triggeredComboStep);
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
    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);

        switch (cmdType)
        {
            // 接收到受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ConditionalExcute(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
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

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboStep);
                    return;
                case AgentCommandDefine.IDLE:
                    StatusDefaultAction(cmdType, towards, meterIndex, mCurTriggeredComboStep.comboStep.agentActionData);
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            // 如果是攻击指令，就执行combo
            if (AgentCommandDefine.GetChangeToStatus(cmdType) == GetStatusName())
            {
                ExcuteCombo(cmdType, towards, triggerMeter, ref mCurTriggeredComboStep);
            }
            else// 否则切换到其他状态执行指令和combo
            {
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, mCurTriggeredComboStep);
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
                ChangeStatusOnCommand(AgentCommandDefine.IDLE, GamePlayDefine.InputDirection_NONE, MeterManager.Ins.MeterIndex, null);
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
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        // 1. 转向攻击方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 播放攻击动画
        if (!string.IsNullOrEmpty(agentActionData.stateName) && !string.IsNullOrEmpty(agentActionData.stateName))
        {
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(agentActionData.statusName, agentActionData.stateName);
        }
        else
        {
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(statusDefaultActionData.statusName, statusDefaultActionData.stateName);
        }

        // 3. 造成伤害
        
    }
}