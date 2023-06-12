using GameEngine;
using System.Collections.Generic;

public class AgentStatusMachine : IMeterHandler
{
    private GameEventListener mEventListener;
    public AgentStatus CurStatus { get; private set; }
    private Dictionary<string, AgentStatus> mStatusMap;
    private Agent mAgent;
    
    public AgentStatusMachine()
    {
        mStatusMap = new Dictionary<string, AgentStatus>();
        mEventListener = new GameEventListener();
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
        status.Initialize(mAgent);
        status.CustomInitialize();
    }

    public void Initialize(Agent agt)
    {
        mAgent = agt;
        
        mEventListener.Listen<uint, string, Dictionary<string, object>>("ChangeAgentStatus", SwitchToStatus);

        AddStatus(AgentStatusDefine.IDLE, new AgentStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new AgentStatus_Run());
        AddStatus(AgentStatusDefine.ATTACK, new AgentStatus_Attack());
        AddStatus(AgentStatusDefine.DASH, new AgentStatus_Dash());
        AddStatus(AgentStatusDefine.TRANSFER, new AgentStatus_Transfer());
    }


    public void SwitchToStatus(uint agentId,string statusName, Dictionary<string, object> context)
    {
        if (agentId != mAgent.GetAgentId())
            return;

        if(CurStatus != null && CurStatus.GetStatusName().Equals(statusName))
        {
            return;
        }

        if(mStatusMap.TryGetValue(statusName,out AgentStatus newStatus))
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
        if(mEventListener != null)
        {
            mEventListener.ClearAllEventListen();
            mEventListener = null;
        }
    }
}
