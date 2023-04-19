using System.Collections.Generic;
using System.IO;
using LitJson;

public class AgentStautsGraphCenter
{
    private Dictionary<uint, AgentStatusGraph> mAllAgentStatusGraphs;

    public AgentStatusGraph GetAgentStatusGraph(uint agentId)
    {
        AgentStatusGraph statesInfo;
        mAllAgentStatusGraphs.TryGetValue(agentId, out statesInfo);
        return statesInfo;
    }

    private void LoadAgentStatusData(string jsonPath)
    {
        if (!FileHelper.FileExist(jsonPath))
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, file don¡®t exist at path:{0}", jsonPath);
            return;
        }
        
        string json = FileHelper.ReadText(jsonPath, System.Text.Encoding.UTF8);
        
        AgentStatusGraph agentStatusGraph = JsonMapper.ToObject<AgentStatusGraph>(json);
        if (agentStatusGraph == null)
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, parse AgentFullStatesInfo data failed at path:{0}", jsonPath);
            return;
        }

        if(!mAllAgentStatusGraphs.TryAdd(agentStatusGraph.agentId, agentStatusGraph))
        {
            Log.Error(LogLevel.Normal, "LoadAgentStateInfo Failed, repeated agent id at path:{0}", jsonPath);
        }
    }

    private void LoadAllAgentStatusDatas(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
            return;

        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach(DirectoryInfo subDi in di.EnumerateDirectories())
        {
            LoadAllAgentStatusDatas(subDi.FullName);
        }

        foreach (FileInfo fi in di.EnumerateFiles("*.json"))
        {
            LoadAgentStatusData(fi.FullName);
        }
    }

    public void Initialize()
    {
        mAllAgentStatusGraphs = new Dictionary<uint, AgentStatusGraph>();
        string dirPath = null;
#if UNITY_EDITOR
        dirPath = PathDefine.EDITOR_DATA_DIR_PATH + "/AgentStatusData";
#else
        dirPath = PathDefine.RELEASE_DATA_DIR_PATH + "/AgentStatusData";
#endif
        LoadAllAgentStatusDatas(dirPath);
    }

    public void Dispose()
    {
        mAllAgentStatusGraphs = null;
    }

}
