using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    private ComboHandler mComboHandler;
    private CustomAnimDriver animDriver;
    private bool[] mTriggerRecord;

    public override void CustomInitialize()
    {
        mComboHandler = new ComboHandler();
        ComboGraph cg = DataCenter.Ins.AgentComboGraphCenter.GetAgentComboGraph(mAgent.GetAgentId());
        mComboHandler.Initialize(cg);
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
        animDriver = new CustomAnimDriver(mAgent, GetStatusName());
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

    private void OnCombo(Combo cmb)
    {
        string stateName = cmb.animStateList[cmb.animStateList.Length - 1];
        mCurAnimStateEndMeter = animDriver.PlayAnimState(stateName);
        Log.Error(LogLevel.Info, "OnCombo----{0}--{1}--cur meter:{2}, end meter:{3}", cmb.comboName, stateName, MeterManager.Ins.MeterIndex, mCurAnimStateEndMeter);
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        base.OnEnter(context);

        byte triggerCmd = (byte)context["triggerCmd"];
        Vector3 towards = (Vector3)context["towards"];

        if(towards != GamePlayDefine.InputDirection_NONE)
        {
            mAgent.MoveControl.TurnTo(towards);
        }

        Combo cmb = mComboHandler.OnCmd(triggerCmd);
        if (cmb != null)
        {
            OnCombo(cmb);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
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
                DelayToMeterExcuteCommand(cmd);
                break;
            case AgentCommandDefine.ATTACK_HARD:
            case AgentCommandDefine.ATTACK_LIGHT:

                DelayToMeterExcuteCommand(cmd);
                break;
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CommandHandleOnMeter(int meterIndex)
    {
        if (meterIndex < mCurAnimStateEndMeter)
            return;

        if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            Combo cmb = mComboHandler.OnCmd(cmdType);
            // 如果触发了combo，就走combo的配置
            if (cmb != null)
            {
                OnCombo(cmb);
                return;
            }

            switch (cmdType)
            {
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                    ExcuteCommand(cmdType, towards);
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
