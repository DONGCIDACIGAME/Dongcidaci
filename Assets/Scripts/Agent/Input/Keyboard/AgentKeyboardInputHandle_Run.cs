public class AgentKeyboardInputHandle_Run : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Run(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Run;
    }
}
