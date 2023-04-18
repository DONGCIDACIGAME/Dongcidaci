using UnityEngine;

public abstract class AgentKeyboardInputHandle : InputHandle
{
    protected Agent mAgent;
    public AgentKeyboardInputHandle(Agent agt)
    {
        mAgent = agt;
    }

    private Vector3 GetInputDirection()
    {
        Vector3 towards = GamePlayDefine.InputDirection_NONE;
        if (Input.GetKey(KeyCode.W))
        {
            towards += DirectionDef.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            towards += DirectionDef.down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            towards += DirectionDef.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            towards += DirectionDef.right;
        }

        return towards;
    }

    protected bool GetDashInputCommand(out AgentInputCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();
        if (Input.GetKeyDown(InputDef.DashKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.DashMeterCheckTolerance))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.DASH, towards);
            return true;
        }

        return false;
    }

    protected bool GetAttackInputCmd(out AgentInputCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyDown(InputDef.LightAttackKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.AttackMeterCheckTolerance))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.ATTACK_HARD, towards);
            return true;
        }

        if (Input.GetKeyDown(InputDef.HardAttackKeyCode) && MeterManager.Ins.CheckTriggerCurrentMeter(GamePlayDefine.AttackMeterCheckTolerance))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.ATTACK_HARD, towards);
            return true;
        }

        return false;
    }

    protected bool GetRunInputCmd(out AgentInputCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();

        if(!towards.Equals(Vector3.zero))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.RUN, towards);
            return true;
        }

        return false;
    }
}
