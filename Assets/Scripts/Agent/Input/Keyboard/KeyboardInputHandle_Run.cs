public class KeyboardInputHandle_Run : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Run(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Run;
    }

    public override void OnMeterEnter(int meterIndex)
    {
        
    }

    public override void OnMeterEnd(int meterIndex)
    {

    }
    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {

    }
}
