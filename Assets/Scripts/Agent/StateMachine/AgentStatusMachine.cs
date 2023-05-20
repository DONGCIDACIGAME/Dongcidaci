using System.Collections.Generic;

/// <summary>
///  TODO-后面改为事件
/// </summary>
/// <param name="stateName"></param>
/// <param name="context"></param>
public delegate void ChangeStatusDelegate(string stateName,  Dictionary<string, object> context = null);

public class AgentStatusMachine : IMeterHandler
{
    public IAgentStatus CurStatus { get; private set; }
    private Dictionary<string, IAgentStatus> mStatusMap;
    private Agent mAgent;


    private AgentInputCommand lastInputCmd;
    
    public AgentStatusMachine()
    {
        mStatusMap = new Dictionary<string, IAgentStatus>();
    }

    public void OnMeterEnter(int meterIndex)
    {
        if (CurStatus != null)
        {
            CurStatus.OnMeterEnter(meterIndex);
        }
    }

    public void OnMeterEnd(int meterIndex)
    {
        if (CurStatus != null)
        {
            CurStatus.OnMeterEnd(meterIndex);
        }
    }

    private void AddStatus(string statusName, AgentStatus status)
    {
        if(string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayerStateMachine AddState Failed, status name is null or empty, please check!");
            return;
        }

        if(status == null)
        {
            Log.Error(LogLevel.Normal, "PlayerStateMachine AddState Failed, status {0} is null, please check!", statusName);
            return;
        }

        if(mStatusMap.ContainsKey(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayerStateMachine AddState Failed, repeate add status {0}!", statusName);
            return;
        }

        mStatusMap.Add(statusName, status);
        status.Initialize(mAgent, SwitchToStatus);
        status.CustomInitialize();
    }

    public void Initialize(Agent agt)
    {
        mAgent = agt;
        AddStatus(AgentStatusDefine.IDLE, new AgentStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new AgentStatus_Run());
        AddStatus(AgentStatusDefine.ATTACK, new AgentStatus_Attack());
        AddStatus(AgentStatusDefine.DASH, new AgentStatus_Dash());
        AddStatus(AgentStatusDefine.TRANSFER, new AgentStatus_Transfer());
    }


    public void SwitchToStatus(string statusName, Dictionary<string, object> context)
    {
        if(CurStatus != null && CurStatus.GetStatusName().Equals(statusName))
        {
            return;
        }

        if(mStatusMap.TryGetValue(statusName,out IAgentStatus newStatus))
        {
            if (CurStatus != null)
            {
                CurStatus.OnExit();
            }

            newStatus.OnEnter(context);
            CurStatus = newStatus;
        }
        else
        {
            Log.Error(LogLevel.Normal, "AgentStatusMachine ChangeToStatus Failed, status {0} not registered!", statusName);
        }
    }

    public void UpdateInputHandle(float deltaTime)
    {

    }

    public void OnUpdate(float deltaTime)
    {
        if(CurStatus != null)
        {
            CurStatus.OnUpdate(deltaTime);
        }
    }

    public void Dispose()
    {
        CurStatus = null;
        mStatusMap = null;
    }
}
