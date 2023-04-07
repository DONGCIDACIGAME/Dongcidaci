using System.Collections.Generic;
using GameEngine;

public delegate void ChangeStatusDelegate(string stateName,  Dictionary<string, object> context = null);

public class AgentStatusMachine
{
    private IAgentStatus mCurStatus;
    private Dictionary<string, IAgentStatus> mStatusMap;
    private Agent mAgent;

    public AgentStatusMachine()
    {
        mStatusMap = new Dictionary<string, IAgentStatus>();
    }

    public void OnMeter(int meterIndex)
    {
        if(mCurStatus != null)
        {
            mCurStatus.OnMeter(meterIndex);
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
    }

    public void Initialize(Agent agt)
    {
        mAgent = agt;
        AddStatus(AgentStatusDefine.IDLE, new AgentStatus_Idle());
        AddStatus(AgentStatusDefine.RUN, new AgentStatus_Run());
        AddStatus(AgentStatusDefine.ATTACK, new AgentStatus_Attack());

        SwitchToStatus(AgentStatusDefine.IDLE, null);
    }

    public void OnCommands(AgentCommandBuffer cmds)
    {
        if(mCurStatus != null)
        {
            mCurStatus.OnCommands(cmds);
        }
    }

    public void SwitchToStatus(string statusName, Dictionary<string, object> context)
    {
        if(mCurStatus != null && mCurStatus.GetStatusName().Equals(statusName))
        {
            return;
        }

        if(mStatusMap.TryGetValue(statusName,out IAgentStatus newStatus))
        {
            if (mCurStatus != null)
            {
                mCurStatus.OnExit();
            }

            newStatus.OnEnter(context);
            mCurStatus = newStatus;
        }
        else
        {
            Log.Error(LogLevel.Normal, "AgentStatusMachine ChangeToStatus Failed, status {0} not registered!", statusName);
        }
    }

    public void OnUpdate(float deltaTime)
    {
        if(mCurStatus != null)
        {
            mCurStatus.OnUpdate(deltaTime);
        }
    }



    public void Dispose()
    {
        mCurStatus = null;
        mStatusMap = null;
    }
}
