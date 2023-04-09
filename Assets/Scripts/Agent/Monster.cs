public class Monster : Agent
{
    public Monster(uint agentId) : base(agentId)
    {
        
    }

    protected override void CustomInitialize()
    {
        mBaseMeterHandler = new MonsterBaseMeterHandler(this);
    }

    protected override void LoadAgentCfg(uint agentId)
    {
        
    }

    protected override void LoadAgentGo()
    {
        throw new System.NotImplementedException();
    }
}
