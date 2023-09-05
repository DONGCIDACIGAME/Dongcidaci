using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_AttackTransition : HeroStatus
{
    private float mTimer;
    private float mExitTime;

    private Vector3 mLastTowards;

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK_TRANSITION;
    }


    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        if (mTimer < mExitTime)
        {
            mTimer += deltaTime;
            return;
        }

        ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, null, null);
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        
        StatusDefaultAction(towards, triggerMeter, args);
    }

    public override void OnExit()
    {
        base.OnExit();
        mTimer = 0;
    }

    private bool CheckTowardsBigChange(Vector3 newTowards, Vector3 oldTowards, float threshold)
    {
        //Log.Error(LogLevel.Info, "CheckTowardsBigChange-----newTowards:{0}, oldTowards:{1}", newTowards, oldTowards);

        return (oldTowards.normalized - newTowards.normalized).magnitude >= threshold;
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
                if (CheckTowardsBigChange(towards, mLastTowards, 1f))
                {
                    ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                    //GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.RUN, cmdType, towards, triggerMeter, args, triggeredComboStep);
                    mLastTowards = towards;
                }
                break;
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }

    }

    public void StatusDefaultAction(Vector3 towards, int triggerMeter, Dictionary<string, object> args)
    {
        AgentActionData agentActionData = GetStatusDefaultActionData();
        if(args != null && args.TryGetValue("transitionAction", out object action))
        {
            agentActionData = action as AgentActionData;
        }

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        float timeToMeter = MeterManager.Ins.GetTimeToMeter(triggerMeter + 1);
        float totalTime = timeToMeter + GamePlayDefine.InputCheckOffset/2 + GamePlayDefine.InputCheckOffset;
        //mDefaultCrossFadeAnimDriver.CrossFadeToState(stateName, stateInfo.layer, stateInfo.loopTime, stateInfo.meterLen, stateInfo.normalizedTime, totalTime);
        mExitTime = mDefaultCrossFadeAnimDriver.StartPlay(stateInfo.stateName, stateInfo.animName, stateInfo.layer, stateInfo.loopTime, stateInfo.meterLen, stateInfo.normalizedTime, totalTime);
        // 3. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);
        mTimer = 0;
        //AgentAnimStateInfo stateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        //if (stateInfo != null)
        //{
        //mExitTime = stateInfo.animLen;
        //}
        //mExitTime = totalTime;
        mLastTowards = towards;
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        // 缓存区取指令
        if (cmdBuffer.PeekCommand(mCurLogicStateEndMeter, out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args, out TriggeredComboStep comboStep))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);
            cmdBuffer.ClearCommandBuffer();
            switch (cmdType)
            {
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.INSTANT_ATTACK:
                case AgentCommandDefine.METER_ATTACK:
                case AgentCommandDefine.CHARGING:
                case AgentCommandDefine.CHARGING_ATTACK:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.BE_HIT_BREAK:
                case AgentCommandDefine.IDLE:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, comboStep);
                    break;
                case AgentCommandDefine.EMPTY:
                case AgentCommandDefine.BE_HIT:
                default:
                    break;
            }
        }
        //else
        //{
        //    ChangeStatusOnCommand(AgentCommandDefine.IDLE, DirectionDef.none, meterIndex, null, null);
        //}
    }


    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_AttackTransition(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
