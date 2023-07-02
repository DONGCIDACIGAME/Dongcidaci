using UnityEngine;

public class Monster : Agent
{

    /// <summary>
    /// 英雄配置数据
    /// </summary>
    private DongciDaci.MonsterBaseCfg mMonsterCfg;

    /// <summary>
    /// 怪物表现层
    /// </summary>
    private MonsterView mMonsterView;

    // AI行为树
    public BTTree BehaviourTree;

    public Monster(uint agentId) : base(agentId)
    { 

    }


    public override void Dispose()
    {
        base.Dispose();

        if(BehaviourTree != null)
        {
            BehaviourTree.Dispose();
        }

        // 取消节拍处理
        MeterManager.Ins.UnregiseterMeterHandler(this);
    }

    protected override void LoadAgentCfg(uint agentId)
    {
        DongciDaci.MonsterBaseCfg cfg = null;
        DongciDaci.MonsterCfg_Data data = ConfigDatabase.Get<DongciDaci.MonsterCfg_Data>();
        if (data != null)
        {
            var cfgs = data.MonsterBaseCfgItems;
            if (cfgs.ContainsKey(agentId))
            {
                cfg = cfgs[agentId];
            }
        }

        mMonsterCfg = cfg;
    }

    private void BindMonsterView(MonsterView monsterView)
    {
        BindAgentView(monsterView);
        mMonsterView = monsterView;
    }

    protected override void LoadAgentView()
    {
        if (mMonsterCfg == null)
        {
            Log.Error(LogLevel.Critical, "LoadAgentView Failed, mMonsterCfg is null or empty!");
            return;
        }

        if (string.IsNullOrEmpty(mMonsterCfg.Prefab))
        {
            Log.Error(LogLevel.Critical, "LoadAgentView Failed, path is null or empty!");
            return;
        }

        var go = PrefabUtil.LoadPrefab(mMonsterCfg.Prefab, AgentManager.Ins.GetMonsterNode(), "Load Agent Prefab");
        if (go != null)
        {
            MonsterView monsterView = go.GetComponent<MonsterView>();
            BindMonsterView(monsterView);
        }
    }

    protected override void CustomInitialize()
    {
        // 位置初始化
        SetPosition(new Vector3(22f, 0.2f, 5f));
        // 朝向初始化
        SetRotation(Vector3.zero);
        // 缩放初始化
        SetScale(Vector3.one);

        MoveControl = new MonsterMoveControl(this);
        SetSpeed(mMonsterCfg.Speed);
        SetDashDistance(mMonsterCfg.DashDistance);
        SetName(mMonsterCfg.Name);

        //Combo_Trigger.SetComboActive("JJJ", true);
        //Combo_Trigger.SetComboActive("DashAttack", true);


        // 加载行为树
        BehaviourTree = BehaviourTreeManager.Ins.LoadTreeWithTreeName(mMonsterCfg.BehaviourTree);
        //BehaviourTree = BehaviourTreeManager.Ins.LoadTreeWithTreeName("Bat2");
        BehaviourTree.Initialize(this, new System.Collections.Generic.Dictionary<string,object>());

    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (BehaviourTree != null)
        {
            BehaviourTree.Excute(deltaTime);
        }
    }

    public override void OnMeterEnter(int meterIndex)
    {
        base.OnMeterEnter(meterIndex);
    }


    public override int GetEntityType()
    {
        return EntityTypeDefine.Monster;
    }


    protected override MyColliderType ColliderType => MyColliderType.Collider_Monster;
    /**
    protected override int GetColliderType()
    {
        return GameColliderDefine.ColliderType_Monster;
    }

    
    protected override IGameColliderHandler GetColliderHanlder()
    {
        return new MonsterColliderHandler(this);
    }
    */

}
