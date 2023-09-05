using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_ChargingTransition : HeroStatus
{
    private ChargeAttackStep mChargeAtkStep;

    public override string GetStatusName()
    {
        return AgentStatusDefine.CHARGING_TRANSITION;
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);


        StatusDefaultAction(towards, triggerMeter, args);
    }

    public override void OnExit()
    {
        base.OnExit();
    }



    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        if (cmdType == AgentCommandDefine.BE_HIT_BREAK)
        {
            ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
            return;
        }

        //if (mTimer < mExitTime)
        //    return;


        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.IDLE:
                //PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                Log.Error(LogLevel.Normal, "Invalid Cmd, get charging attack on {0}!", GetStatusName());
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(towards);
                break;
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if(mChargeAtkStep != null)
        {
            Dictionary<string, object> _args = new Dictionary<string, object>();
            _args.Add("chargeAtkStep", mChargeAtkStep);
            ChangeStatusOnCommand(AgentCommandDefine.CHARGING_ATTACK, mAgent.GetTowards(), meterIndex, _args, null);
        }
    }


    public void StatusDefaultAction(Vector3 towards, int triggerMeter, Dictionary<string, object> args)
    {
        if (args != null && args.TryGetValue("chargeAtkStep", out object chargeAtkStep))
        {
            mChargeAtkStep = chargeAtkStep as ChargeAttackStep;
            string statusName = mChargeAtkStep.attackAction.statusName;
            string stateName = mChargeAtkStep.attackAction.stateName;

            AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
            mMatchMeterCrossfadeAnimDriver.StartPlay(stateInfo.stateName, stateInfo.animName, stateInfo.loopTime);
            // 3. 处理动画相关的位移
            mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);
        }
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_ChargingTransition(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
