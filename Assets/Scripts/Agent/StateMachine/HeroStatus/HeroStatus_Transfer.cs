using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Transfer : HeroStatus
{
    /// <summary>
    /// 状态持续时间
    /// </summary>
    private float mExitTime;

    /// <summary>
    /// 计时
    /// </summary>
    private float mTimer;

    public override string GetStatusName()
    {
        return AgentStatusDefine.TRANSFER;
    }

    public override void CustomInitialize()
    {
        base.CustomInitialize();
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
    }

    public override void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.OnEnter(cmdType, towards, triggerMeter, args, triggeredComboStep);


        StatusDefaultAction(cmdType, towards, triggerMeter, args, null);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {

    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        mTimer += deltaTime;

        if (mTimer >= mExitTime)
        {
            int meterIndex = MeterManager.Ins.MeterIndex;
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
                    case AgentCommandDefine.ATTACK_SHORT:
                    case AgentCommandDefine.ATTACK_LONG:
                        GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), GetChangeToStatus(cmdType), cmdType, towards, triggerMeter, args, mCurTriggeredComboStep);
                        //PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, mCurTriggeredComboStep);
                        break;
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
            //GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, mCurTriggeredComboStep);
        }
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        base.CustomOnCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);

        PushInputCommandToBuffer(cmdType, towards, triggerMeter, args, triggeredComboStep);
    }

    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        mExitTime = 0;
        mTimer = 0;
        if (args != null && args.TryGetValue("duration", out object obj))
        {
            float duration = (float)obj;
            mExitTime = MeterManager.Ins.GetCurrentMeterTotalTime() * duration;
        }
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_Transfer(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}
