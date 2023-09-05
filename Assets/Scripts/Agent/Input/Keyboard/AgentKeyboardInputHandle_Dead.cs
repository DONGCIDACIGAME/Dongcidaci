public class AgentKeyboardInputHandle_Dead : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Dead(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Dead;
    }
}
