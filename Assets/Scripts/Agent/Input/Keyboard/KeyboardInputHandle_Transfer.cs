public class KeyboardInputHandle_Transfer : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Transfer(Agent agt) : base(agt)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Transfer;
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
