using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus_Dead : MonsterStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.DEAD;
    }

    public void StatusDefaultAction()
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
}
