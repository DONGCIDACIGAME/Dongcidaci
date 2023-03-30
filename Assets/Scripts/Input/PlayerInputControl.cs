using UnityEngine;

public class PlayerInputControl : IInputControl
{
    public string GetInputControlName()
    {
        return InputDef.PlayerInputCtlName;
    }

    private Agent mAgent;
    public void Initialize()
    {
        
    }

    public void BindAgent(Agent agt)
    {
        mAgent = agt;
    }

    public void InputControlUpdate(float deltaTime)
    {
        int agentAction = AgentActionDefine.IDLE;

        Vector3 towards = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            towards += DirectionDef.up;
            agentAction = AgentActionDefine.RUN;
        }

        if (Input.GetKey(KeyCode.S))
        {
            towards += DirectionDef.down;
            agentAction = AgentActionDefine.RUN;
        }

        if (Input.GetKey(KeyCode.A))
        {
            towards += DirectionDef.left;
            agentAction = AgentActionDefine.RUN;
        }

        if (Input.GetKey(KeyCode.D))
        {
            towards += DirectionDef.right;
            agentAction = AgentActionDefine.RUN;
        }

        if(!towards.Equals(Vector3.zero))
        {
            mAgent.MoveControl.TurnTo(towards);
            mAgent.MoveControl.Move(towards, deltaTime);
        }

        mAgent.OnAction(agentAction);
    }
}
