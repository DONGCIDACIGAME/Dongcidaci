public class AgentKeyboardInputHandle_Idle : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Idle(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Idle;
    }
}

