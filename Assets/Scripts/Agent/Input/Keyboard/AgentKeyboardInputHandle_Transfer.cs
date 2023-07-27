public class AgentKeyboardInputHandle_Transfer : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Transfer(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Transfer;
    }
}
