public class AgentKeyboardInputHandle_RunMeter : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_RunMeter(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_MeterRun;
    }
}
