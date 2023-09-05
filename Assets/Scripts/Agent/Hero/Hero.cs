
using UnityEngine;
using GameEngine;
using GameSkillEffect;

public class Hero : Agent
{
    /// <summary>
    /// 英雄配置数据
    /// </summary>
    private DongciDaci.HeroBaseCfg mHeroCfg;

    /// <summary>
    /// 英雄的属性
    /// Added by weng 0704
    /// </summary>
    private HeroAttribute _mHeroAttr;

    /// <summary>
    /// 英雄的属性管理
    /// Added by weng 0704
    /// </summary>
    public HeroAttrHandler HeroAttrHandler;


    /// <summary>
    /// 相机跟随
    /// </summary>
    private CamFollowTarget mCft;

    public HeroView Hero_View;

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
        Hero_View = heroView;
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

        var go = PrefabUtil.LoadPrefab(mHeroCfg.Prefab, GameNodeCenter.Ins.HeroNode, "Load Agent Prefab");
        if(go != null)
        {
            HeroView heroView = go.GetComponent<HeroView>();
            BindHeroView(heroView);
        }
    }



    private IInputHandle attackmouseInput;

    protected override void CustomInitialize()
    {
        // added by weng 0704
        // 英雄属性和属性管理器，和 agtAttr 和 attrHandler是同一个对象
        _mHeroAttr = new HeroAttribute(
            mHeroCfg.MaxHp, mHeroCfg.MaxHp, mHeroCfg.BaseAttack, mHeroCfg.DefenseRate,
            mHeroCfg.CriticalRate, mHeroCfg.CriticalDmgRate, mHeroCfg.DodgeRate, mHeroCfg.MoveSpeed, 
            0, mHeroCfg.ExtraEnergyGain,mHeroCfg.BeatTolerance,mHeroCfg.LuckyRate
            );
        _mAgtAttr = _mHeroAttr;
        this.HeroAttrHandler = new HeroAttrHandler();
        this.HeroAttrHandler.InitAgentAttr(this, _mAgtAttr);
        this.AttrHandler = HeroAttrHandler;
        this.ColliderHandler = new HeroColliderHandler(this);

        // 位置初始化
        // changed by weng 0626
        SetPosition(new Vector3(14f,0.2f,10f));
        // 朝向初始化
        SetRotation(Vector3.zero);
        // 缩放初始化
        SetScale(Vector3.one);

        Camera mainCam = CameraManager.Ins.GetMainCam();
        if (mainCam != null)
        {
            mCft = mainCam.gameObject.AddComponent<CamFollowTarget>();
            // changed by weng 0626
            // turn vector3(0,10,-10) to vector3(0,10,-6.5)
            // changed by weng 0813
            mCft.SetFollowTarget(Hero_View.GetGameObject(), new Vector3(0, 20f, -20f));
        }

        MoveControl = new PlayerMoveControl(this);
        SetSpeed(mHeroCfg.Speed);
        SetTurnSpeed(mHeroCfg.TurnSpeed);
        SetDashDistance(mHeroCfg.DashDistance);
        SetName(mHeroCfg.Name);

        Combo_Trigger.SetComboActive("JJJ", true);
        Combo_Trigger.SetComboActive("JJJ_Instant", false);
        Combo_Trigger.SetComboActive("DashAttack", true);

        attackmouseInput = new AgentMouseInputHandle_Attack(this);
        InputControlCenter.MouseInputCtl.RegisterInputHandle("Test", attackmouseInput);
        attackmouseInput.SetEnable(true);
    }

    public Hero(uint agentId) : base(agentId)
    {
        StatusMachine = new HeroStatusMachine();
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (mCft != null)
        {
            mCft.OnUpdate(deltaTime);
        }

        Combo_Trigger.SetComboActive("JJJ_Instant", true);
    }

    public override void Dispose()
    {
        base.Dispose();
        attackmouseInput.SetEnable(false);
        InputControlCenter.MouseInputCtl.UnregisterInputHandle("Test");
    }

    public override int GetEntityType()
    {
        return EntityTypeDefine.Hero;
    }


    protected override MyColliderType ColliderType => MyColliderType.Collider_Hero;
    /**
    protected override int GetColliderType()
    {
        return GameColliderDefine.ColliderType_Hero;
    }

    protected override IGameColliderHandler GetColliderHanlder()
    {
        return new HeroColliderHandler(this);
    }
    */

}
