public static class AgentHelper 
{
    public static int GetAgentStateMeterLen(Agent agt, string statusName, string stateName)
    {
        if (agt == null)
        {
            Log.Error(LogLevel.Normal, "GetAgentStateMeterLen Error, Agent is null!");
            return 0;
        }

        if (agt.StatusGraph == null)
        {
            Log.Error(LogLevel.Normal, "GetAgentStateMeterLen Error, AgentStatusGraph is null, agent Id:{0}!",agt.GetAgentId());
            return 0;
        }

        if(!agt.StatusGraph.statusMap.TryGetValue(statusName, out AgentStatusInfo asi))
        {
            Log.Error(LogLevel.Normal, "GetAgentStateMeterLen Failed, agent Id:{0} don't have a status named {1}!", agt.GetAgentId(), statusName);
            return 0;
        }
        
        for(int i = 0; i<asi.animStates.Length;i++)
        {
            if(asi.animStates[i].stateName.Equals(stateName))
            {
                return asi.animStates[i].stateMeterLen;
            }
        }

        Log.Error(LogLevel.Normal, "GetAgentStateMeterLen Failed,agent Id:{0} , status name:{1},  don't have a state named:{2} ", agt.GetAgentId(), statusName, stateName);
        return 0;
    }
}
