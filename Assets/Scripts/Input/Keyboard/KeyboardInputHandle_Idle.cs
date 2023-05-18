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

        // 攻击判断>冲刺判断>跑动判断
        // 存在逻辑短路

        bool hasCmd = GetAttackInputCmd(out cmd) || GetDashInputCommand(out cmd) || GetRunInputCmd(out cmd);

        // 没有新的cmd 回到idle
        if (!hasCmd)
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
        }

        // 执行cmd的操作
        mAgent.OnCommand(cmd);
    }

}

