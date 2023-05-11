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

    public DongciDaci.HeroBaseCfg GetAgentCfg()
    {
        return mHeroCfg;
    }

    protected override void LoadAgentGo()
    {
        if (mHeroCfg == null)
        {
            Log.Error(LogLevel.Critical, "LoadAgentGo Failed, mHeroCfg is null or empty!");
            return;
        }

        if (string.IsNullOrEmpty(mHeroCfg.Prefab))
        {
            Log.Error(LogLevel.Critical, "LoadAgentGo Failed, path is null or empty!");
            return;
        }

        mAgentGo = PrefabUtil.LoadPrefab(mHeroCfg.Prefab, AgentManager.Ins.GetHeroNode(), "Load Agent Prefab");
    }

    protected override void CustomInitialize()
    {
        if (mAgentGo != null)
        {
            // 位置初始化
            SetPosition(new Vector3(0, 1, 0));
            // 朝向初始化
            SetTowards(new Vector3(0, 0, 1));
            AnimPlayer.Initialize(mAgentGo.GetComponent<Animator>());
        }

        if (mHeroCfg != null && mAgentGo != null)
        {
            mAgentGo.name = mHeroCfg.Name;
            Camera mainCam = CameraManager.Ins.GetMainCam();
            if (mainCam != null)
            {
                mCft = mainCam.gameObject.AddComponent<CamFollowTarget>();
                mCft.SetFollowTarget(mAgentGo, new Vector3(0, -0.5f, 0.866f), 20);
            }
        }

        MoveControl = new PlayerMoveControl(this);
        SetSpeed(mHeroCfg.Speed);
        SetDashDistance(mHeroCfg.DashDistance);

        ComboTrigger.SetComboActive("JJJ", true);
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
}
