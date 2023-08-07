public class HeroStatusMachine : AgentStatusMachine
{
    public override void Initialize(Agent agt)
    {
        base.Initialize(agt);

        AddStatus(AgentStatusDefine.IDLE, new HeroStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new HeroStatus_Run());
        AddStatus(AgentStatusDefine.ATTACK, new HeroStatus_Attack());
        AddStatus(AgentStatusDefine.INSTANT_ATTACK, new HeroStatus_InstantAttack());
        AddStatus(AgentStatusDefine.DASH, new HeroStatus_Dash());
        AddStatus(AgentStatusDefine.BEHIT, new HeroStatus_Behit());
        AddStatus(AgentStatusDefine.TRANSITION, new HeroStatus_Transition());
    }
}
