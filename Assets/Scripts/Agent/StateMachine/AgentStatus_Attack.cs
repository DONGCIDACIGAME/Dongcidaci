using GameEngine;
using System.Collections;
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
        Log.Error(LogLevel.Normal, "ProgressWaitOnNormalAttack Error, all attack input must trigger combo!");
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
            mCustomAnimDriver.PlayAnimStateWithCut(stateName);
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, null);
        }
    }

    private void ProgressWaitOnComboAttack(byte cmdType, Vector3 towards, int triggerMeter, TriggerableCombo combo)
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
            ExcuteCombo(combo);
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
            case AgentCommandDefine.ATTACK_SHORT:
                Log.Error(LogLevel.Normal, "CustomOnNormalCommand Error, all attack input must trigger combo!");
                break;
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }
    }

    private void ExcuteCombo(TriggerableCombo combo)
    {
        if (combo == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, combo is null!");
            return;
        }

        ComboActionData actionData = combo.GetCurrentComboAction();
        if (actionData == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, ComboActionData is null, combo name:{0}, index:{1}!",combo.GetComboName(), combo.triggeredAt);
            return;
        }

        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(actionData.stateName);
        mAgent.ComboEffectsExcutor.Start(combo);
        if(actionData.endFlag)
        {
            //mChangeToTransfer = true;
            //transferDuration = combo.GetComboTransferDuration();
            mAgent.ComboTrigger.ResetAllCombo();
        }

        mCurTriggeredCombo = null;
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

    private bool mChangeToTransfer;
    private float transferDuration;

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurLogicStateEndMeter)
            return;

        //if(mChangeToTransfer)
        //{
        //    Dictionary<string, object> args = new Dictionary<string, object>();
        //    args.Add("duration", transferDuration);
        //    ChangeStatus(AgentStatusDefine.IDLE, args);
        //    return;
        //}
        
        TriggerableCombo combo = GetCurTriggeredCombo();
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
                    if(combo == null)
                    {
                        Log.Error(LogLevel.Normal, "CommandHandleOnMeter Error, all attack input must trigger combo!");
                        //mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(AgentAnimDefine.DefaultAnimName_AttackShort);
                    }
                    else
                    {
                        ExcuteCombo(combo);
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

        //if (MeterManager.Ins.MeterIndex == mCurLogicStateEndMeter && !cmdBuffer.HasCommand())
        //{
        //    // 是否在输入的容差时间内
        //    bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset);

        //    // 超过输入的容差时间，进入idle
        //    if (!inInputTime)
        //    {
        //        ChangeToIdle();
        //    }
        //}

        //Log.Logic(LogLevel.Info, "cur anim state:{0}, progress:{1}", mAgent.AnimPlayer.CurStateName, mAgent.AnimPlayer.CurStateProgress);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }
}
