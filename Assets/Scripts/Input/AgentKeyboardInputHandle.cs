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
        if (Input.GetKeyDown(InputDef.DashKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset, out int triggerMeter))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.DASH, triggerMeter, towards);
            return true;
        }

        return false;
    }

    protected bool GetAttackInputCmd(out AgentInputCommand cmd)
    {
        cmd = null;
        int triggerMeter = -1;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyDown(InputDef.LightAttackKeyCode))
        {
            if(MeterManager.Ins.CheckTriggered(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset, out triggerMeter))
            {
                cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
                cmd.Initialize(AgentCommandDefine.ATTACK_LIGHT, triggerMeter, towards);
                //Log.Error(LogLevel.Info, "Trigger light attack--------------------------------------------------{0}", triggerMeter);
                return true;
            }
            else
            {
                Log.Error(LogLevel.Info, "---------------------------Trigger light attack FAILED{0}----------------------------", triggerMeter);
            }
        }

        if (Input.GetKeyDown(InputDef.HardAttackKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset, out triggerMeter))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.ATTACK_HARD, triggerMeter, towards);
            //Log.Error(LogLevel.Info, "Trigger hard attack+++++++++++++++++++++++++++++++{0}", triggerMeter);
            return true;
        }

        return false;
    }

    protected bool GetRunInputCmd(out AgentInputCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();

        if(!towards.Equals(GamePlayDefine.InputDirection_NONE))
        {
            cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.RUN, MeterManager.Ins.MeterIndex, towards);
            return true;
        }

        return false;
    }
}
