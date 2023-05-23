using UnityEngine;

public abstract class Agent : Entity, IMeterHandler
{
    /// <summary>
    /// 角色id
    /// </summary>
    protected uint mAgentId;

    /// <summary>
    /// 角色的游戏体
    /// </summary>
    protected GameObject mAgentGo;

    /// <summary>
    /// 动画播放控制器
    /// </summary>
    public AgentAnimPlayer AnimPlayer;

    /// <summary>
    /// 状态机
    /// </summary>
    protected AgentStatusMachine StatusMachine;

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
    public ComboTrigger ComboTrigger;

    /// <summary>
    /// combo招式效果执行器
    /// </summary>
    public ComboActionEffectsExcutor ComboEffectsExcutor;

    // 角色移动速度
    protected float mSpeed;
    // 角色的冲刺速度
    protected float mDashDistance;
    // 角色朝向
    protected Vector3 mTowards;
    // 角色位置
    protected Vector3 mPosition;

    public uint GetAgentId()
    {
        return mAgentId;
    }


    public Vector3 GetPosition()
    {
        return mPosition;
    }

    public void SetPosition(Vector3 position)
    {
        mPosition = position;
        if(mAgentGo != null)
        {
            mAgentGo.transform.position = position;
        }
    }

    public float GetSpeed()
    {
        return mSpeed;
    }

    public void SetSpeed(float speed)
    {
        mSpeed = speed;
        Log.Logic(LogLevel.Info, "set speed:{0}", speed);
    }

    public float GetDashDistance()
    {
        return mDashDistance;
    }

    public void SetDashDistance(float dashDistance)
    {
        this.mDashDistance = dashDistance;
    }

    public Vector3 GetTowards()
    {
        return mTowards.normalized;
    }

    public void SetTowards(Vector3 towards)
    {
        mTowards = towards;
        if(mAgentGo != null)
        {
            mAgentGo.transform.rotation = Quaternion.LookRotation(towards);
            //Log.Logic(LogLevel.Info, "SetTowards-----{0}", towards);
        }
    }


    /// <summary>
    /// 加载角色配置信息
    /// </summary>
    /// <param name="agentId"></param>
    protected abstract void LoadAgentCfg(uint agentId);

    /// <summary>
    /// 加载角色物件
    /// </summary>
    protected abstract void LoadAgentGo();

    /// <summary>
    /// 自定义的初始化
    /// </summary>
    protected abstract void CustomInitialize();

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize()
    {
        LoadAgentCfg(mAgentId);
        LoadAgentGo();

        ComboTrigger.Initialize(this);
        ComboEffectsExcutor.Initialize(this);

        MeterManager.Ins.RegisterMeterHandler(this);
        StatusMachine.Initialize(this);
        CustomInitialize();

        StatusMachine.SwitchToStatus(AgentStatusDefine.IDLE, null);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Dispose()
    {
        EntityManager.Ins.RemoveEntity(this);
        MeterManager.Ins.UnregiseterMeterHandler(this);
        mAgentGo = null;

        if(AnimPlayer != null)
        {
            AnimPlayer.Dispose();
            AnimPlayer = null;
        }

        if(ComboTrigger != null)
        {
            ComboTrigger.Dispose();
            ComboTrigger = null;
        }

        if(ComboEffectsExcutor != null)
        {
            ComboEffectsExcutor.Dispose();
            ComboEffectsExcutor = null;
        }

        if(StatusMachine != null)
        {
            StatusMachine.Dispose();
            StatusMachine = null;
        }
    }

    public Agent(uint agentId)
    {
        mAgentId = agentId;
        EntityManager.Ins.AddEntity(this);
        AnimPlayer = new AgentAnimPlayer();
        StatusGraph = DataCenter.Ins.AgentStatusGraphCenter.GetAgentStatusGraph(mAgentId);
        StatusMachine = new AgentStatusMachine();
        ComboTrigger = new ComboTrigger();
        ComboEffectsExcutor = new ComboActionEffectsExcutor();
    }


    /// <summary>
    /// 记录上一个指令
    /// 这是上一个输入指令的一份数据拷贝
    /// </summary>
    private AgentInputCommand lastInputCmd;

    public void OnCommand(AgentInputCommand cmd)
    {
        if(cmd == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, AgentInputCommand is null!");
            return;
        }

        // 如果和上一个指令在同一帧，则该指令也会被丢弃
        if(lastInputCmd != null && cmd.Frame == lastInputCmd.Frame)
        {
            return;
        }

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
        lastInputCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop().Copy(cmd);

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
        int result = ComboTrigger.OnNewInput(cmd.CmdType, cmd.TriggerMeter, out TriggeredComboAction combo);

        // 如果触发了combo，就需要同时处理指令和combo的逻辑
        if (result == ComboDefine.ComboTriggerResult_Succeed)
        {
            Log.Error(LogLevel.Info, "Trigger combo {0}-{1}", combo.comboData.comboName, combo.actionData.stateName);
            curStatus.OnComboCommand(cmd, combo);
        }
        // 如果不是combo的触发命令类型，就直接执行指令
        else if(result == ComboDefine.ComboTriggerResult_NotComboTrigger)
        {
            curStatus.OnNormalCommand(cmd);
        }
        // 是combo的触发命令类型，但是没有匹配到combo，就按照正常指令执行
        else if(result == ComboDefine.ComboTriggerResult_Failed)
        {
            curStatus.OnNormalCommand(cmd);
        }
        // 是combo的触发命令类型，但是上一个combo的执行还未完成
        else if(result == ComboDefine.ComboTriggerResult_ComboExcuting)
        {
            Log.Logic(LogLevel.Info, "##上一个combo还未执行完成，请降低输入频率, cmd:{0}", cmd.CmdType);
            // 后面可能会在这里做一些UI提示
        }

        // 指令归还指令池
        cmd.Recycle();
    }

    public void OnMeterEnter(int meterIndex)
    {
        StatusMachine.OnMeterEnter(meterIndex);
        ComboTrigger.OnMeterEnter(meterIndex);
        ComboEffectsExcutor.OnMeterEnter(meterIndex);
    }

    
    public void OnMeterEnd(int meterIndex)
    { 
        StatusMachine.OnMeterEnd(meterIndex);
        ComboTrigger.OnMeterEnd(meterIndex);
        ComboEffectsExcutor.OnMeterEnd(meterIndex);
    }


    /// <summary>
    /// update
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void OnUpdate(float deltaTime)
    {
        MoveControl.OnUpdate(deltaTime);
        StatusMachine.OnUpdate(deltaTime);
        ComboEffectsExcutor.OnUpdate(deltaTime);
    }
}
