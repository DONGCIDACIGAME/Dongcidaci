using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public class AgentStatus_Transfer : AgentStatus
{
    /// <summary>
    /// 状态持续时间
    /// </summary>
    private float mStateDuration;

    /// <summary>
    /// 计时
    /// </summary>
    private float mTime;

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

    public override void OnEnter(byte cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> context)
    {
        base.OnEnter(cmdType, towards, triggerMeter, context);

        mStateDuration = 0;
        mTime = 0;
        if (context.TryGetValue("duration", out object obj))
        {
            float duration = (float)obj;
            mStateDuration = MeterManager.Ins.GetCurrentMeterTime() * duration;
        }

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

        mTime += deltaTime;

        if(mTime >= mStateDuration)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            //args.Add("cmdType", AgentCommandDefine.IDLE);
            //args.Add("towards", DirectionDef.none);
            //args.Add("triggerMeter", MeterManager.Ins.MeterIndex);
            GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args);
        }
    }

    public override void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData)
    {
        
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new KeyboardInputHandle_Transfer(mAgent);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }
}
