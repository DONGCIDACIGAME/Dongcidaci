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

        cmds.AddCommand(AgentCommandDefine.IDLE);

        // 检测到移动的按键
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // 添加移动指令
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        if (Input.GetKeyDown(KeyCode.K) && MeterManager.Ins.CheckTriggerCurrentMeter())
        {
            cmds.AddCommand(AgentCommandDefine.ATTACK_HARD);
        }

        mAgent.OnCommands(cmds);
    }
}

