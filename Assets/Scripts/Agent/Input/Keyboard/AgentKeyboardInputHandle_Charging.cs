public class AgentKeyboardInputHandle_Charging : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Charging(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Charging;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mHero == null)
            return;

        AgentCommand cmd;
        bool hasCmd = GetInstantAttackInputCmd(out cmd)
            || GetChargingAttackCmd(out cmd)
            || GetDashInputCommand(out cmd)
            || GetRunInputCmd(out cmd);


        if (hasCmd)
        {
            mHero.OnCommand(cmd);
        }
    }
}
