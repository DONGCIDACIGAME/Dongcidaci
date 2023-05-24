using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Run : AgentStatus
{

    public override string GetStatusName()
    {
        return AgentStatusDefine.RUN;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mInputHandle = new KeyboardInputHandle_Run(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        if(context == null)
        {
            Log.Error(LogLevel.Critical, "AgentStatus_Run OnEnter Error, 传入的参数为空，请检查!");
        }

        if(context.TryGetValue("towards", out object arg))
        {
            Vector3 towards = (Vector3)arg;
            mAgent.MoveControl.TurnTo(towards);
        }

        mAgent.ComboTrigger.ResetAllCombo();
        mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
    }

    public override void OnExit()
    {
        base.OnExit();
        mStepLoopAnimDriver.Reset();
    }

    protected override void CustomOnNormalCommand(byte cmdType, Vector3 towards, int triggerMeter)
    {
        base.CustomOnNormalCommand(cmdType, towards, triggerMeter);

        switch (cmdType)
        {
            case AgentCommandDefine.BE_HIT:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, null);
                break;
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(towards);
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

        // 按照目前的设计，run是不会触发combo的，所以执行到这里，肯定是其他指令类型，立即响应，切换到其他状态去处理
        ChangeStatusOnCommand(cmdType, towards, triggerMeter, triggeredComboAction);
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if(cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter))
        {
            Log.Logic(LogLevel.Info, "PeekCommand--{0}, meterIndex:{1}, towards:{2}", cmdType, meterIndex, towards);
            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.BE_HIT:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, mCurTriggeredComboAction);
                    return;
                case AgentCommandDefine.RUN:
                    mAgent.MoveControl.TurnTo(towards);
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }

            if (meterIndex <= mCurLogicStateEndMeter)
                return;

            mCurLogicStateEndMeter = mStepLoopAnimDriver.MoveNext();
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mAgent.MoveControl.Move(deltaTime);
    }

    public override void StatusDefaultAction()
    {
        
    }
}
