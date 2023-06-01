using System.Collections.Generic;
using UnityEngine;

public abstract class Agent : MapEntity, IMeterHandler
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
    public ComboTrigger Combo_Trigger;

    /// <summary>
    /// 效果执行器
    /// </summary>
    public EffectExcutorController EffectExcutorCtl;

    public MovementExcutorController MovementExcutorCtl;

    // 角色移动速度
    protected float mSpeed;
    // 角色的冲刺速度
    protected float mDashDistance;

    public uint GetAgentId()
    {
        return mAgentId;
    }


    //public Vector3 GetPosition()
    //{
    //    return mPosition;
    //}

    //public void SetPosition(Vector3 position)
    //{
    //    mPosition = position;
    //    if(mAgentGo != null)
    //    {
    //        mAgentGo.transform.position = position;
    //    }
    //}

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

    //public Vector3 GetTowards()
    //{
    //    return mTowards.normalized;
    //}

    //public void SetTowards(Vector3 towards)
    //{
    //    mTowards = towards;
    //    if(mAgentGo != null)
    //    {
    //        mAgentGo.transform.rotation = Quaternion.LookRotation(towards);
    //        //Log.Logic(LogLevel.Info, "SetTowards-----{0}", towards);
    //    }
    //}


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

        Combo_Trigger.Initialize(this);
        EffectExcutorCtl.Initialize(this);
        MovementExcutorCtl.Initialize(this);

        MeterManager.Ins.RegisterMeterHandler(this);
        StatusMachine.Initialize(this);
        CustomInitialize();

        Dictionary<string, object> args = new Dictionary<string, object>();
        args.Add("cmdType", AgentCommandDefine.IDLE);
        args.Add("towards", GamePlayDefine.InputDirection_NONE);
        args.Add("triggerMeter", MeterManager.Ins.MeterIndex);
        StatusMachine.SwitchToStatus(AgentStatusDefine.IDLE, args);
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
    }

    public Agent(uint agentId)
    {
        mAgentId = agentId;
        AnimPlayer = new AgentAnimPlayer();
        StatusGraph = DataCenter.Ins.AgentStatusGraphCenter.GetAgentStatusGraph(mAgentId);
        StatusMachine = new AgentStatusMachine();
        Combo_Trigger = new ComboTrigger();
        EffectExcutorCtl = new EffectExcutorController();
        MovementExcutorCtl = new MovementExcutorController();
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

    public void OnMeterEnter(int meterIndex)
    {
        StatusMachine.OnMeterEnter(meterIndex);
        Combo_Trigger.OnMeterEnter(meterIndex);
        EffectExcutorCtl.OnMeterEnter(meterIndex);
        MovementExcutorCtl.OnMeterEnter(meterIndex);
    }

    
    public void OnMeterEnd(int meterIndex)
    { 
        StatusMachine.OnMeterEnd(meterIndex);
        Combo_Trigger.OnMeterEnd(meterIndex);
        EffectExcutorCtl.OnMeterEnd(meterIndex);
        MovementExcutorCtl.OnMeterEnd(meterIndex);
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
    }
}
