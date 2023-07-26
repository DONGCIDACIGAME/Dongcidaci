public class KeyboardInputHandle_RunMeter : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_RunMeter(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_MeterRun;
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
}
