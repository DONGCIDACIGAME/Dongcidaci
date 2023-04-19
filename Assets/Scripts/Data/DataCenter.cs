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

    /// <summary>
    /// combo��������
    /// </summary>
    public AgentComboGraphCenter AgentComboGraphCenter;


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
    }

    public override void Initialize()
    {
        // ����������������
        TableDataCenter = ConfigDatabase.CreateInstance("DongciDaci");
        TableDataCenter.LoadFromLocalResources();

        // �������н�ɫ״̬����
        AgentStatusGraphCenter = new AgentStautsGraphCenter();
        AgentStatusGraphCenter.Initialize();

        // ��������combo����
        AgentComboGraphCenter = new AgentComboGraphCenter();
        AgentComboGraphCenter.Initialize();
    }
}