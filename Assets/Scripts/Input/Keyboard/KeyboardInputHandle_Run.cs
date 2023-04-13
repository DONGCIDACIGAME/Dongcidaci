using UnityEngine;

public class KeyboardInputHandle_Run : AgentKeyboardInputHandle
{
    public KeyboardInputHandle_Run(Agent agt) : base(agt)
    {
    }

    public override string GetHandleName()
    {
        return InputDef.KeyboardInputHandle_Run;
    }

    public override void OnMeter(int meterIndex)
    {
        
    }

    private Vector3 lastInputTowards;

    public override void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        AgentCommandBuffer cmds = mAgent.CommandBufferPool.PopAgentCommandBuffer();
        cmds.AddCommand(AgentCommandDefine.EMPTY);
        bool hasCmd = false;
        Vector3 towards = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            towards += DirectionDef.up;
            cmds.AddCommand(AgentCommandDefine.RUN);
            hasCmd = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            towards += DirectionDef.down;
            cmds.AddCommand(AgentCommandDefine.RUN);
            hasCmd = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            towards += DirectionDef.left;
            cmds.AddCommand(AgentCommandDefine.RUN);
            hasCmd = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            towards += DirectionDef.right;
            cmds.AddCommand(AgentCommandDefine.RUN);
            hasCmd = true;
        }

        if (Input.GetKeyDown(KeyCode.K) && MeterManager.Ins.CheckTriggerCurrentMeter())
        {
            cmds.AddCommand(AgentCommandDefine.ATTACK_HARD);
            hasCmd = true;
        }

        if(!hasCmd)
        {
            cmds.AddCommand(AgentCommandDefine.IDLE);
        }

        if (!towards.Equals(lastInputTowards))
        {
            mAgent.MoveControl.TurnTo(towards);
            lastInputTowards = towards;
        }

        mAgent.OnCommands(cmds);
    }
}
