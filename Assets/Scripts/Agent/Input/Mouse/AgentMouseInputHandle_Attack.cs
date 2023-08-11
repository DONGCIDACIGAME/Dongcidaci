using GameEngine;
using UnityEngine;

public class AgentMouseInputHandle_Attack : AgentMouseInputHandle
{
    private Vector3 screenCenter;

    private Vector3 towards;

    public AgentMouseInputHandle_Attack(Hero hero) : base(hero)
    {
        towards = DirectionDef.none;
        screenCenter = new Vector3(Screen.width/2f, Screen.height/2f, 0);
    }

    protected bool GetAttackInputCmd(out AgentCommand cmd)
    {
        cmd = null;
        int triggerMeter = -1;

        if (Input.GetMouseButtonDown(0))
        {
            if (MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out triggerMeter))
            {
                cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();

                Vector3 attackTowards = new Vector3(towards.x, 0, towards.y);

                if (mHero.Hero_View.InstantAttack)
                {
                    cmd.Initialize(AgentCommandDefine.ATTACK_SHORT_INSTANT, triggerMeter, TimeMgr.Ins.FrameIndex, attackTowards);
                }
                else
                {
                    cmd.Initialize(AgentCommandDefine.ATTACK_SHORT, triggerMeter, TimeMgr.Ins.FrameIndex, attackTowards);
                }
                return true;
            }
            else
            {
                Log.Error(LogLevel.Info, "---------------------------Trigger light attack FAILED{0}---------------{1}", triggerMeter, AudioManager.Ins.GetCurBgmTime());
            }
        }

        //if (Input.GetMouseButtonDown(0) && MeterManager.Ins.CheckTriggered(GamePlayDefine.InputCheckTolerance, GamePlayDefine.InputCheckOffset, out triggerMeter))
        //{
        //    cmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
        //    if (mHero.Hero_View.InstantAttack)
        //    {
        //        cmd.Initialize(AgentCommandDefine.ATTACK_LONG_INSTANT, triggerMeter, TimeMgr.Ins.FrameIndex, attackTowards);
        //    }
        //    else
        //    {
        //        cmd.Initialize(AgentCommandDefine.ATTACK_LONG, triggerMeter, TimeMgr.Ins.FrameIndex, attackTowards);
        //    }
        //    //Log.Error(LogLevel.Info, "Trigger hard attack+++++++++++++++++++++++++++++++{0}", triggerMeter);
        //    return true;
        //}

        return false;
    }

    public override string GetHandleName()
    {
        return InputDef.AgentMouseInputHandle_Attack;
    }

    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        
    }

    public override void OnMeterEnd(int meterIndex)
    {
       
    }

    public override void OnMeterEnter(int meterIndex)
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        if (mHero == null)
            return;

        towards = Input.mousePosition - screenCenter;

        GameEventSystem.Ins.Fire("ChangeAttackTowards", towards);

        AgentCommand cmd;
        bool hasCmd = GetAttackInputCmd(out cmd);
        if (hasCmd)
        {
            mHero.OnCommand(cmd);
        }
    }
}
