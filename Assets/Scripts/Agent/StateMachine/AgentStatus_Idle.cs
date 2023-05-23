using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);
        // 进入idle状态会打断combo，即combo要从头开始触发
        mAgent.ComboTrigger.ResetAllCombo();
        mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
    }

    public override void OnExit()
    {
        base.OnExit();
        mStepLoopAnimDriver.Reset();
    }

    protected override void CustomOnNormalCommand(AgentInputCommand cmd)
    {
        base.CustomOnNormalCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, null);
                break;
            case AgentCommandDefine.IDLE:
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

        ChangeStatusOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboAction);
                    return;
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex < mCurLogicStateEndMeter)
                return;

            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

}
