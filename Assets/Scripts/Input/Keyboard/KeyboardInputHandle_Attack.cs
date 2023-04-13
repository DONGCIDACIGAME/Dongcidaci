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

        AgentCommandBuffer cmds = mAgent.CommandBufferPool.PopAgentCommandBuffer();

        bool inMeterTrigger = MeterManager.Ins.IsInMeterTrigger();
        if(inMeterTrigger)
        {
            cmds.AddCommand(AgentCommandDefine.EMPTY);

            if (Input.GetKeyDown(KeyCode.K))
            {
                cmds.AddCommand(AgentCommandDefine.ATTACK_HARD);
            }
        }
        else
        {
            cmds.AddCommand(AgentCommandDefine.IDLE);

        }

        // 检测到移动的按键
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // 添加移动指令
            cmds.AddCommand(AgentCommandDefine.RUN);
        }



        mAgent.OnCommands(cmds);
    }
}

