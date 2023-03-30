public class Monster : Agent
{
    public Monster(uint agentId) : base(agentId)
    {
        mBaseMeterHandler = new MonsterBaseMeterHandler(this);
        MeterManager.Ins.RegisterBaseMeterHandler(mBaseMeterHandler);
    }

    protected override void CustomInitialize()
    {
        throw new System.NotImplementedException();
    }

    protected override void LoadAgentCfg(uint agentId)
    {
        
    }

    protected override void LoadAgentGo()
    {
        throw new System.NotImplementedException();
    }
}
