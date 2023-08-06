using UnityEngine;
using GameEngine;
using System.Collections.Generic;
using GameSkillEffect;

public class GameManager:MonoBehaviour
{
    public static GameManager Ins;

    private GameConfig mGameConf;

    // 是否可以update
    private bool enableUpdate;

    private CoroutineManager                        CoroutineMgr;                                   // 协程管理器
    private UIManager                               UIMgr;                                          // UI管理器
    private ResourceMgr                             ResMgr;                                         // 资源加载管理器
    private GameSceneManager                        SceneMgr;                                       // 场景管理器
    private CameraManager                           CameraMgr;                                      // 相机管理器
    private InputManager                            InputMgr;                                       // 输入控制管理器
    private GameEventSystem                         EventSystem;                                    // 事件系统
    private AudioManager                            AudioMgr;                                       // 声音管理器
    private MeterManager                            MeterMgr;                                       // 节拍管理器

    //public GameScopeManager       GameScopeMgr { get; private set; }     // 域管理器
    //private AgentManager                            AgentMgr;                                       // 角色控制器 
    private DataCenter                              DataCenter;                                     // 数据中心
    private TimerCenter                             TimerCenter;                                    // 定时器中心
    private MeterTimerCenter                        MeterTimerCenter;                               // 节拍定时器中心
    private UpdateCenter                            UpdateCenter;                                   // Update驱动中心
    private TimeMgr                                 TimeMgr;                                        // 时间中心

    private GameColliderManager                     GameColliderMgr;                                // 碰撞中心
    private GameMapManager                          GameMapMgr;                                     // 地图管理器

    // added by weng 0708
    private SkEftEntityManager                      SkEftEntityMgr;                                 //技能效果实体的管理器

    private List<IModuleManager> mAllModuleMgrs;

    public GameConfig GetGameConfig()
    {
        return this.mGameConf;
    }

    private void Initialize()
    {
        Ins = this;
        enableUpdate = true;
        mAllModuleMgrs = new List<IModuleManager>();

        // 这里的注册顺序不要修改
        //GameScopeMgr = RegisterModuleMgr(GameScopeManager.Ins);
        TimeMgr = RegisterModuleMgr(TimeMgr.Ins);
        UpdateCenter = RegisterModuleMgr(UpdateCenter.Ins);
        DataCenter = RegisterModuleMgr(DataCenter.Ins);
        EventSystem = RegisterModuleMgr(GameEventSystem.Ins);
        CoroutineMgr = RegisterModuleMgr(CoroutineManager.Ins);
        UIMgr = RegisterModuleMgr(UIManager.Ins);
        CameraMgr = RegisterModuleMgr(CameraManager.Ins);
        ResMgr = RegisterModuleMgr(ResourceMgr.Ins);
        MeterMgr = RegisterModuleMgr(MeterManager.Ins);
        InputMgr = RegisterModuleMgr(InputManager.Ins);
        SceneMgr = RegisterModuleMgr(GameSceneManager.Ins);
        AudioMgr = RegisterModuleMgr(AudioManager.Ins);
        TimerCenter = RegisterModuleMgr(TimerCenter.Ins);
        MeterTimerCenter = RegisterModuleMgr(MeterTimerCenter.Ins);
        //AgentMgr = RegisterModuleMgr(AgentManager.Ins);
        GameColliderMgr = RegisterModuleMgr(GameColliderManager.Ins);
        GameMapMgr = RegisterModuleMgr(GameMapManager.Ins);
        SkEftEntityMgr = RegisterModuleMgr(SkEftEntityManager.Ins);

        // 先做所有模块的初始化
        foreach (IModuleManager mm in mAllModuleMgrs)
        {
            mm.__Initialize__();
            mm.Initialize();
        }

        // add 0615 weng
        Application.targetFrameRate = 120;

    }

    private void Awake()
    {
        Debug.developerConsoleVisible = true;
        //DisposeGame();
        Initialize();
        Launch();
    }

