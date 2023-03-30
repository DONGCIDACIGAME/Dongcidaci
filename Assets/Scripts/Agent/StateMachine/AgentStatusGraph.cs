using System.Collections.Generic;

[System.Serializable]
public class AgentStatusGraph
{
    public string agentName;
    public uint agentId;
    public Dictionary<string, AgentStatusInfo> statusMap;

    public AgentStatusInfo GetStatusInfo(string statusName)
    {
        AgentStatusInfo statusInfo;
        if(!statusMap.TryGetValue(statusName, out statusInfo))
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, no status info named {0}", statusName);
        }

        return statusInfo;
    }
}
