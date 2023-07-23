public class KeyboardInputHandle_Idle : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Idle(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Idle;
    }

    public override void OnMeterEnter(int meterIndex)
    {
        
    }

    public override void OnMeterEnd(int meterIndex)
    {

    }

}

