using UnityEngine;

public abstract class AgentKeyboardInputHandle : InputHandle
{
    protected Hero mHero;
    public AgentKeyboardInputHandle(Hero hero)
    {
        mHero = hero;
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
        if (Input.GetKeyDown(InputDef.DashKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out int triggerMeter))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.DASH, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            return true;
        }

        return false;
    }

    protected bool GetInstantAttackInputCmd(out AgentCommand cmd)
    {
        cmd = null;
        int triggerMeter = -1;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyDown(InputDef.InstantAttackKeyCode))
        {
            if(MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out triggerMeter))
            {
                cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
                cmd.Initialize(AgentCommandDefine.INSTANT_ATTACK, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
                return true;
            }
            else
            {
                Log.Error(LogLevel.Info, "---------------------------Trigger light attack FAILED{0}---------------{1}", triggerMeter, AudioManager.Ins.GetCurBgmTime());
            }
        }

        return false;
    }

    protected bool GetChargingCmd(out AgentCommand cmd)
    {
        cmd = null;
        int triggerMeter = -1;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyDown(InputDef.ChargingAttackKeyCode) && MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out triggerMeter))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            cmd.Initialize(AgentCommandDefine.CHARGING, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            //Log.Error(LogLevel.Info, "Trigger hard attack+++++++++++++++++++++++++++++++{0}", triggerMeter);
            return true;
        }

        return false;
    }

    protected bool GetChargingAttackCmd(out AgentCommand cmd)
    {
        cmd = null;
        //int triggerMeter = -1;
        Vector3 towards = GetInputDirection();

        if (Input.GetKeyUp(InputDef.ChargingAttackKeyCode))
        {
            cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
            //if(MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out triggerMeter))
            //{
            //    cmd.Initialize(AgentCommandDefine.CHARGING_ATTACK, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            //}
            //else
            //{
            //    cmd.Initialize(AgentCommandDefine.CHARGING_ATTACKFAILED, triggerMeter, TimeMgr.Ins.FrameIndex, towards);
            //}
            cmd.Initialize(AgentCommandDefine.CHARGING_ATTACK, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
            //Log.Error(LogLevel.Info, "Trigger hard attack+++++++++++++++++++++++++++++++{0}", triggerMeter);
            return true;
        }

        return false;
    }


    private Vector3 GetInputDirectionOnKeyDown()
    {
        Vector3 towards = DirectionDef.none;
        if (Input.GetKeyDown(KeyCode.W))
        {
            towards += DirectionDef.up;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            towards += DirectionDef.down;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            towards += DirectionDef.left;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            towards += DirectionDef.right;
        }

        return towards;
    }


    protected bool GetRunInputCmd(out AgentCommand cmd)
    {
        cmd = null;
        Vector3 towards = GetInputDirection();
        if (towards.Equals(DirectionDef.none))
            return false;

        cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        cmd.Initialize(AgentCommandDefine.RUN, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, towards);
        return true;
    }



    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        
    }

    public override void OnMeterEnter(int meterIndex)
    {
        
    }

    public override void OnMeterEnd(int meterIndex)
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mHero == null)
            return;
        AgentCommand cmd;
        bool hasCmd = GetInstantAttackInputCmd(out cmd)
            || GetChargingCmd(out cmd)
            || GetDashInputCommand(out cmd) 
            || GetRunInputCmd(out cmd);
        if (hasCmd)
        {
            mHero.OnCommand(cmd);
        }
    }
}
