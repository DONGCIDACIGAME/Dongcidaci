using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AgentStateInfoCenter
{
    private Dictionary<uint, AgentStatusGraph> mAgentStateInfos;

    public AgentStatusGraph GetAgentStatusGraph(uint agentId)
    {
        AgentStatusGraph statesInfo;
        mAgentStateInfos.TryGetValue(agentId, out statesInfo);
        return statesInfo;
    }

    private void LoadAgentStateInfo(string jsonPath)
    {
        if (!FileHelper.FileExist(jsonPath))
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, file don¡®t exist at path:{0}", jsonPath);
            return;
        }

        string json = FileHelper.ReadText(jsonPath, System.Text.Encoding.UTF8);
        AgentStatusGraph agentStates = JsonUtility.FromJson<AgentStatusGraph>(json);
        if (agentStates == null)
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, parse AgentFullStatesInfo data failed at path:{0}", jsonPath);
            return;
        }

        if(!mAgentStateInfos.TryAdd(agentStates.agentId, agentStates))
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, repeated agent id at path:{0}", jsonPath);
        }
    }

    private void LoadAllAgentStateInfos(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
            return;

        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach(DirectoryInfo subDi in di.EnumerateDirectories())
        {
            LoadAllAgentStateInfos(subDi.FullName);
        }

        foreach (FileInfo fi in di.EnumerateFiles("*.json"))
        {
            LoadAgentStateInfo(fi.FullName);
        }
    }

    public void Initialize()
    {
        mAgentStateInfos = new Dictionary<uint, AgentStatusGraph>();
        string dirPath = null;
#if UNITY_EDITOR
        dirPath = PathDefine.EDITOR_DATA_DIR_PATH + "/StateInfo";
#else
        dirPath = PathDefine.RELEASE_DATA_DIR_PATH + "/StateInfo";
#endif
        LoadAllAgentStateInfos(dirPath);
    }

    public void Dispose()
    {
        mAgentStateInfos = null;
    }

}
