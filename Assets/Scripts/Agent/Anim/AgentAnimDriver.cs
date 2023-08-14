using GameEngine;

public abstract class AgentAnimDriver
{
    protected Agent mAgent;
    protected AgentAnimStateInfo mCurAnimState;
    

    public AgentAnimDriver(Agent agt)
    {
        mAgent = agt;
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
    }
}