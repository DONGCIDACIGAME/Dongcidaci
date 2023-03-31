using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;

    protected AgentStateAnimQueue mStateAnimQueue;

    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
    }

    public abstract string GetStatusName();

    public abstract void OnAction(int action);

    protected void AgentStatusCrossFadeToState(AgentAnimStateInfo state)
    {
        if(state == null)
        {
            Log.Error(LogLevel.Normal, "AgentStatusCrossFadeToState failed, state is null!");
            return;
        }

        string stateName = state.stateName;

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "AgentStatusCrossFadeToState failed, stateName is null or empty!");
            return;
        }

        float duration = MeterManager.Ins.GetTimeToNextBaseMeter(state.stateMeterLen);
        if(duration > 0)
        {
            mAgent.AnimPlayer.CrossFadeToStateInNormalizedTime(stateName, state.stateLen, state.layer, state.normalizedTime, duration);
        }
    }

    public virtual void OnEnter(Dictionary<string, object> context)
    {
        Log.Logic(LogLevel.Info, "Enter Agent Status:[{0}]", GetStatusName());

        AgentStatusInfo statusInfo = mAgent.StatusGraph.GetStatusInfo(GetStatusName());
        if (statusInfo == null)
            return;

        mStateAnimQueue = new AgentStateAnimQueue();
        mStateAnimQueue.Initialize(statusInfo);

        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        AgentStatusCrossFadeToState(state);
    }

    public abstract void OnExit();

    public virtual void OnUpdate(float deltaTime)
    {
        int ret = mStateAnimQueue.MoveNext(deltaTime);
        if (ret == AgentAnimDefine.AnimQueue_AnimLoop)
        {
            AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
            float duration = MeterManager.Ins.GetTimeToNextBaseMeter(state.stateMeterLen);
            mAgent.AnimPlayer.UpdateAnimSpeed(state.stateLen/duration);
        }

        if(ret == AgentAnimDefine.AnimQueue_AnimChange)
        {
            AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
            AgentStatusCrossFadeToState(state);
        }
    }

}
