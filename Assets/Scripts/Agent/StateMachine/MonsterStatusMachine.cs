public class MonsterStatusMachine : AgentStatusMachine
{
    public override void Initialize(Agent agt)
    {
        base.Initialize(agt);

        AddStatus(AgentStatusDefine.IDLE, new MonsterStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new MonsterStatus_Run());
        AddStatus(AgentStatusDefine.METER_ATTACK, new MonsterStatus_MeterAttack());
        AddStatus(AgentStatusDefine.DASH, new MonsterStatus_Dash());
        AddStatus(AgentStatusDefine.BEHIT, new MonsterStatus_Behit());
    }
}
