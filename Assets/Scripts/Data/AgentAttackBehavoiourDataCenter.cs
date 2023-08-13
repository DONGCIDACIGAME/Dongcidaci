using LitJson;
using System.Collections.Generic;
using System.IO;

public class AgentAttackBehavoiourDataCenter 
{
    private Dictionary<uint, AttackBehaviourData> mAllAtkBehaviourDatas;

    public AttackBehaviourData GetAgentAtkBehaviourData(uint agentId)
    {
        if(mAllAtkBehaviourDatas.TryGetValue(agentId, out AttackBehaviourData atkBehaviourData))
        {
            return atkBehaviourData;
        }

        return null;
    }

    private void LoadAgentAtkBehaviourData(string jsonPath)
    {
        if (!FileHelper.FileExist(jsonPath))
        {
            Log.Error(LogLevel.Normal, "LoadAgentAtkBehaviourData Failed, file don‘t exist at path:{0}", jsonPath);
            return;
        }

        string json = FileHelper.ReadText(jsonPath, System.Text.Encoding.UTF8);

        AttackBehaviourData atkBehaviourData = JsonMapper.ToObject<AttackBehaviourData>(json);
        if (atkBehaviourData == null)
        {
            Log.Error(LogLevel.Normal, "LoadAgentAtkBehaviourData Failed, parse AttackBehaviourData failed at path:{0}", jsonPath);
            return;
        }

        if (!mAllAtkBehaviourDatas.TryAdd(atkBehaviourData.agentId, atkBehaviourData))
        {
            Log.Error(LogLevel.Normal, "LoadAgentAtkBehaviourData Failed, repeated agent id at path:{0}", jsonPath);
        }
    }

    private void LoadAllAgentAtkBehaviourData(string dirPath)
    {
        if (string.IsNullOrEmpty(dirPath))
            return;

        DirectoryInfo di = new DirectoryInfo(dirPath);
        foreach (DirectoryInfo subDi in di.EnumerateDirectories())
        {
            LoadAllAgentAtkBehaviourData(subDi.FullName);
        }

        foreach (FileInfo fi in di.EnumerateFiles("*.json"))
        {
            LoadAgentAtkBehaviourData(fi.FullName);
        }
    }

    public void Initialize()
    {
        mAllAtkBehaviourDatas = new Dictionary<uint, AttackBehaviourData>();
        string dirPath = null;
#if UNITY_EDITOR
        dirPath = PathDefine.EDITOR_DATA_DIR_PATH + "/AtkBehaviourData";
#else
        dirPath = PathDefine.RELEASE_DATA_DIR_PATH + "/AtkBehaviourData";
#endif
        LoadAllAgentAtkBehaviourData(dirPath);
    }

    public void Dispose()
    {
        mAllAtkBehaviourDatas = null;
    }
}
