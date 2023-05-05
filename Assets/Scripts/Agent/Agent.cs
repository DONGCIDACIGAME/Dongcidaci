using UnityEngine;

public abstract class Agent : IEntity, IMeterHandler
{
    /// <summary>
    /// ��ɫid
    /// </summary>
    protected uint mAgentId;
    /// <summary>
    /// ʵ��id
    /// </summary>
    protected int mEntityId;
    /// <summary>
    /// ��ɫ����Ϸ��
    /// </summary>
    protected GameObject mAgentGo;

    // ���׷�������Ŀ��(��ֹ��������λ��)
    //protected VirtualCamTarget mVirtualCamTarget;

    /// <summary>
    /// �������ſ�����
    /// </summary>
    public AgentAnimPlayer AnimPlayer;

    /// <summary>
    /// ����״̬��
    /// </summary>
    protected AgentStatusMachine StatusMachine;

    /// <summary>
    /// �ƶ�����
    /// </summary>
    public AgentMoveControl MoveControl;

    /// <summary>
    /// ��ɫ״̬��Ϣ��
    /// </summary>
    public AgentStatusGraph StatusGraph;

    /// <summary>
    /// Combo������
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
    // ��ɫ�ƶ��ٶ�
    protected float mSpeed;
    // ��ɫ�ĳ���ٶ�
    protected float mDashDistance;
    // ��ɫ����
    protected Vector3 mTowards;
    // ��ɫλ��
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
    /// ���ؽ�ɫ������Ϣ
    /// </summary>
    /// <param name="agentId"></param>
    protected abstract void LoadAgentCfg(uint agentId);

    /// <summary>
    /// ���ؽ�ɫ���
    /// </summary>
    protected abstract void LoadAgentGo();

    /// <summary>
    /// �Զ���ĳ�ʼ��
    /// </summary>
    protected abstract void CustomInitialize();

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Initialize()
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
    /// ����
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

    public void OnMeter(int meterIndex)
    {
        StatusMachine.OnMeter(meterIndex);
    }

    /// <summary>
    /// ��¼��һ��ָ��
    /// ������һ������ָ���һ�����ݿ���
    /// </summary>
    private AgentInputCommand lastInputCmd;

    public void OnCommand(AgentInputCommand cmd)
    {
        if(cmd == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, AgentInputCommand is null!");
            return;
        }

        // ����ͬһ�ĵ�ͬһ��ָ�������
        if (cmd.Equals(lastInputCmd))
        {
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // ��¼��ε�ָ������
        AgentInputCommandPool.Ins.PushAgentInputCommand(lastInputCmd);
        lastInputCmd = AgentInputCommandPool.Ins.CreateAgentInputCommandCopy(cmd);

        if (StatusMachine == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, StatusMachine is null!");
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // ��ȡ��ǰ��status
        IAgentStatus curStatus = StatusMachine.CurStatus;
        if(curStatus == null)
        {
            Log.Error(LogLevel.Normal, "Agent OnCommand Error, cur status is null!");
            AgentInputCommandPool.Ins.PushAgentInputCommand(cmd);
            return;
        }

        // ���������combo����ִ��combo�߼�������ִ�е���ָ����߼�
        // $$$$$$$$$$$$$$$������߼����ԣ�Ӧ����combohandlerͨ����ָ�����ƥ�䣬�����һ��ƥ���combomove��Ȼ�󸽴���cmd������status
        // ���򴥷���combo��ָ����޷���ȷִ��ָ����߼���

        if (ComboHandler.TryTriggerCombo(cmd.CmdType, cmd.TriggerMeter, out Combo combo, out ComboMove comboMove))
        {
            curStatus.OnComboMove(combo, comboMove, cmd.Towards);
        }
        else
        {
            curStatus.OnCommand(cmd);
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
