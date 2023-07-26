using UnityEngine;
using GameEngine;

public class MouseInputHandle_CommonInput : InputHandle
{
    public override string GetHandleName()
    {
        return InputDef.MouseInputHandle_CommonInput;
    }

    public override void OnMeterEnd(int meterIndex)
    {
        
    }

    public override void OnMeterEnter(int meterIndex)
    {
        
    }

    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameEventSystem.Ins.Fire("MouseButtonDown");
        }

        if(Input.GetMouseButtonUp(0))
        {
            GameEventSystem.Ins.Fire("MouseButtonUp");
        }
    }
}
