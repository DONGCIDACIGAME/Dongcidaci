public class AgentKeyboardInputHandle_Charging : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Charging(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Charging;
    }
}
