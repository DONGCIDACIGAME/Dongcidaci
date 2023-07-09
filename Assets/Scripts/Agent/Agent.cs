using GameEngine;
using GameSkillEffect;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Agent : MapEntityWithCollider, IMeterHandler
{
    /// <summary>
    /// 角色id
    /// </summary>
    protected uint mAgentId;

    /// <summary>
    /// 动画播放控制器
    /// </summary>
    public AnimPlayer AnimPlayer;

    /// <summary>
    /// 状态机
    /// </summary>
    public AgentStatusMachine StatusMachine;

    /// <summary>
    /// 移动控制
    /// </summary>
    public AgentMoveControl MoveControl;

    /// <summary>
    /// 角色状态信息表
    /// </summary>
    public AgentStatusGraph StatusGraph;

    /// <summary>
    /// Combo触发器
    /// </summary>
    public ComboTrigger Combo_Trigger;

    /// <summary>
    /// 效果执行器
    /// </summary>
    public EffectExcutorController EffectExcutorCtl;

    /// <summary>
    /// Added by Weng 0704
    /// 角色的技能效果状态管理器
    /// </summary>
    public AgentSkEftHandler SkillEftHandler;

    /// <summary>
    /// Added by Weng 0704
    /// 角色的属性
    /// </summary>
    protected AgentAttribute _mAgtAttr;

    /// <summary>
    /// Added by Weng 0704
    /// 角色的属性结算变更管理器
    /// </summary>
    public AgentAttrHandler AttrHandler;


    /// <summary>
    /// 碰撞处理
    /// </summary>
    public IGameColliderHandler ColliderHandler;



    /// <summary>
    /// 移动执行器
    /// </summary>
    public AnimMovementExcutorController MovementExcutorCtl;

    /// <summary>
    ///  用于更新Agent相关的基础表现逻辑，对外，对上层都不开放
    /// </summary>
    private AgentView mAgentView;

    // 角色移动速度
    protected float mSpeed;
    // 角色的冲刺速度
    protected float mDashDistance;
    protected string mName;
    /// <summary>
    /// 攻击半径
    /// </summary>
    protected float mAttackRadius;
    /// <summary>
    /// 交互半径
    /// </summary>
    protected float mInteractRadius;
    /// <summary>
    /// 跟随半径
    /// </summary>
    protected float mFollowRadius;

    public uint GetAgentId()
    {
        return mAgentId;
    }

    #region Properties
    public float GetSpeed()
    {
        return mSpeed;
    }

    public void SetSpeed(float speed)
    {
        mSpeed = speed;
    }

    public float GetDashDistance()
    {
        return mDashDistance;
    }

    public void SetDashDistance(float dashDistance)
    {
        mDashDistance = dashDistance;
    }

    public string GetName()
    {
        return mName;
    }

    public void SetName(string name)
    {
        mName = name;
    }

    public float GetAttackRadius()
    {
        return mAttackRadius;
    }

    public void SetAttackRadius(float radius)
    {
        mAttackRadius = radius;
    }

    public float GetInteractRadius()
    {
        return mInteractRadius;
    }

    public void SetInteractRadius(float radius)
    {
        mInteractRadius = radius;
    }


    public float GetFollowRadius()
    {
        return mFollowRadius;
    }

    public void SetFollowRadius(float radius)
    {
        mFollowRadius = radius;
    }

    #endregion

    public void SetTowards(Vector3 towards)
    {
        Quaternion quaternion = Quaternion.LookRotation(towards);
        SetRotation(quaternion.eulerAngles);
    }

    public Vector3 GetTowards()
    {
        Vector3 rot = GetRotation();
        Quaternion quaternion = Quaternion.Euler(rot);
        Vector3 towards = (quaternion * Vector3.forward).normalized;
        return towards;
    }


    /// <summary>
    /// 加载角色配置信息
    /// </summary>
    /// <param name="agentId"></param>
    protected abstract void LoadAgentCfg(uint agentId);

    /// <summary>
    /// 加载角色物件
    /// </summary>
    protected abstract void LoadAgentView(); 


    protected virtual void BindAgentView(AgentView agentView)
    {
        BindMapEntityView(agentView);
        mAgentView = agentView;

        if (mAgentView != null)
        {
            mAgentView.name = GetName();
            AnimPlayer.Initialize(mAgentView.GetComponent<Animator>());
        }
    }

    /// <summary>
    /// 自定义的初始化
    /// </summary>
    protected abstract void CustomInitialize();

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize()
    {
        // 加载角色基础配置数据
        LoadAgentCfg(mAgentId);

        // 加载角色表现层
        LoadAgentView();

        // comobo 触发器 初始化
        Combo_Trigger.Initialize(this);

        // 效果执行器 初始化
        EffectExcutorCtl.Initialize(this);

        // 移动控制器 初始化
        MovementExcutorCtl.Initialize(this);

        // 注册节拍处理
        MeterManager.Ins.RegisterMeterHandler(this);

        // 状态机 初始化
        StatusMachine.Initialize(this);

        // Added by weng 0704
        // 增加技能效果处理器的初始化
        SkillEftHandler = GamePoolCenter.Ins.AgtSkEftHandlerPool.Pop();
        SkillEftHandler.InitAgentSkEftHandler(this);


        // 其他自定义初始化
        CustomInitialize();

        // 加载完成后默认进入idle状态
        TriggeredComboStep triggeredComboStep = null;
        Dictionary<string, object> args = null;
        StatusMachine.SwitchToStatus(GetAgentId(), AgentStatusDefine.IDLE, AgentCommandDefine.IDLE, DirectionDef.none, MeterManager.Ins.MeterIndex, args, triggeredComboStep) ;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
        MeterManager.Ins.UnregiseterMeterHandler(this);

        if(AnimPlayer != null)
        {
            AnimPlayer.Dispose();
            AnimPlayer = null;
        }

        if(Combo_Trigger != null)
        {
            Combo_Trigger.Dispose();
            Combo_Trigger = null;
        }

        if(EffectExcutorCtl != null)
        {
            EffectExcutorCtl.Dispose();
            EffectExcutorCtl = null;
        }

        if(MovementExcutorCtl != null)
        {
            MovementExcutorCtl.Dispose();
            MovementExcutorCtl = null;
        }

        if(StatusMachine != null)
        {
            StatusMachine.Dispose();
            StatusMachine = null;
        }

        if(mAgentView != null)
        {
            mAgentView.Dispose();
            mAgentView = null;
        }
    }

    public Agent(uint agentId)
    {
        mAgentId = agentId;
        AnimPlayer = new AnimPlayer();
        StatusGraph = DataCenter.Ins.AgentStatusGraphCenter.GetAgentStatusGraph(mAgentId);
        StatusMachine = new AgentStatusMachine();
        Combo_Trigger = new ComboTrigger();
        EffectExcutorCtl = new EffectExcutorController();
        MovementExcutorCtl = new AnimMovementExcutorController();
    }


    /// <summary>
    /// 记录上一个指令
    /// 这是上一个输入指令的一份数据拷贝
    /// </summary>
    private AgentCommand lastInputCmd;

    

    public void OnCommand(AgentCommand cmd)
    {
        if (cmd == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, AgentInputCommand is null!");
            return;
        }

        // 如果和上一个指令在同一帧，则该指令也会被丢弃
        if(lastInputCmd != null && cmd.Frame == lastInputCmd.Frame)
        {
            return;
        }

        // 不是可以响应的指令类型
        if (!AttrHandler.CheckCmdReact(cmd.CmdType))
            return;

        // 对于可优化的指令，同一拍的同一个指令不做处理
        if (AgentCommandDefine.IsOptimizable(cmd.CmdType) && cmd.Equals(lastInputCmd))
        {
            cmd.Recycle();
            return;
        }

        // 上次记录的指令归还指令池
        // modified by weng 0516 1720
        if (lastInputCmd != null)
        {
            lastInputCmd.Recycle();
        }

        // 记录这次的指令数据
        lastInputCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop().ShallowCopy(cmd);

        if (StatusMachine == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, StatusMachine is null!");
            cmd.Recycle();
            return;
        }

        // 获取当前的status
        AgentStatus curStatus = StatusMachine.CurStatus;
        if(curStatus == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, cur status is null!");
            cmd.Recycle();
            return;
        }

        // 新的输入尝试触发combo
        int result = Combo_Trigger.OnNewInput(cmd.CmdType, cmd.TriggerMeter, out TriggeredComboStep triggeredComboStep);

        // 如果触发了combo，就需要同时处理指令和combo的逻辑
        if (result == ComboDefine.ComboTriggerResult_Succeed)
        {
            Log.Error(LogLevel.Info, "Trigger combo {0}-{1}", triggeredComboStep.comboData.comboName, triggeredComboStep.comboStep.agentActionData.stateName);
            curStatus.OnCommand(cmd, triggeredComboStep);
        }
        // 如果不是combo的触发命令类型，就直接执行指令
        else if(result == ComboDefine.ComboTriggerResult_NotComboTrigger)
        {
            curStatus.OnCommand(cmd, triggeredComboStep);
        }
        // 是combo的触发命令类型，但是没有匹配到combo，就按照正常指令执行
        else if(result == ComboDefine.ComboTriggerResult_Failed)
        {
            curStatus.OnCommand(cmd, triggeredComboStep);
        }
        // 是combo的触发命令类型，但是上一个combo的执行还未完成
        else if(result == ComboDefine.ComboTriggerResult_ComboExcuting)
        {
            Log.Error(LogLevel.Info, "##上一个combo还未执行完成，请降低输入频率, cmd:{0}", cmd.CmdType);
            // 后面可能会在这里做一些UI提示
        }

        // 指令归还指令池
        cmd.Recycle();
    }

    public virtual void OnMeterEnter(int meterIndex)
    {
        StatusMachine.OnMeterEnter(meterIndex);
        Combo_Trigger.OnMeterEnter(meterIndex);
        EffectExcutorCtl.OnMeterEnter(meterIndex);
        MovementExcutorCtl.OnMeterEnter(meterIndex);
        // Added by weng 0708
        // Qustion :Is grammer SkillEftHandler?.OnMeterEnter(meterIndex) can be used?
        if (SkillEftHandler != null)
        {
            SkillEftHandler.OnMeterEnter(meterIndex);
        }
        
    }

    
    public virtual void OnMeterEnd(int meterIndex)
    { 
        StatusMachine.OnMeterEnd(meterIndex);
        Combo_Trigger.OnMeterEnd(meterIndex);
        EffectExcutorCtl.OnMeterEnd(meterIndex);
        MovementExcutorCtl.OnMeterEnd(meterIndex);
        // Added by weng 0708
        if (SkillEftHandler != null)
        {
            SkillEftHandler.OnMeterEnd(meterIndex);
        }
    }


    /// <summary>
    /// update
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void OnUpdate(float deltaTime)
    {
        MoveControl.OnUpdate(deltaTime);
        StatusMachine.OnUpdate(deltaTime);
        EffectExcutorCtl.OnUpdate(deltaTime);
        MovementExcutorCtl.OnUpdate(deltaTime);
        mAgentView.OnMyUpdate(this, deltaTime);
        // Added by weng 0708
        if (SkillEftHandler != null)
        {
            SkillEftHandler.OnUpdate(deltaTime);
        }

    }

    public virtual void OnLateUpdate(float deltaTime)
    {
        mAgentView.OnMyLateUpdate(this, deltaTime);
    }
    
    public List<T> DetectMapEntityInVision<T>() where T: MapEntity
    {
        List<T> entityInVision = new List<T>();
        var visionShape = mAgentView.GetVisionShape();
        bool ret = GameColliderManager.Ins.CheckCollideHappenWithShape(visionShape, ColliderHanlderDefine.EmptyHandler, out Dictionary<ConvexCollider2D, Vector2> tgtsDict);
        if (!ret)
            return null;

        foreach(KeyValuePair< ConvexCollider2D, Vector2> kv in tgtsDict)
        {
            ConvexCollider2D collider = kv.Key;
            Entity entity = EntityManager.Ins.GetEntity(collider.GetBindEntityId());
            if(entity is T)
            {
                entityInVision.Add(entity as T);
            }
        }
        return entityInVision;
    }


    public List<Agent> DetectAgentInVision()
    {
        var retAgents = new List<Agent>();
        var visionShape = mAgentView.GetVisionShape();
        bool ret = GameColliderManager.Ins.CheckCollideHappenWithShape(visionShape, ColliderHanlderDefine.EmptyHandler, out Dictionary<ConvexCollider2D, Vector2> tgtsDict);
        if (ret)
        {
            var agentColliders = GameColliderDefine.GetAgentColliders(tgtsDict.Keys.ToArray());
            if (agentColliders.Count > 0)
            {
                foreach (var collider in agentColliders)
                {
                    var entityId = collider.GetBindEntityId();
                    if (entityId == 0) continue;
                    var detectedEntity = EntityManager.Ins.GetEntity(entityId);
                    if (detectedEntity is Agent)
                    {
                        retAgents.Add(detectedEntity as Agent);
                    }
                } 
            }
        }
        return retAgents;
    }

}
