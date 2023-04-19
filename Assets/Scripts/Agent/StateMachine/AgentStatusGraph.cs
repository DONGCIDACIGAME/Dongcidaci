using System.Collections.Generic;

[System.Serializable]
public class AgentStatusGraph
{
    public string agentName;
    public uint agentId;
    public Dictionary<string, AgentStatusInfo> statusMap;
}
