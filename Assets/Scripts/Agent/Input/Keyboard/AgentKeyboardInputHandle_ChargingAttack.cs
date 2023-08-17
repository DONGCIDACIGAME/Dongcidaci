public class AgentKeyboardInputHandle_ChargingAttack : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_ChargingAttack(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_ChargingAttack;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mHero == null)
            return;

        AgentCommand cmd;
        bool hasCmd = GetRunInputCmd(out cmd);
        if (hasCmd)
        {
            mHero.OnCommand(cmd);
        }
    }
}
