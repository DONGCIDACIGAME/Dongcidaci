using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    /// <summary>
    /// 自定义动画驱动器
    /// </summary>
    protected CustomAnimDriver mCustomAnimDriver;

    /// <summary>
    /// 准备要切换到过度态
    /// </summary>
    private bool changeToTransferState;

    public override void CustomInitialize()
    {
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        mCustomAnimDriver = new CustomAnimDriver(mAgent, GetStatusName());
        changeToTransferState = false;
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        if(mCustomAnimDriver != null)
        {
            mCustomAnimDriver.Dispose();
            mCustomAnimDriver = null;
        }
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        byte triggerCmd = (byte)context["triggerCmd"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        if(context.TryGetValue("combo", out object obj))
        {
            TriggerableCombo combo = obj as TriggerableCombo;

            if(combo != null)
            {
                // 当前拍的剩余时间
                float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
                // 当前拍的总时间
                float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

                if (timeOfCurrentMeter == 0)
                {
                    ComboActionData actionData = combo.GetCurrentComboAction();
                    mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(actionData.stateName);
                    return;
                }

                ProgressWaitOnComboAttack(triggerCmd, towards, triggerMeter, combo);
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        changeToTransferState = false;
    }

    private void ProgressWaitOnNormalAttack(byte cmdType, Vector3 towards, int triggerMeter, string stateName)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;
        if (progress >= GamePlayDefine.AttackMeterProgressWait)
        {
            mCustomAnimDriver.PlayAnimStateWithCut(stateName);
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, null);
        }
    }

    private void ProgressWaitOnComboAttack(byte cmdType, Vector3 towards, int triggerMeter, TriggerableCombo combo)
    {
        if (combo == null)
            return;

        ComboActionData actionData = combo.GetCurrentComboAction();
        if (actionData == null)
            return;

        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;
        if (progress >= GamePlayDefine.AttackMeterProgressWait)
        {
            mCustomAnimDriver.PlayAnimStateWithCut(actionData.stateName);
            mAgent.ComboEffectsExcutor.Start(combo);
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, combo);
        }
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
                ProgressWaitOnNormalAttack(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, AgentAnimDefine.DefaultAnimName_AttackLong);
                break;
            case AgentCommandDefine.ATTACK_SHORT:
                ProgressWaitOnNormalAttack(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, AgentAnimDefine.DefaultAnimName_AttackShort);
                break;
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggerableCombo combo)
    {
        base.CustomOnComboCommand(cmd, combo);
        mAgent.MoveControl.TurnTo(cmd.Towards);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.DASH:
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, combo);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                ProgressWaitOnComboAttack(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, combo);
                break;
            default:
                break;
        }
    }


    protected override void CommandHandleOnMeter(int meterIndex)
    {
        Log.Error(LogLevel.Info, "-------------------------------meter {0}---------------------------------", meterIndex);

        if (meterIndex < mCurLogicStateEndMeter)
            return;

        TriggerableCombo combo = GetCurTriggeredCombo();
        if (changeToTransferState)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            float duration = combo.GetComboTransferDuration();
            args.Add("duration", duration);
            ChangeStatus(AgentStatusDefine.TRANSFER, args);
            return;
        }

        changeToTransferState = false;
        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            mAgent.MoveControl.TurnTo(towards);

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ChangeStatusOnNormalCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.ATTACK_SHORT:
                    if(combo != null)
                    {
                        ComboActionData actionData = combo.GetCurrentComboAction();
                        if (actionData != null)
                        {
                            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(actionData.stateName);
                            mAgent.ComboEffectsExcutor.Start(combo);
                            changeToTransferState = actionData.endFlag;
                        }
                    }
                    else
                    {
                        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(AgentAnimDefine.DefaultAnimName_AttackShort);
                    }
                    break;
                case AgentCommandDefine.ATTACK_LONG:
                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    break;
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(MeterManager.Ins.MeterIndex == mCurLogicStateEndMeter && !cmdBuffer.HasCommand())
        {
            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset);

            // 超过输入的容差时间，进入idle
            if (!inInputTime)
            {
                ChangeToIdle();
            }
        }

        Log.Logic(LogLevel.Info, "cur anim state:{0}, progress:{1}", mAgent.AnimPlayer.CurStateName, mAgent.AnimPlayer.CurStateProgress);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }
}
