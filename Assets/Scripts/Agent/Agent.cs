using UnityEngine;

public abstract class Agent : IEntity, IMeterHandler
{
    /// <summary>
    /// 角色id
    /// </summary>
    protected uint mAgentId;
    /// <summary>
    /// 实体id
    /// </summary>
    protected int mEntityId;
    /// <summary>
    /// 角色的游戏体
    /// </summary>
    protected GameObject mAgentGo;

    // 相机追随的虚拟目标(防止动画自身位置)
    //protected VirtualCamTarget mVirtualCamTarget;

    /// <summary>
    /// 动画播放控制器
    /// </summary>
    public AgentAnimPlayer AnimPlayer;

    /// <summary>
    /// 动画状态机
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
    /// Combo管理器
    /// </summary>
    public ComboHandler ComboHandler;

    public uint GetAgentId()
    {
        return mAgentId;
    }

    public int GetEntityId()
    {
        return mEntityId;
    }
    // 角色移动速度
    protected float mSpeed;
    // 角色的冲刺速度
    protected float mDashDistance;
    // 角色朝向
    protected Vector3 mTowards;
    // 角色位置
    protected Vector3 mPosition;

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
        CustomInitialize();

        StatusGraph = DataCenter.Ins.AgentStatusGraphCenter.GetAgentStatusGraph(mAgentId);

        StatusMachine = new AgentStatusMachine();
        StatusMachine.Initialize(this);
        MeterManager.Ins.RegisterMeterHandler(this);

        ComboHandler = new ComboHandler();
        ComboGraph cg = DataCenter.Ins.AgentComboGraphCenter.GetAgentComboGraph(mAgentId);
        ComboHandler.Initialize(cg);
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Dispose()
    {
        EntityManager.Ins.RemoveEntity(this);
        mAgentGo = null;

        if(AnimPlayer != null)
        {
            AnimPlayer.Dispose();
            AnimPlayer = null;
        }

        if(ComboHandler != null)
        {
            ComboHandler.Dispose();
            ComboHandler = null;
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
        mEntityId = EntityManager.Ins.AddEntity(this);
        AnimPlayer = new AgentAnimPlayer();
    }

    public AgentAnimStateInfo GetStateInfo(string statusName, string stateName)
    {
        AgentStatusInfo statusInfo = GetStatusInfo(statusName);
        if (statusInfo == null)
            return null;

        if(statusInfo.animStates == null || statusInfo.animStates.Length == 0)
        {
            Log.Error(LogLevel.Normal, "GetStateInfo Failed, statusInfo.animStates is null or empty!");
            return null;
        }

        for(int i = 0; i < statusInfo.animStates.Length; i++)
        {
            AgentAnimStateInfo stateInfo = statusInfo.animStates[i];
            if (stateInfo.stateName == stateName)
            {
                return stateInfo;
            }
        }

        return null;
    }

    public AgentStatusInfo GetStatusInfo(string statusName)
    {
        if (StatusGraph == null)
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, StatusGraph is null!");
            return null;
        }

        if(StatusGraph.statusMap == null)
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, StatusGraph.statusMap is null!");
            return null;
        }

        AgentStatusInfo statusInfo;
        if (!StatusGraph.statusMap.TryGetValue(statusName, out statusInfo))
        {
            Log.Error(LogLevel.Normal, "GetStatusInfo Failed, no status info named {0}", statusName);
        }

        return statusInfo;
    }

    public virtual void OnMeter(int meterIndex)
    {
        StatusMachine.OnMeter(meterIndex);
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

        // 对于同一拍的同一个指令不做处理
        if (cmd.Equals(lastInputCmd))
        {
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // 记录这次的指令数据
        AgentInputCommandPool.Ins.PushAgentInputCommand(lastInputCmd);
        lastInputCmd = AgentInputCommandPool.Ins.CreateAgentInputCommandCopy(cmd);

        if (StatusMachine == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, StatusMachine is null!");
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // 获取当前的status
        IAgentStatus curStatus = StatusMachine.CurStatus;
        if(curStatus == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, cur status is null!");
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // 如果触发了combo，就执行combo逻辑，否则执行单个指令的逻辑
        // $$$$$$$$$$$$$$$这里的逻辑不对，应该是combohandler通过对指令进行匹配，查出来一个匹配的combomove，然后附带给cmd，传给status
        // 否则触发了combo的指令就无法正确执行指令的逻辑了

        if (ComboHandler.TryTriggerCombo(cmd.CmdType, cmd.TriggerMeter, out Combo combo, out ComboStep comboMove))
        {
            curStatus.OnComboCommand(cmd, combo, comboMove);
        }
        else
        {
            curStatus.OnNormalCommand(cmd);
        }

        AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
    }

    float record;

    /// <summary>
    /// update
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void OnUpdate(float deltaTime)
    {
        MoveControl.OnUpdate(deltaTime);
        StatusMachine.OnUpdate(deltaTime);

        record += deltaTime;

        // 4 6 拍为何让这玩意儿打两下?
        if(record >= 1.7f && record <= 1.8f)
        {
            var cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.ATTACK_LIGHT, 4, GamePlayDefine.InputDirection_NONE);
            OnCommand(cmd);
        }

        if (record >= 2.30f && record <= 2.35f)
        {
            var cmd = AgentInputCommandPool.Ins.PopAgentInputCommand();
            cmd.Initialize(AgentCommandDefine.ATTACK_LIGHT, 6, GamePlayDefine.InputDirection_NONE);
            OnCommand(cmd);
        }
    }


}
