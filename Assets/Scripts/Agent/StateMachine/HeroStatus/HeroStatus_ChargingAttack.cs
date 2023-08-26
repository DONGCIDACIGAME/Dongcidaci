using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_ChargingAttack : HeroStatus
{
    private Vector3 mMoveTowards;

    //private float mExitTime;
    //private float mTimer;

    public override string GetStatusName()
    {
        return AgentStatusDefine.CHARGING_ATTACK;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_ChargingAttack(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        StatusDefaultAction(towards, args);
    }

    public override void OnExit()
    {
        base.OnExit();
        mMoveTowards = DirectionDef.none;
        //mExitTime = 0;
        //mTimer = 0;
    }


    public void StatusDefaultAction(Vector3 towards, Dictionary<string, object> args)
    {
        // 1. 转向被攻击的方向
        mAgent.MoveControl.TurnTo(towards);

        AgentActionData agentActionData = GetStatusDefaultActionData();

        if (args.TryGetValue("chargeAtkStep", out object v))
        {
            ChargeAttackStep chargeAtkStep = v as ChargeAttackStep;
            agentActionData = chargeAtkStep.attackAction;
        }

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放攻击动画
        // 当前节拍的进度
        float progress = MeterManager.Ins.GetCurrentMeterProgress();
        if (progress <= GamePlayDefine.AttackMeterProgressWait)
        {
            mCurLogicStateEndMeter = mMatchMeterCrossfadeAnimDriver.StartPlay(statusName, stateName);
        }
        else
        {
            mCurLogicStateEndMeter = mMatchMeterCrossfadeAnimDriver.StartPlay(statusName, stateName, 1);
        }


        //mCurLogicStateEndMeter = mMatchMeterCrossfadeAnimDriver.StartPlay(statusName, stateName);
        //mExitTime = mDefaultCrossFadeAnimDriver.StartPlay(statusName, stateName);
        //mTimer = 0;

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
            case AgentCommandDefine.RUN:
                mMoveTowards = towards;
                break;
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.INSTANT_ATTACK:
            case AgentCommandDefine.METER_ATTACK:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.CHARGING:
            case AgentCommandDefine.CHARGING_ATTACK:
            case AgentCommandDefine.CHARGING_ATTACKFAILED:
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
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex <= mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        // 缓存区取指令
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.BE_HIT_BREAK:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.IDLE:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.METER_ATTACK:
                case AgentCommandDefine.CHARGING:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, comboStep);
                    break;
                case AgentCommandDefine.CHARGING_ATTACK:
                case AgentCommandDefine.EMPTY:
                case AgentCommandDefine.BE_HIT:
                default:
                    break;
            }
        }
        else
        {
            ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        }
    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        //mTimer += deltaTime;

        //if (mTimer >= mExitTime)
        //{
        //    ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
        //    return;
        //}

        if (mMoveTowards == DirectionDef.none)
            return;

        mAgent.MoveControl.MoveTowards(mMoveTowards, deltaTime * 1.5f, deltaTime);
    }
}