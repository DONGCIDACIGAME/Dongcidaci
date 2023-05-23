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

    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ChangeStatusOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.DASH:
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

        PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }
}
