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
        Vector3 towards = DirectionDef.none;
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

    protected bool GetDashInputCommand(out AgentCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();
        if (Input.GetKeyDown(InputDef.DashKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.DashMeterCheckTolerance, GamePlayDefine.DashMeterCheckOffset, out int triggerMeter))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.DASH, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            return true;
        }

        return false;
    }

    protected bool GetAttackInputCmd(out AgentCommand cmd)
    {
        cmd = null;
        int triggerMeter = -1;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyDown(InputDef.AttackShortKeyCode))
        {
            if(MeterManager.Ins.CheckTriggered(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset, out triggerMeter))
            {
                cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
                cmd.Initialize(AgentCommandDefine.ATTACK_SHORT, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
                //Log.Error(LogLevel.Info, "Trigger light attack--------------------------------------------------{0}", triggerMeter);
                return true;
            }
            else
            {
                Log.Error(LogLevel.Info, "---------------------------Trigger light attack FAILED{0}---------------{1}", triggerMeter, AudioManager.Ins.GetCurBgmTime());
            }
        }

        if (Input.GetKeyDown(InputDef.AttackLongKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset, out triggerMeter))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.ATTACK_LONG, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            //Log.Error(LogLevel.Info, "Trigger hard attack+++++++++++++++++++++++++++++++{0}", triggerMeter);
            return true;
        }

        return false;
    }

    protected bool GetRunInputCmd(out AgentCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();

        if(!towards.Equals(DirectionDef.none))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.RUN, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
            return true;
        }

        return false;
    }
}
