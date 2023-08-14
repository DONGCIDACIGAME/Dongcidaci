public class HeroStatusMachine : AgentStatusMachine
{
    public override void Initialize(Agent agt)
    {
        base.Initialize(agt);

        AddStatus(AgentStatusDefine.IDLE, new HeroStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new HeroStatus_Run());
        AddStatus(AgentStatusDefine.INSTANT_ATTACK, new HeroStatus_InstantAttack());
        AddStatus(AgentStatusDefine.METER_ATTACK, new HeroStatus_MeterAttack());
        AddStatus(AgentStatusDefine.CHARGING, new HeroStatus_Charging());
        AddStatus(AgentStatusDefine.CHARGING_ATTACK, new HeroStatus_ChargingAttack());
        AddStatus(AgentStatusDefine.DASH, new HeroStatus_Dash());
        AddStatus(AgentStatusDefine.BEHIT, new HeroStatus_Behit());
        AddStatus(AgentStatusDefine.TRANSITION, new HeroStatus_Transition());
    }
}
