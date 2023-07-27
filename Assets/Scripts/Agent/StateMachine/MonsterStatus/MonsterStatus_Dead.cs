using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_Dead : MonsterStatus
{
    public override string GetStatusName()
    {
        throw new System.NotImplementedException();
    }

    public override void StatusDefaultAction(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, AgentActionData agentActionData)
    {
        throw new System.NotImplementedException();
    }

    protected override void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        throw new System.NotImplementedException();
    }

    protected override void CustomOnMeterEnd(int meterIndex)
    {
        throw new System.NotImplementedException();
    }

    protected override void CustomOnMeterEnter(int meterIndex)
    {
        throw new System.NotImplementedException();
    }
}
