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

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (MeterManager.Ins.MeterIndex > mCurLogicStateEndMeter)
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset);

            // 超过输入的容差时间，进入idle
            if (!inInputTime && !cmdBuffer.HasCommand())
            {
                ChangeStatusOnCommand(AgentCommandDefine.IDLE, GamePlayDefine.InputDirection_NONE, MeterManager.Ins.MeterIndex, null);
            }
        }
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);

        switch (cmdType)
        {
            // 接收到受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            // 根据节拍进度冲刺
            case AgentCommandDefine.DASH:
                ConditionalExcute(cmdType, towards, triggerMeter, triggeredComboStep); 
                break;
            // 其他指令类型，都要等本次冲刺结束后执行，先放入指令缓存区
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

        // 缓存区取指令
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.DASH:
                    if (mCurTriggeredComboStep != null)
                    {
                        ExcuteCombo(cmdType, towards, triggerMeter, ref mCurTriggeredComboStep);
                    }
                    else
                    {
                        StatusDefaultAction(cmdType, towards, triggerMeter, statusDefaultActionData);
                    }
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

    /// <summary>
    /// dash的默认逻辑
    /// 1. 播放冲刺动画
    /// 3. 向当前方向冲刺一段距离
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        // 1. 播放冲刺动画
        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(agentActionData.statusName, agentActionData.stateName);
       
        // 2. 向当前方向冲刺一段距离
        Dash();
    }
}
