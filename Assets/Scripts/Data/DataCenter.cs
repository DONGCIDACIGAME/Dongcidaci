using GameEngine;

public class DataCenter : ModuleManager<DataCenter>
{
    /// <summary>
    /// 配表数据中心
    /// </summary>
    public ConfigDatabase TableDataCenter;

    /// <summary>
    /// 角色状态数据配置中心
    /// </summary>
    public AgentStautsGraphCenter AgentStatusGraphCenter;


    public override void Dispose()
    {
        if (TableDataCenter != null)
        {
            TableDataCenter.Dispose();
            TableDataCenter = null;
        }
        
        if(AgentStatusGraphCenter != null)
        {
            AgentStatusGraphCenter.Dispose();
            AgentStatusGraphCenter = null;
        }
    }

    public override void Initialize()
    {
        // 加载所有配置数据
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        AgentStatusGraphCenter = new AgentStautsGraphCenter();
        AgentStatusGraphCenter.Initialize();
    }
}