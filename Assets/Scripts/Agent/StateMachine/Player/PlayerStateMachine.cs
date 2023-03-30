using System.Collections.Generic;
using GameEngine;

public delegate void ChangeStatusDelegate(string stateName,  Dictionary<string, object> context = null);

public class PlayerStateMachine
{
    private IAgentStatus mCurStatus;
    private Dictionary<string, IAgentStatus> mStatusMap;
    private Agent mAgent;

    public PlayerStateMachine(Agent agt)
    {
        mAgent = agt;
    }

    private void AddState(string stateName, AgentStatus state)
    {
        if(string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayerStateMachine AddState Failed, state name is null or empty, please check!");
            return;
        }

        if(state == null)
        {
            Log.Error(LogLevel.Normal, "PlayerStateMachine AddState Failed, state {0} is null, please check!", stateName);
            return;
        }

        state.Initialize(mAgent, SwitchToStatus);
    }

    public void Initialize()
    {
        mStatusMap = new Dictionary<string, IAgentStatus>();
        mStatusMap.Add(AgentStatusDefine.IDLE, new AgentStatus_Idle());
        mStatusMap.Add(AgentStatusDefine.RUN, new AgentStatus_Run());
    }

    public void OnAction(int action)
    {
        if(mCurStatus != null)
        {
            mCurStatus.OnAction(action);
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
