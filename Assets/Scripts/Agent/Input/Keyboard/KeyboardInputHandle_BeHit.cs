public class KeyboardInputHandle_BeHit : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_BeHit(Agent agt) : base(agt)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_BeHit;
    }

    public override void OnMeterEnter(int meterIndex)
    {

    }

    public override void OnMeterEnd(int meterIndex)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        //if (mAgent == null)
        //    return;

        //AgentInputCommand cmd;
        //bool hasCmd = GetAttackInputCmd(out cmd) || GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);
        //if (!hasCmd)
        //{
        //    cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        //    cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, DirectionDef.none);
        //}
        //mAgent.OnCommand(cmd);
    }
}
