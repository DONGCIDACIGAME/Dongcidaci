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
    public AgentStateInfoCenter AgentStateInfoCenter;


    public override void Dispose()
    {
        if (TableDataCenter != null)
        {
            TableDataCenter.Dispose();
            TableDataCenter = null;
        }
        
        if(AgentStateInfoCenter != null)
        {
            AgentStateInfoCenter.Dispose();
            AgentStateInfoCenter = null;
        }
    }

    public override void Initialize()
    {
        // 加载所有配置数据
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        AgentStateInfoCenter = new AgentStateInfoCenter();
        AgentStateInfoCenter.Initialize();
    }
}