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
        // ����������������
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        AgentStatusGraphCenter = new AgentStautsGraphCenter();
        AgentStatusGraphCenter.Initialize();
    }
}