    private void DisposeGame()
    {
        if (mAllModuleMgrs != null)
        {
            foreach (IModuleManager mm in mAllModuleMgrs)
            {
                mm.__Dispose__();
                mm.Dispose();
            }
        }

        GamePoolCenter.Ins.Dispose();

        mGameConf = null;
        enableUpdate = false;
        mAllModuleMgrs = null;
        EventSystem = null;
        CoroutineMgr = null;
        UIMgr = null;
        CameraMgr = null;
        ResMgr = null;
        InputMgr = null;
        SceneMgr = null;
        AudioMgr = null;
        MeterMgr = null;
        //AgentMgr = null;
        TimerCenter = null;
        DataCenter = null;
        MeterTimerCenter = null;
        TimeMgr = null;
        GameColliderMgr = null;
        GameMapMgr = null;
        // added by weng 0708
        SkEftEntityMgr = null;
    }

    private T RegisterModuleMgr<T>(T mgr) where T:IModuleManager
    {
        mAllModuleMgrs.Add(mgr);
        return mgr;
    }

    public void IntializeGameConfig(GameConfig cfg)
    {
        this.mGameConf = cfg;
    }

    public void QuitGame()
    {
        DisposeGame();
        Destroy(this);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void RegisterInputOnLaunch()
    {
        // 注册键盘输入控制
        InputManager.Ins.RegisterInputControl(InputControlCenter.KeyboardInputCtl);

        // 注册鼠标输入控制
        InputManager.Ins.RegisterInputControl(InputControlCenter.MouseInputCtl);
        InputControlCenter.MouseInputCtl.RegisterInputHandle(
            InputDef.MouseInputHandle_CommonInput,
            new MouseInputHandle_CommonInput());

        // 注册手柄输入控制
        InputManager.Ins.RegisterInputControl(InputControlCenter.JoystickInputCtl);
    }

    private void Launch()
    {
        RegisterInputOnLaunch();
    }

    private float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    private void Update()
    {
        float deltaTime = GetDeltaTime();
        
        if(enableUpdate)
        {
            TimeMgr.OnGameUpdate(deltaTime);
            ResMgr.OnGameUpdate(deltaTime);
            CoroutineMgr.OnGameUpdate(deltaTime);
            SceneMgr.OnGameUpdate(deltaTime);
            InputMgr.OnGameUpdate(deltaTime);
            CameraMgr.OnGameUpdate(deltaTime);
            EventSystem.OnGameUpdate(deltaTime);
            AudioMgr.OnGameUpdate(deltaTime);
            MeterMgr.OnGameUpdate(deltaTime);
            TimerCenter.OnGameUpdate(deltaTime);
            MeterTimerCenter.OnGameUpdate(deltaTime);
            //AgentMgr.OnGameUpdate(deltaTime);
            UpdateCenter.OnGameUpdate(deltaTime);
            GameColliderMgr.OnGameUpdate(deltaTime);
            // added by weng 0708
            SkEftEntityMgr.OnGameUpdate(deltaTime);
        }

        UIMgr.OnGameUpdate(deltaTime);
    }

    private void LateUpdate()
    {
        float deltaTime = GetDeltaTime();

        ResMgr.OnLateUpdate(deltaTime);
        SceneMgr.OnLateUpdate(deltaTime);
        InputMgr.OnLateUpdate(deltaTime);
        CameraMgr.OnLateUpdate(deltaTime);
        EventSystem.OnLateUpdate(deltaTime);
        AudioMgr.OnLateUpdate(deltaTime);
        UIMgr.OnLateUpdate(deltaTime);
        MeterMgr.OnLateUpdate(deltaTime);
        //AgentMgr.OnLateUpdate(deltaTime);
        TimerCenter.OnLateUpdate(deltaTime);
        MeterTimerCenter.OnLateUpdate(deltaTime);
        GameColliderMgr.OnLateUpdate(deltaTime);
    }

    private void OnDestroy()
    {
        
    }
}
