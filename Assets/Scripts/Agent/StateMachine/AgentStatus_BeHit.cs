using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_BeHit : AgentStatus
{
    /// <summary>
    /// 自定义动画驱动器
    /// </summary>
    protected CustomAnimDriver mCustomAnimDriver;

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mCustomAnimDriver = new CustomAnimDriver(mAgent, GetStatusName());
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        if (mCustomAnimDriver != null)
        {
            mCustomAnimDriver.Dispose();
            mCustomAnimDriver = null;
        }
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
                ChangeStatusOnNormalCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, null);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggerableCombo combo)
    {
        base.CustomOnComboCommand(cmd, combo);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.DASH:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, combo);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnComboCommand(cmd, combo);
                break;
            default:
                break;
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {

    }
}
