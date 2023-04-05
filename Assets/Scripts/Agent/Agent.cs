using UnityEngine;

public abstract class Agent : IEntity
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
    /// <summary>
    /// �������ദ����
    /// </summary>
    protected IMeterHandler mBaseMeterHandler;

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

    public Vector3 GetTowards()
    {
        return mTowards;
    }

    public void SetTowards(Vector3 towards)
    {
        mTowards = towards;
        if(mAgentGo != null)
        {
            mAgentGo.transform.rotation = Quaternion.LookRotation(towards);
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
    }

    public virtual void Dispose()
    {
        EntityManager.Ins.RemoveEntity(this);
        mAgentGo = null;
        AnimPlayer = null;
    }

    public Agent(uint agentId)
    {
        mAgentId = agentId;
        mEntityId = EntityManager.Ins.AddEntity(this);
        AnimPlayer = new AgentAnimPlayer();
    }

    public void OnMeter(int meterIndex)
    {
        StatusMachine.OnMeter(meterIndex);
    }

    public void OnCommands(AgentCommandBuffer cmds)
    {
        if(StatusMachine != null)
        {
            StatusMachine.OnCommands(cmds);
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
    }
}
