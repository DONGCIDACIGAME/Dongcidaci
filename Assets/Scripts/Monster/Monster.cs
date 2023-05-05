public class Monster : Agent
{
    public Monster(uint agentId) : base(agentId){ Initialize(); }

    /// <summary>
    /// ≥ı ºªØ
    /// </summary>
    public override void Initialize()
    {
        LoadAgentCfg(mAgentId);
        LoadAgentGo();
        CustomInitialize();
        //StatusGraph = DataCenter.Ins.AgentStatusGraphCenter.GetAgentStatusGraph(mAgentId);
        //StatusMachine = new AgentStatusMachine();
        //StatusMachine.Initialize(this);
        MeterManager.Ins.RegisterMeterHandler(this);
    }

    protected override void LoadAgentCfg(uint agentId)
    {

    }

    protected override void LoadAgentGo()
    {
        
    }

    protected override void CustomInitialize()
    {
        
    }

    public override void OnMeter(int meterIndex)
    {
        
    }






}
