public class AgentKeyboardInputHandle_AttackTransition : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_AttackTransition(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_AttackTransition;
    }
}
