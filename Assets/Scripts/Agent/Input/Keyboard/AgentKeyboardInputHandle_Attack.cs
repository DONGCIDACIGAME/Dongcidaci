public class AgentKeyboardInputHandle_Attack : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Attack(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Attack;
    }

}

