using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Dead : HeroStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.DEAD;
    }

    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
       
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

    public override void RegisterInputHandle()
    {

    }

    public override void UnregisterInputHandle()
    {
        
    }
}
