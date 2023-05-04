using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    private ComboHandler mComboHandler;
    private AttackAnimDriver animDriver;
    private bool[] mTriggerRecord;

    public override void CustomInitialize()
    {
        mComboHandler = new ComboHandler();
        ComboGraph cg = DataCenter.Ins.AgentComboGraphCenter.GetAgentComboGraph(mAgent.GetAgentId());
        mComboHandler.Initialize(cg);
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        animDriver = new AttackAnimDriver(mAgent, GetStatusName());
        mTriggerRecord = new bool[MeterManager.Ins.GetCurAudioTotalMeterLen()];
    }

    private void ResetTriggerRecord()
    {
        if (mTriggerRecord == null)
            return;

        for(int i = 0;i<mTriggerRecord.Length;i++)
        {
            mTriggerRecord[i] = false;
        }
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        byte triggerCmd = (byte)context["triggerCmd"];
        Vector3 towards = (Vector3)context["towards"];
        int triggerMeter = (int)context["triggerMeter"];

        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "CmdHandleOnEnterAttack Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;

        if (progress >= GamePlayDefine.AttackMeterProgressWait)
        {
            animDriver.PlayAnimState("AttackBegin");
        }

        DelayToMeterExcuteCommand(triggerCmd, towards);
        //ProgressWaitOnAtk(triggerCmd, towards, triggerMeter);
    }

    public override void OnExit()
    {
        base.OnExit();
        mComboHandler.ClearComboActionRecord();
    }

    private void ProgressWaitOnAttack(byte cmdType, Vector3 towards, int triggerMeter)
    {
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
            TryTriggerCombo(triggerMeter, cmdType, towards);
        }
        else
        {
            DelayToMeterExcuteCommand(cmdType, towards);
        }
    }

    protected override void CustomOnCommand(AgentInputCommand cmd)
    {
        base.CustomOnCommand(cmd);

        switch (cmd.CmdType)
        {
            case AgentCommandDefine.BE_HIT:
                ExcuteCommand(cmd);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:
                DelayToMeterExcuteCommand(cmd.CmdType, cmd.Towards);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    private int lastComboMeterIndex;
    protected bool TryTriggerCombo(int triggerIndex, byte cmdType, Vector3 towards)
    {
        if (lastComboMeterIndex == triggerIndex)
            return false;

        if(mComboHandler.OnCmd(cmdType, out Combo combo, out ComboMove comboMove))
        {
            mAgent.MoveControl.TurnTo(towards);
            string stateName = comboMove.animState;
            mCurAnimStateEndMeter = animDriver.PlayAnimState(stateName);
            Log.Error(LogLevel.Info, "OnCombo----{0}--{1}--cur meter:{2}, end meter:{3}", combo.comboName, stateName, MeterManager.Ins.MeterIndex, mCurAnimStateEndMeter);
            lastComboMeterIndex = triggerIndex;
            return true;
        }

        return false;
    }



    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            if(TryTriggerCombo(meterIndex, cmdType, towards))
            {
                return;
            }

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ExcuteCommand(cmdType, towards, meterIndex);
                    return;
                case AgentCommandDefine.ATTACK_HARD:
                case AgentCommandDefine.ATTACK_LIGHT:
                    mAgent.MoveControl.TurnTo(towards);
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

        if(meterIndex == 0)
        {
            ResetTriggerRecord();
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }

    public override void Dispose()
    {
        base.Dispose();

        if(animDriver != null)
        {
            animDriver.Dispose();
            animDriver = null;
        }
    }
}
