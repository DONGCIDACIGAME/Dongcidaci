public class KeyboardInputHandle_Idle : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Idle(Agent agt) : base(agt)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Idle;
    }

    public override void OnMeter(int meterIndex)
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        AgentInputCommand cmd;
        bool hasCmd = GetAttackInputCmd(out cmd) || GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);
        if (!hasCmd)
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
        }
        mAgent.OnCommand(cmd);
    }
}

