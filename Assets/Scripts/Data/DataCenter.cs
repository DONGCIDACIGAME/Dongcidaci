using GameEngine;

public class DataCenter : ModuleManager<DataCenter>
{
    /// <summary>
    /// �����������
    /// </summary>
    public ConfigDatabase TableDataCenter;

    /// <summary>
    /// ��ɫ״̬������������
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
        // ����������������
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        AgentStateInfoCenter = new AgentStateInfoCenter();
        AgentStateInfoCenter.Initialize();
    }
}