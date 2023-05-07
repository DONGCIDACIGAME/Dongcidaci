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

            // 是否在输入的容差时间内
            bool inInputTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.AttackMeterCheckTolerance, GamePlayDefine.AttackMeterCheckOffset);
            
            if (inInputTime)
            {
                // 在输入容差时间内，空输入，动画定格状态
                cmd.Initialize(AgentCommandDefine.EMPTY, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            }
            else
            {
                // 超过输入的容差时间，进入idle
                cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            }
        }
        mAgent.OnCommand(cmd);
    }

}

