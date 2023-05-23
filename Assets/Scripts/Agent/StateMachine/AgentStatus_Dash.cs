using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Dash : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.DASH;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Dash(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    private void Dash()
    {
        float meterTime = MeterManager.Ins.GetCurrentMeterTime();
        mAgent.MoveControl.Dash(mAgent.GetDashDistance(), GamePlayDefine.DashMeterTime * meterTime);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        byte triggerCmd = (byte)context["triggerCmd"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        if (context.TryGetValue("comboAction", out object obj))
        {
            TriggeredComboAction triggeredComboAction = obj as TriggeredComboAction;
            if (triggeredComboAction != null)
            {
                bool excute = ConditionalExcuteCombo(triggerCmd, towards, triggerMeter, triggeredComboAction);

                if(excute)
                {
                    Dash();
                }
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (MeterManager.Ins.MeterIndex >= mCurLogicStateEndMeter)
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

            // 超过输入的容差时间，进入idle
            if (!inInputTime && !cmdBuffer.HasCommand())
            {
                ChangeToIdle();
            }
        }
    }

    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            // 接收到受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.DASH:
                Log.Error(LogLevel.Info, "如果冲刺没有配combo，就会执行到这里");
                break;
            // 其他指令类型，都要等本次冲刺结束后执行，先放入指令缓存区
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmd, triggeredComboAction);

        // 如果是冲刺指令，就根据节拍进度执行combo
        if (AgentCommandDefine.GetChangeToStatus(cmd.CmdType) == GetStatusName())
        {
            ConditionalExcuteCombo(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
        }
        else // 否则都要等本次冲刺结束后执行，先放入指令缓存区
        {
            PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex < mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        // 缓存区取指令
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            mAgent.MoveControl.TurnTo(towards);

            // 如果是冲刺指令，就执行combo
            if (AgentCommandDefine.GetChangeToStatus(cmdType) == GetStatusName())
            {
                mAgent.MoveControl.TurnTo(towards);
                Dash();
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
}
