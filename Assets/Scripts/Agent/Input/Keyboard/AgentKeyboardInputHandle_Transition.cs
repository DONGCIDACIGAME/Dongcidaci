public class AgentKeyboardInputHandle_Transition : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Transition(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Transition;
    }
}
