using UnityEngine;

public class KeyboardInputHandle_Run : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Run(Agent agt) : base(agt)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Run;
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
            cmd.Initialize(AgentCommandDefine.IDLE, GamePlayDefine.InputDirection_NONE);
        }
        mAgent.OnCommand(cmd);
    }
}
