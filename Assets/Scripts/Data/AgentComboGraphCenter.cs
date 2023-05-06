using LitJson;
using System.Collections.Generic;
using System.IO;

public class AgentComboGraphCenter 
{
    private Dictionary<uint, ComboDataGraph> mAllAgentComboGraphs;

    public ComboDataGraph GetAgentComboGraph(uint agentId)
    {
        ComboDataGraph comboGraph;
        mAllAgentComboGraphs.TryGetValue(agentId, out comboGraph);
        return comboGraph;
    }

    private void LoadAgentComboGraph(string jsonPath)
    {
        if (!FileHelper.FileExist(jsonPath))
        {
            Log.Error(LogLevel.Normal, "LoadAgentComboGraph Failed, file don‘t exist at path:{0}", jsonPath);
            return;
        }

        string json = FileHelper.ReadText(jsonPath, System.Text.Encoding.UTF8);

        ComboDataGraph comboGraph = JsonMapper.ToObject<ComboDataGraph>(json);
        if (comboGraph == null)
        {
            Log.Error(LogLevel.Normal, "LoadAgentComboGraph Failed, parse combo graph data failed at path:{0}", jsonPath);
            return;
        }

        if (!mAllAgentComboGraphs.TryAdd(comboGraph.agentId, comboGraph))
        {
            Log.Error(LogLevel.Normal, "LoadAgentComboGraph Failed, repeated agent id at path:{0}", jsonPath);
        }
    }

    private void LoadAllAgentComboGraphs(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
            return;

        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach (DirectoryInfo subDi in di.EnumerateDirectories())
        {
            LoadAllAgentComboGraphs(subDi.FullName);
        }

        foreach (FileInfo fi in di.EnumerateFiles("*.json"))
        {
            LoadAgentComboGraph(fi.FullName);
        }
    }

    public void Initialize()
    {
        mAllAgentComboGraphs = new Dictionary<uint, ComboDataGraph>();
        string dirPath = null;
#if UNITY_EDITOR
        dirPath = PathDefine.EDITOR_DATA_DIR_PATH + "/Combo";
#else
        dirPath = PathDefine.RELEASE_DATA_DIR_PATH + "/Combo";
#endif
        LoadAllAgentComboGraphs(dirPath);
    }

    public void Dispose()
    {
        mAllAgentComboGraphs = null;
    }
}
