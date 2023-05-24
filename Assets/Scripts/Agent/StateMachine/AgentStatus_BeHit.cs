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

        mAgent.ComboTrigger.ResetAllCombo();
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnNormalCommand(byte cmdType, Vector3 towards, int triggerMeter)
    {
        base.CustomOnNormalCommand(cmdType, towards, triggerMeter);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, null);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmdType, towards, triggerMeter, triggeredComboAction);

        // 按照目前的设计，be_hit是不会触发combo的，所以执行到这里，肯定是其他指令类型，都放入缓存区等待节拍
        PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboAction);
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    public override void StatusDefaultAction()
    {
        
    }
}
