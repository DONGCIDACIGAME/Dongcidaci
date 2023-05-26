using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_BeHit : AgentStatus
{
    public override void CustomInitialize()
    {
        base.CustomInitialize();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.BE_HIT;
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        byte cmdType = (byte)context["cmdType"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        StatusDefaultAction(cmdType, towards, triggerMeter, null);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboStep);
                break;
            case AgentCommandDefine.DASH:
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

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    /// <summary>
    /// 受击状态默认逻辑
    /// 1. 播放受击动作
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        
    }
}
