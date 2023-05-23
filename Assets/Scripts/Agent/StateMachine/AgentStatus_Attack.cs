using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
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

        byte triggerCmd = (byte)context["triggerCmd"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        mAgent.MoveControl.TurnTo(towards);

        if (context.TryGetValue("comboAction", out object obj))
        {
            TriggeredComboAction triggeredComboAction = obj as TriggeredComboAction;
            if(triggeredComboAction != null)
            {
                ConditionalExcuteCombo(triggerCmd, towards, triggerMeter, triggeredComboAction);
            }
        }
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
    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            // 接收到受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                Log.Error(LogLevel.Info, "如果攻击没有配combo，就会执行到这里");
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Combo指令直接处理逻辑
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="triggeredComboAction"></param>
    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmd, triggeredComboAction);

        // 如果是攻击指令，就根据节拍进度执行combo
        if (AgentCommandDefine.GetChangeToStatus(cmd.CmdType) == GetStatusName())
        {
            ConditionalExcuteCombo(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
        }
        else// 否则都要等本次攻击结束后执行，先放入指令缓存区
        {
            PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, cmd.TriggerMeter,  triggeredComboAction);
        }
    }

    /// <summary>
    /// 节拍开始逻辑
    /// </summary>
    /// <param name="meterIndex"></param>
    protected override void CustomOnMeterEnter(int meterIndex)
    {
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex < mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            // 如果是攻击指令，就执行combo
            if (AgentCommandDefine.GetChangeToStatus(cmdType) == GetStatusName())
            {
                mAgent.MoveControl.TurnTo(towards);
                ExcuteCombo(mCurTriggeredComboAction);
            }
            else// 否则切换到其他状态执行指令和combo
            {
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, mCurTriggeredComboAction);
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
        if (MeterManager.Ins.MeterIndex >= mCurLogicStateEndMeter)
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

            // 超过输入的容差时间，且当前指令缓存区里没有指令（没有待执行指令）
            if (!inInputTime && !cmdBuffer.HasCommand())
            {
                ChangeToIdle();
            }
        }
        //Log.Logic(LogLevel.Info, "cur anim state:{0}, progress:{1}", mAgent.AnimPlayer.CurStateName, mAgent.AnimPlayer.CurStateProgress);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }
}
