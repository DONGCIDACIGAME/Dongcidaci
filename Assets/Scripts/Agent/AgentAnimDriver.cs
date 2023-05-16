public class AgentAnimDriver
{
    protected Agent mAgent;
    protected AgentAnimStateInfo mCurAnimState;
    protected AgentAnimStateInfo[] mAnimStates;

    public AgentAnimDriver(Agent agt, string statusName)
    {
        mAgent = agt;
        if(agt != null)
        {
            AgentStatusInfo statusInfo = AgentHelper.GetAgentStatusInfo(agt, statusName);
            if(statusInfo != null)
            {
                mAnimStates = statusInfo.animStates;
            }
        }
        mCurAnimState = null;
    }

    public AgentAnimStateInfo GetCurAnimState()
    {
        return mCurAnimState;
    }

    public virtual void Dispose()
    {
        mAgent = null;
        mCurAnimState = null;
        mAnimStates = null;
    }
}