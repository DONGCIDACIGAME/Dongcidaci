using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_Run : MonsterStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
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
        StatusDefaultAction(towards);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.BE_HIT_BREAK:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                Log.Error(LogLevel.Normal, "Invalid Cmd, get charging attack on {0}!", GetStatusName());
                break;
            case AgentCommandDefine.RUN:
                StatusDefaultAction(towards);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.CHARGING:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.BE_HIT_BREAK:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, comboStep);
                    break;
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }
        }
        else
        {
            ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, meterIndex, null, null);
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }

    /// <summary>
    /// Run的默认逻辑
    /// 1. 转向移动的方向
    /// 2. 播放动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public void StatusDefaultAction(Vector3 towards)
    {
        // 1. 转向移动放方向
        mAgent.MoveControl.TurnTo(towards);

        // 2. 步进式动画继续
        mStepLoopAnimDriver.StartPlay(GetStatusName());
    }
}
