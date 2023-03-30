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

        if(!towards.Equals(lastInputTowards))
        {
            mAgent.MoveControl.TurnTo(towards);
            lastInputTowards = towards;
        }

        if(!towards.Equals(Vector3.zero))
        {
            mAgent.MoveControl.Move(towards, deltaTime);
        }

        mAgent.OnAction(agentAction);
    }
}
