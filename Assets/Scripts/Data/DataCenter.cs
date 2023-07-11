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

    /// <summary>
    /// combo配置中心
    /// </summary>
    public AgentComboGraphCenter AgentComboGraphCenter;

    /// <summary>
    /// 行为树数据中心
    /// </summary>
    public BehaviourTreeCenter BehaviourTreeCenter;


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

        if(AgentComboGraphCenter != null)
        {
            AgentComboGraphCenter.Dispose();
            AgentComboGraphCenter = null;
        }

        if(BehaviourTreeCenter != null)
        {
            BehaviourTreeCenter.Dispose();
            BehaviourTreeCenter = null;
        }
    }

    public override void Initialize()
    {
        // 加载所有配置数据
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        // 加载所有角色状态配置
        AgentStatusGraphCenter = new AgentStautsGraphCenter();
        AgentStatusGraphCenter.Initialize();

        // 加载所有combo配置
        AgentComboGraphCenter = new AgentComboGraphCenter();
        AgentComboGraphCenter.Initialize();

        // 加载所有行为树配置
        BehaviourTreeCenter = new BehaviourTreeCenter();
        BehaviourTreeCenter.Initialize();
    }

    /// <summary>
    /// Added by weng
    /// DataCenter应该根据当前的关卡加载对应的地图数据
    /// </summary>
    /// <returns></returns>
    public GameMapData GetCrtLevelMapData()
    {
        return null;
    }


}