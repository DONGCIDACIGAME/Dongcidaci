public class AgentKeyboardInputHandle_InstantAttack : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_InstantAttack(Hero hero) : base(hero)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_InstantAttack;
    }

    //public override void OnUpdate(float deltaTime)
    //{
    //    if (mHero == null)
    //        return;
    //    AgentCommand cmd;
    //    bool hasCmd = GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);
    //    if (hasCmd)
    //    {
    //        mHero.OnCommand(cmd);
    //    }
    //}

}
