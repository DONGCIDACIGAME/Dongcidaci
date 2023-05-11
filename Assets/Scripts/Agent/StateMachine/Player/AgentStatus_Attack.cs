using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    public override void CustomInitialize()
    {
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
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
                float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
                // 当前拍的总时间
                float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

                if (timeOfCurrentMeter == 0)
                {
                    ComboStepData comboStep = combo.GetCurrentComboStep();
                    mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimState(comboStep.animState);
                    return;
                }

                float progress = timeToNextMeter / timeOfCurrentMeter;

                //if (progress >= GamePlayDefine.AttackMeterProgressWait)
                //{
                //    animDriver.PlayAnimState("AttackBegin");
                //}

                PushInputCommandToBuffer(triggerCmd, towards);
            }
        }



    }

    public override void OnExit()
    {
        base.OnExit();
    }

    private void ProgressWaitOnAttack(byte cmdType, Vector3 towards, int triggerMeter, TriggerableCombo combo)
    {
        if (combo == null)
            return;

        ComboStepData comboStep = combo.GetCurrentComboStep();
        if (comboStep == null)
            return;

        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
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
            string stateName = comboStep.animState;
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimState(stateName);
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards);
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
                PushInputCommandToBuffer(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
                //PushInputCommandToBuffer(cmd.CmdType, cmd.Towards);
                //Log.Logic(LogLevel.Info, "++++++++++++++++++++DelayToMeterExcuteCommand Add Atk----{0} ", MeterManager.Ins.MeterIndex);
                //ProgressWaitOnAttack(cmd.CmdType, cmd.Towards, cmd.TriggerMeter);
                Log.Error(LogLevel.Normal, "CustomOnNormalCommand Error，所有攻击模式应该都能匹配上combo！");
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnComboCommand(AgentInputCommand cmd, TriggerableCombo combo)
    {
        base.CustomOnComboCommand(cmd, combo);
        mAgent.MoveControl.TurnTo(cmd.Towards);
        ProgressWaitOnAttack(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, combo);
    }



    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurLogicStateEndMeter)
            return;

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ChangeStatusOnNormalCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.ATTACK_LONG:
                case AgentCommandDefine.ATTACK_SHORT:
                    TriggerableCombo combo = mAgent.ComboTrigger.GetCurTriggeredCombo();
                    if(combo != null)
                    {
                        ComboStepData comboStep = combo.GetCurrentComboStep();
                        if (comboStep != null)
                        {
                            mAgent.MoveControl.TurnTo(towards);

                            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimState(comboStep.animState);
                        }
                    }

                    break;
                case AgentCommandDefine.EMPTY:
                default:
                    return;
            }
        }
    }

    public override void OnMeter(int meterIndex)
    {
        base.OnMeter(meterIndex);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }
}
