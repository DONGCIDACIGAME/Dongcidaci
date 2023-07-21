public class KeyboardInputHandle_InstantAttack : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_InstantAttack(Agent agt) : base(agt)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_InstantAttack;
    }


    public override void OnMeterEnter(int meterIndex)
    {

    }

    public override void OnMeterEnd(int meterIndex)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        AgentCommand cmd;
        bool hasCmd = GetAttackInputCmd(out cmd) || GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);
        if (hasCmd)
        {
            mAgent.OnCommand(cmd);
        }
    }
}
