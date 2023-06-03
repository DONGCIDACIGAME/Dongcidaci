public class Monster : Agent
{
    public Monster(uint agentId) : base(agentId){ Initialize(); }

    /// <summary>
    /// 重写初始化
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        LoadAgentCfg(mAgentId);
        LoadAgentGo();
        CustomInitialize();
        MeterManager.Ins.RegisterMeterHandler(this);
    }

    public override void Dispose()
    {
        base.Dispose();

        MeterManager.Ins.UnregiseterMeterHandler(this);
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

    public override int GetEntityType()
    {
        return EntityTypeDefine.Monster;
    }
}
