using System.Collections.Generic;
using UnityEngine;
using GameEngine;
public class HeroStatus_Charging : HeroStatus
{
    private int mStartChargingMeter;
    private ChargeAttackTrigger mChargeAtkTrigger;

    public override void CustomInitialize()
    {
        base.CustomInitialize();
        mChargeAtkTrigger = new ChargeAttackTrigger();
        mChargeAtkTrigger.Initialize(mAgent, "Charging");
    }

    public override string GetStatusName()
    {
        return AgentStatusDefine.CHARGING;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_Charging(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        mStartChargingMeter = triggerMeter;
        StatusDefaultAction(towards);
    }

    public override void OnExit()
    {
        base.OnExit();

        GameEventSystem.Ins.Fire("HeroEndCharging");
    }


    public void StatusDefaultAction(Vector3 towards)
    {
        // 1. 转向被攻击的方向
        mAgent.MoveControl.TurnTo(towards);


        AgentActionData agentActionData = mChargeAtkTrigger.GetChargeActionData();
        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放蓄力动画
        mMatchMeterCrossfadeAnimDriver.StartPlay(stateName, statusName);

        ChargeAttackStep maxChargeStep = mChargeAtkTrigger.GetMaxChargeStep();
        if(maxChargeStep != null)
        {
            float fullTime = MeterManager.Ins.GetTimeToMeterWithOffset(maxChargeStep.chargeMeterLen);
            GameEventSystem.Ins.Fire("HeroStartCharging", fullTime);
        }

        // 3. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        // 只响应打断型受击状态
        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.CHARGING_ATTACK:
                // PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);

                // 共蓄力了几拍
                int totalChargeMeter = triggerMeter - mStartChargingMeter;
                ChargeAttackStep chargeAtkStep = mChargeAtkTrigger.Trigger(totalChargeMeter);
                if (chargeAtkStep != null)
                {
                    Dictionary<string, object> _args = new Dictionary<string, object>();
                    _args.Add("chargeAtkStep", chargeAtkStep);
                    ChangeStatusOnCommand(cmdType, towards, triggerMeter, _args, triggeredComboStep);
                    //TriggeredComboStep _triggeredComboStep = null;
                    //GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetEntityId(), AgentStatusDefine.CHARGING_TRANSITION, AgentCommandDefine.EMPTY, towards, MeterManager.Ins.MeterIndex, _args, _triggeredComboStep);
                }
                else
                {
                    ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, triggerMeter, null, null);
                }

                break;
            case AgentCommandDefine.RUN:
                mAgent.MoveControl.TurnTo(towards);
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.CHARGING:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
            default:
                break;
        }
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.CHARGING_ATTACK:
                    // 共蓄力了几拍
                    int totalChargeMeter = triggerMeter - mStartChargingMeter;
                    ChargeAttackStep chargeAtkStep = mChargeAtkTrigger.Trigger(totalChargeMeter);
                    if(chargeAtkStep != null)
                    {
                        args.Add("chargeAtkStep", chargeAtkStep);
                        ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, comboStep);
                    }
                    else
                    {
                        ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, triggerMeter, null, null);
                    }
                    break;
                case AgentCommandDefine.BE_HIT_BREAK:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.EMPTY:
                case AgentCommandDefine.BE_HIT:
                default:
                    break;
            }
        }
    }
}
