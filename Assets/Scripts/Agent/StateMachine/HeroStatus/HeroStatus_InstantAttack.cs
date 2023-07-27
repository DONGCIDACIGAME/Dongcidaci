using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_InstantAttack : HeroStatus
{
    private float mExitTime;
    private float mTimer;

    public override string GetStatusName()
    {
        return AgentStatusDefine.ATTACK;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_InstantAttack(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    private void ExcuteCmd(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        if (triggeredComboStep != null)
        {
            ExcuteCombo(cmdType, towards, triggerMeter, args, ref triggeredComboStep);
        }
        else
        {
            StatusDefaultAction(cmdType, towards, triggerMeter, args, statusDefaultActionData);
        }
    }

    /// <summary>
    /// 状态进入
    /// </summary>
    /// <param name="context"></param>
    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);

        ExcuteCmd(cmdType, towards, triggerMeter, args, triggeredComboStep);
    }

    /// <summary>
    /// 状态退出
    /// </summary>
    public override void OnExit()
    {
        base.OnExit();
    }


    /// <summary>
    /// 常规指令直接处理逻辑
    /// </summary>
    /// <param name="cmd"></param>
    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        switch (cmdType)
        {
            // 接收到打断型的受击指令，马上切换到受击状态
            case AgentCommandDefine.BE_HIT_BREAK:
                ChangeStatusOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.ATTACK_LONG:
            case AgentCommandDefine.ATTACK_SHORT:
            case AgentCommandDefine.ATTACK_LONG_INSTANT:
            case AgentCommandDefine.ATTACK_SHORT_INSTANT:
                ExcuteCmd(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            // 其他指令类型，都要等本次攻击结束后执行，先放入指令缓存区
            case AgentCommandDefine.DASH:
            case AgentCommandDefine.RUN:
            case AgentCommandDefine.IDLE:
            case AgentCommandDefine.RUN_METER:
                PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
                break;
            case AgentCommandDefine.BE_HIT://攻击状态下，非打断的受击行为不做处理
            case AgentCommandDefine.EMPTY:
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 节拍开始逻辑
    /// </summary>
    /// <param name="meterIndex"></param>
    protected override void CustomOnMeterEnter(int meterIndex)
    {
        // 逻辑拍结束前，不能响应缓存区指令
        if (meterIndex <= mCurLogicStateEndMeter)
        {
            //Log.Error(LogLevel.Info, "CustomOnMeterEnter--- meterIndex:{0}, logicMeterEnd:{1}", meterIndex, mCurLogicStateEndMeter);
            return;
        }

        // 缓存区取指令
        if (cmdBuffer.PeekCommand(out int cmdType, out Vector3 towards, out int triggerMeter, out Dictionary<string, object> args))
        {
            Log.Logic(LogLevel.Info, "PeekCommand:{0}-----cur meter:{1}", cmdType, meterIndex);

            switch (cmdType)
            {
                case AgentCommandDefine.BE_HIT_BREAK:
                case AgentCommandDefine.RUN:
                case AgentCommandDefine.DASH:
                case AgentCommandDefine.IDLE:
                    ChangeStatusOnCommand(cmdType, towards, meterIndex, args, mCurTriggeredComboStep);
                    break;
                case AgentCommandDefine.ATTACK_SHORT:
                case AgentCommandDefine.ATTACK_LONG:
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

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnGameUpdate(float deltaTime)
    {
        base.OnGameUpdate(deltaTime);

        mTimer += deltaTime;

        if (mTimer >= mExitTime)
        {
            Dictionary<string, object> _args = null;
            TriggeredComboStep _triggeredComboStep = null;
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.TRANSITION, AgentCommandDefine.EMPTY, DirectionDef.none, MeterManager.Ins.MeterIndex, _args, _triggeredComboStep);
        }
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
    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        if (agentActionData == null)
            return;

        // 1. 转向被攻击的方向
        mAgent.MoveControl.TurnTo(towards);

        string statusName = agentActionData.statusName;
        string stateName = agentActionData.stateName;

        // 2. 播放攻击动画
        mDefaultCrossFadeAnimDriver.CrossFadeToState(statusName, stateName);

        // 3. 处理动画相关的位移
        mAgent.MovementExcutorCtl.Start(statusName, stateName, DirectionDef.RealTowards, DirectionDef.none, 0);

        AgentAnimStateInfo animStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);

        // 攻击动作拍数
        mCurLogicStateEndMeter = MeterManager.Ins.GetMeterIndex(triggerMeter, animStateInfo.meterLen) - 1;

        // 攻击动作的时长
        mExitTime = animStateInfo.animLen;
        mTimer = 0;
    }
}
