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
            
            // 是否在静止的容差时间内
            bool inEmptyTime = MeterManager.Ins.IsInMeterWithTolerance(MeterManager.Ins.MeterIndex, GamePlayDefine.EmptyStatusMaxTime, GamePlayDefine.EmptyStatusMaxTime / 2f);
            
            if (inInputTime && inEmptyTime)
            {
                // 这里有问题，如果指令中有idle，如果atk不是在节拍处处理，则在节拍处一定会回一次idle，再进来的话combo又从新开始了
                // 要做两件事，一是把combohandler挪到agent里，二是这里的idle添加规则需要再思考一下，或者这里只是负责输入的，那么这里是不管status的处理逻辑，status里需要做特殊处理
                cmd.Initialize(AgentCommandDefine.EMPTY, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            }
            else
            {
                cmd.Initialize(AgentCommandDefine.IDLE, MeterManager.Ins.MeterIndex, GamePlayDefine.InputDirection_NONE);
            }
        }
        mAgent.OnCommand(cmd);
    }

}

