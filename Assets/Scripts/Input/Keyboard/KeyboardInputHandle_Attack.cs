using UnityEngine;

public class KeyboardInputHandle_Attack : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Attack(Agent agt) : base(agt)
    {

    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Attack;
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

            //float tolerance = Mathf.Min(GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.EmptyStatusMaxTime);
            //bool inMeterTrigger = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, tolerance, 0);
            //if (inMeterTrigger)
            //{
            //    cmd.Initialize(AgentCommandDefine.EMPTY, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            //}
            //else
            //{
                cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            //}
        }
        mAgent.OnCommand(cmd);
    }
}

