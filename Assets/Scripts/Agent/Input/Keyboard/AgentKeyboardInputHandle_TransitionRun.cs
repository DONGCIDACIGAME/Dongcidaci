public class AgentKeyboardInputHandle_TransitionRun : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_TransitionRun(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_MeterRun;
    }
}
