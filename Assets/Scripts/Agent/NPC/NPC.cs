using UnityEngine;
public class NPC : Agent
{
    public NPC(uint agentId) : base(agentId)
    {
    }

    protected override MyColliderType ColliderType => MyColliderType.Collider_NPC;

    public override int GetEntityType()
    {
        return EntityTypeDefine.NPC;
    }

    protected override void CustomInitialize(Vector3 initPos)
    {
        
    }

    protected override void LoadAgentCfg(uint agentId)
    {
        
    }

    protected override void LoadAgentView()
    {
        
    }
}
