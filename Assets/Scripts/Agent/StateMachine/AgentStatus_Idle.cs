using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Idle : AgentStatus
{
    /// <summary>
    /// 步进式动画驱动器
    /// </summary>
    protected StepLoopAnimDriver mStepLoopAnimDriver;

    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Idle(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        mStepLoopAnimDriver = new StepLoopAnimDriver(mAgent, GetStatusName());
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        if(mStepLoopAnimDriver != null)
        {
            mStepLoopAnimDriver.Dispose();
            mStepLoopAnimDriver = null;
        }    
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
                ChangeStatusOnNormalCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
                ProgressWaitOnCommand(GamePlayDefine.DashMeterProgressWait, cmd, null);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnNormalCommand(cmd);
                break;
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, null);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        base.CustomOnComboCommand(cmd, triggeredComboAction);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.DASH:
                ProgressWaitOnCommand(GamePlayDefine.DashMeterProgressWait, cmd, triggeredComboAction);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnComboCommand(cmd, triggeredComboAction);
                break;
            default:
                break;
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
            switch (cmdType)
            {
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.BE_HIT:
                    ChangeStatusOnNormalCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex != mCurLogicStateEndMeter)
                return;

            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }
}
