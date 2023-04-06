using UnityEngine;

public class PlayerKeyboardInputControl : IInputControl
{
    public string GetInputControlName()
    {
        return InputDef.PlayerKeyboardInputCtlName;
    }

    private Agent mAgent;

    public void BindAgent(Agent agt)
    {
        this.mAgent = agt;
    }

    private Vector3 lastInputTowards;

    public void InputControlUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        // TODO: 这里需要加个缓存池
        AgentCommandBuffer cmds = mAgent.CommandBufferPool.PopAgentCommandBuffer() ;
        cmds.AddCommand(AgentCommandDefine.IDLE);

        Vector3 towards = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            towards += DirectionDef.up;
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        if (Input.GetKey(KeyCode.S))
        {
            towards += DirectionDef.down;
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        if (Input.GetKey(KeyCode.A))
        {
            towards += DirectionDef.left;
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        if (Input.GetKey(KeyCode.D))
        {
            towards += DirectionDef.right;
            cmds.AddCommand(AgentCommandDefine.RUN);
        }

        if(Input.GetKeyDown(KeyCode.K) && MeterManager.Ins.CheckTriggerBaseMeter())
        {
            cmds.AddCommand(AgentCommandDefine.ATTACK_HARD);
        }

        if(!towards.Equals(lastInputTowards))
        {
            mAgent.MoveControl.TurnTo(towards);
            lastInputTowards = towards;
        }

        if(!towards.Equals(Vector3.zero))
        {
            mAgent.MoveControl.Move(towards, deltaTime);
        }

        mAgent.OnCommands(cmds);
    }

    public void InputControlOnMeter(int meterIndex)
    {

    }
}
