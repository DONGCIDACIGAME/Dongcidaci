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
                return asi.animStates[i].meterLen;
            }
        }

        Log.Error(LogLevel.Normal, "GetAgentStateMeterLen Failed,agent Id:{0} , status name:{1},  don't have a state named:{2} ", agt.GetAgentId(), statusName, stateName);
        return 0;
    }

    public static AgentAnimStateInfo GetAgentAnimStateInfo(Agent agt, string statusName, string stateName)
    {
        AgentStatusInfo statusInfo = GetAgentStatusInfo(agt, statusName);
        if (statusInfo == null)
            return null;

        if (statusInfo.animStates == null || statusInfo.animStates.Length == 0)
        {
            Log.Error(LogLevel.Normal, "GetStateInfo Failed, statusInfo.animStates is null or empty!");
            return null;
        }

        for (int i = 0; i < statusInfo.animStates.Length; i++)
        {
            AgentAnimStateInfo stateInfo = statusInfo.animStates[i];
            if (stateInfo.stateName == stateName)
            {
                return stateInfo;
            }
        }

        return null;
    }

    public static AgentStatusInfo GetAgentStatusInfo(Agent agt, string statusName)
    {
        if(agt == null)
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, agt is null!");
            return null;
        }

        if (agt.StatusGraph == null)
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, StatusGraph is null!");
            return null;
        }

        if (agt.StatusGraph.statusMap == null)
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, StatusGraph.statusMap is null!");
            return null;
        }

        AgentStatusInfo statusInfo;
        if (!agt.StatusGraph.statusMap.TryGetValue(statusName, out statusInfo))
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, no status info named {0}", statusName);
        }

        return statusInfo;
    }

    public static AgentActionData GetAgentDefaultStatusActionData(Agent agt, string statusName)
    {
        AgentStatusInfo statusInfo = GetAgentStatusInfo(agt, statusName);

        if(statusInfo == null)
        {
            Log.Error(LogLevel.Normal, "GetAgentDefaultStatusActionData Failed, agent id:{0}, statusName:{1}",agt.GetAgentId(), statusName);
            return null;
        }

        return statusInfo.defaultAciton;
    }

    public static float GetEntityDistance(MapEntity entity1, MapEntity entity2)
    {
        if (entity1 == null || entity2 == null)
        {
            Log.Error(LogLevel.Normal, "GetEntityDistance Error, has null entity!");
            return 0;
        }

        return (entity2.GetPosition() - entity1.GetPosition()).magnitude;
    }
}
