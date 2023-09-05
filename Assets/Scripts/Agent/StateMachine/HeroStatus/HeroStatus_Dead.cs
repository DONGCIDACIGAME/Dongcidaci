using System.Collections.Generic;
using UnityEngine;

public class HeroStatus_Dead : HeroStatus
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

    public override void RegisterInputHandle()
    {
        mInputHandle = new AgentKeyboardInputHandle_Dead(mAgent as Hero);
        InputControlCenter.KeyboardInputCtl.RegisterInputHandle(mInputHandle.GetHandleName(), mInputHandle);
    }

    public override void UnregisterInputHandle()
    {
        InputControlCenter.KeyboardInputCtl.UnregisterInputHandle(mInputHandle.GetHandleName());
    }
}
