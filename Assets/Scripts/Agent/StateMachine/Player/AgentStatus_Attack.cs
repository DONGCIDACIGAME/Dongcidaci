using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 重击状态
/// </summary>
public class AgentStatus_Attack : AgentStatus
{
    private ComboHandler mComboHandler;

    public override void CustomInitialize()
    {
        mComboHandler = new ComboHandler();
        mInputHandle = new KeyboardInputHandle_Attack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    private void OnCombo(Combo cmb)
    {
        string stateName = cmb.animStateList[cmb.animStateList.Length - 1];
        AgentAnimStateInfo state = mAgent.GetStateInfo("Attack",stateName);
        /// TODO: 自定义的动画驱动器
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

        Combo cmb = mComboHandler.OnInput(triggerCmd);
        if(cmb != null)
        {
            OnCombo(cmb);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        //ResetAnimQueue();
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
            Log.Logic(LogLevel.Info, "PeekCommand--{0}", cmdType);
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

            //AnimQueueMoveOn();
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
}
