using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_InstantAttack : AgentStatus
{
    private float mExitTime;
    private float mTimer;

    public override string GetStatusName()
    {
        return AgentStatusDefine.INSTANT_ATTACK;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_InstantAttack(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        StatusDefaultAction(cmdType, towards, triggerMeter, args, GetAgentActionData());
    }

    public override void OnExit()
    {
        base.OnExit();

        mTimer = 0;
        mExitTime = 0;
    }


    /// <summary>
    /// attack的默认逻辑
    /// 1. 转向攻击方向
    /// 2. 播放攻击动画
    /// 3. 造成伤害
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="agentActionData"></param>
    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        // 1. 转向被攻击的方向
        mAgent.MoveControl.TurnTo(towards);

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        AgentAnimStateInfo info = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);

        // 2. 播放攻击动画
        mDefaultCrossAnimDriver.CrossFadeToState(statusName, stateName);

        // 3. 处理动画相关的位移
        //mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);
        mAgent.MovementExcutorCtl.Start(0, info.animLen, DirectionDef.RealTowards, DirectionDef.none, 0);

        mExitTime = info.animLen;
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        mTimer += deltaTime;
        if (mTimer >= mExitTime)
        {
            int meterIndex = MeterManager.Ins.MeterIndex;
            // 缓存区取指令
            if (cmdBuffer.PeekCommand(out byte cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args))
            {
                Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

                switch (cmdType)
                {
                    case AgentCommandDefine.BE_HIT_BREAK:
                    case AgentCommandDefine.RUN:
                    case AgentCommandDefine.DASH:
                    case AgentCommandDefine.IDLE:
                    case AgentCommandDefine.ATTACK_SHORT:
                    case AgentCommandDefine.ATTACK_LONG:
                        GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentCommandDefine.GetChangeToStatus(cmdType), cmdType, towards, triggerMeter, args, mCurTriggeredComboStep);
                        //PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, mCurTriggeredComboStep);
                        break;
                    case AgentCommandDefine.EMPTY:
                    case AgentCommandDefine.BE_HIT:
                    default:
                        break;
                }
            }

            //GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, mCurTriggeredComboStep);
        }
    }

    protected override void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);

        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
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
        
    }
}
