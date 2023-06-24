using UnityEngine;
using GameEngine;

public class Hero : Agent
{
    /// <summary>
    /// 英雄配置数据
    /// </summary>
    private DongciDaci.HeroBaseCfg mHeroCfg;

    /// <summary>
    /// 相机跟随
    /// </summary>
    private CamFollowTarget mCft;

    private HeroView mHeroView;


    protected override void LoadAgentCfg(uint agentId)
    {
        DongciDaci.HeroBaseCfg cfg = null;
        DongciDaci.HeroCfg_Data data = ConfigDatabase.Get<DongciDaci.HeroCfg_Data>();
        if(data != null)
        {
            var cfgs = data.HeroBaseCfgItems;
            if(cfgs.ContainsKey(agentId))
            {
                cfg = cfgs[agentId];
            }
        }

        mHeroCfg =  cfg;
    }

    private void BindHeroView(HeroView heroView)
    {
        BindAgentView(heroView);
        mHeroView = heroView;
    }

    protected override void LoadAgentView()
    {
        if (mHeroCfg == null)
        {
            Log.Error(LogLevel.Critical, "LoadAgentView Failed, mHeroCfg is null or empty!");
            return;
        }

        if (string.IsNullOrEmpty(mHeroCfg.Prefab))
        {
            Log.Error(LogLevel.Critical, "LoadAgentView Failed, path is null or empty!");
            return;
        }

        var go = PrefabUtil.LoadPrefab(mHeroCfg.Prefab, AgentManager.Ins.GetHeroNode(), "Load Agent Prefab");
        if(go != null)
        {
            HeroView heroView = go.GetComponent<HeroView>();
            BindHeroView(heroView);
        }
    }



    protected override void CustomInitialize()
    {
        // 位置初始化
        SetPosition(Vector3.zero);
        // 朝向初始化
        SetRotation(Vector3.zero);
        // 缩放初始化
        SetScale(Vector3.one);

        Camera mainCam = CameraManager.Ins.GetMainCam();
        if (mainCam != null)
        {
            mCft = mainCam.gameObject.AddComponent<CamFollowTarget>();
            mCft.SetFollowTarget(mHeroView.GetGameObject(), new Vector3(0, 10f, -10f));
        }

        MoveControl = new PlayerMoveControl(this);
        SetSpeed(mHeroCfg.Speed);
        SetDashDistance(mHeroCfg.DashDistance);
        SetName(mHeroCfg.Name);

        Combo_Trigger.SetComboActive("JJJ", true);
        Combo_Trigger.SetComboActive("DashAttack", true);
    }

    public Hero(uint agentId) : base(agentId)
    {

    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (mCft != null)
        {
            mCft.OnUpdate(deltaTime);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }

    public override int GetEntityType()
    {
        return EntityTypeDefine.Hero;
    }

    protected override int GetColliderType()
    {
        return GameColliderDefine.ColliderType_Hero;
    }

    protected override IGameColliderHandler GetColliderHanlder()
    {
        return new HeroColliderHandler(this);
    }
}
