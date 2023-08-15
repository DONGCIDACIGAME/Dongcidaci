public class AgentKeyboardInputHandle_Run : AgentKeyboardInputHandle
{
    public AgentKeyboardInputHandle_Run(Hero hero) : base(hero)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.AgentKeyboardInputHandle_Run;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mHero == null)
            return;
        AgentCommand cmd;
        bool hasCmd = GetInstantAttackInputCmd(out cmd)
            || GetChargingCmd(out cmd)
            || GetChargingAttackCmd(out cmd)
            || GetDashInputCommand(out cmd)
            || GetRunInputCmd(out cmd);


        if (!hasCmd)
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, DirectionDef.none);
        }

        mHero.OnCommand(cmd);
    }
}
