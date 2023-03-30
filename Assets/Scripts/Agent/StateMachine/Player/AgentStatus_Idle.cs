using System.Collections.Generic;

public class AgentStatus_Idle : AgentStatus
{
    public override string GetStatusName()
    {
        return AgentStatusDefine.IDLE;
    }

    public override void OnAction(int action)
    {
        if(action == AgentActionDefine.IDLE)
        {
            return;
        }

        if(action == AgentActionDefine.RUN)
        {
            ChangeStatus(AgentStatusDefine.RUN);
        }
    }

    public override void OnEnter(Dictionary<string, object> context)
    {
        if (mAgent == null)
            return;

        

        //mAgent.AnimPlayer.CrossFadeToStateInNormalizedTime(AgentStatusDefine.IDLE,)
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate(float deltaTime)
    {
        
    }
}
