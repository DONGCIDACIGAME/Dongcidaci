public class Monster : Agent
{
    // AI行为树
    private BTTree mBehaviourTree;

    public Monster(uint agentId) : base(agentId){ Initialize(); }


    public override void Dispose()
    {
        base.Dispose();

        if(mBehaviourTree != null)
        {
            mBehaviourTree.Dispose();
        }

        // 取消节拍处理
        MeterManager.Ins.UnregiseterMeterHandler(this);
    }

    protected override void LoadAgentCfg(uint agentId)
    {

    }

    protected override void LoadAgentView()
    {
        
    }

    protected BTTree LoadBTTree(string BTTreeFileName)
    {
        return null;
    }

    protected override void CustomInitialize()
    {
        // 加载行为树
        mBehaviourTree = LoadBTTree(string.Empty);

        // 注册节拍处理
        MeterManager.Ins.RegisterMeterHandler(this);
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if(mBehaviourTree != null)
        {
            mBehaviourTree.Excute(deltaTime);
        }
    }

    public override int GetEntityType()
    {
        return EntityTypeDefine.Monster;
    }

    public override void HandleCollideTo(ICollideProcessor tgtColliderProcessor)
    {
        // do something here

    }
}
