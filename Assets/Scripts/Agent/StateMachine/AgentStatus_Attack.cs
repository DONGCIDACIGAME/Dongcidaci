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

        if(context.TryGetValue("combo", out object obj))
        {
            TriggeredComboAction triggeredComboAction = obj as TriggeredComboAction;
            if(triggeredComboAction != null)
            {
                ProgressWaitOnComboCommand(triggerCmd, towards, triggerMeter, triggeredComboAction);
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
    /// 根据节拍进度处理Combo指令
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboAction"></param>
    private void ProgressWaitOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;
        if (progress >= GamePlayDefine.AttackMeterProgressWait)
        {
            ExcuteCombo(triggeredComboAction);
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, triggeredComboAction);
        }
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
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnNormalCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, null);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                Log.Error(LogLevel.Normal, "CustomOnNormalCommand Error, all attack input must trigger combo!");
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
        if(!AgentCommandDefine.IsComboTrigger(cmd.CmdType))
        {
            Log.Error(LogLevel.Normal, "CustomOnComboCommand Error,[{0}] is  not combo trigger command type!", cmd.CmdType);
            return;
        }

        mAgent.MoveControl.TurnTo(cmd.Towards);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.DASH:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, triggeredComboAction);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ProgressWaitOnComboCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
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
        if (meterIndex < mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            mAgent.MoveControl.TurnTo(towards);

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ChangeStatusOnNormalCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.ATTACK_SHORT:
                    if(mCurTriggeredComboAction != null)
                    {
                        ExcuteCombo(mCurTriggeredComboAction);
                        mCurTriggeredComboAction = null;
                    }
                    break;
                case AgentCommandDefine.ATTACK_LONG:
                    break;
                case AgentCommandDefine.EMPTY:
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
