using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_Idle : MonsterStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
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

        StatusDefaultAction();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT_BREAK:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                Log.Error(LogLevel.Normal, "Invalid Cmd, get charging attack on {0}!", GetStatusName());
                break;
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.CHARGING:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.BE_HIT_BREAK:
                    ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, comboStep);
                    break;
                case AgentCommandDefine.IDLE:
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
    /// Idle状态的默认逻辑
    /// 播放动作
    /// </summary>
    public void StatusDefaultAction()
    {
        // 1. 步进式动画继续
        mStepLoopAnimDriver.StartPlay(GetStatusName());
    }
}
