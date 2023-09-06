using UnityEngine;
using GameSkillEffect;
using Unity.AI.Navigation;
using UnityEngine.AI;
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

    public float m_Range = 100.0f;
    UnityEngine.AI.NavMeshAgent m_Agent;
    public Monster(uint agentId) : base(agentId)
    {

        StatusMachine = new MonsterStatusMachine();

    }


    public override void Dispose()
    {
        base.Dispose();

        if (BehaviourTree != null)
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

        var go = PrefabUtil.LoadPrefab(mMonsterCfg.Prefab, GameNodeCenter.Ins.MonsterNode, "Load Agent Prefab");
        if (go != null)
        {
            MonsterView monsterView = go.GetComponent<MonsterView>();
            BindMonsterView(monsterView);
            m_Agent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
    }

    protected override void CustomInitialize()
    {
        // added by weng 0704
        _mAgtAttr = new AgentAttribute(
            mMonsterCfg.MaxHp, mMonsterCfg.MaxHp, mMonsterCfg.BaseAttack, mMonsterCfg.DefenseRate,
            mMonsterCfg.CriticalRate, mMonsterCfg.CriticalDmgRate, mMonsterCfg.DodgeRate, mMonsterCfg.MoveSpeed
            );
        this.AttrHandler = new AgentAttrHandler();
        this.AttrHandler.InitAgentAttr(this, _mAgtAttr);
        this.ColliderHandler = new MonsterColliderHandler(this);


        // 位置初始化
        SetPosition(new Vector3(22f, 0.2f, 15f));
        // 朝向初始化
        SetRotation(Vector3.zero);
        // 缩放初始化 modifiy by weng 0704
        //SetScale(Vector3.one);

        MoveControl = new MonsterMoveControl(this);
        SetSpeed(mMonsterCfg.Speed);
        SetTurnSpeed(mMonsterCfg.TurnSpeed);
        SetDashDistance(mMonsterCfg.DashDistance);
        SetName(mMonsterCfg.Name);
        SetAttackRadius(mMonsterCfg.AttackRadius);
        SetInteractRadius(mMonsterCfg.InteractRadius);
        SetFollowRadius(mMonsterCfg.FollowRadius);

        Combo_Trigger.SetComboActive("JJJ", true);
        Combo_Trigger.SetComboActive("DashAttack", true);


        // 加载行为树
        BehaviourTree = DataCenter.Ins.BehaviourTreeCenter.LoadTreeWithTreeName(mMonsterCfg.BehaviourTree);
        BehaviourTree.Initialize(this, new System.Collections.Generic.Dictionary<string, object>());
    }



    public override void OnUpdate(float deltaTime)
    {
        // modified by weng
        // 0906
        base.OnUpdate(deltaTime);
        if (BehaviourTree != null)
        {
            BehaviourTree.Excute(deltaTime);
        }
        //随机游走
        //if (m_Agent.pathPending || m_Agent.remainingDistance > 0.1f)
            //return;
        //m_Agent.destination = m_Range * Random.insideUnitCircle;
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
