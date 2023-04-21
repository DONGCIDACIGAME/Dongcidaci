﻿public class AgentAnimDriver
{
    protected Agent mAgent;
    protected AgentAnimStateInfo mCurAnimState;
    protected AgentAnimStateInfo[] mAnimStates;

    public AgentAnimDriver(Agent agt, string statusName)
    {
        mAgent = agt;
        if(agt != null)
        {
            AgentStatusInfo statusInfo = agt.GetStatusInfo(statusName);
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


}