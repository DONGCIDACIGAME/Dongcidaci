using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Charging : HeroStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.CHARGING;
    }

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_Charging(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        
    }
}
