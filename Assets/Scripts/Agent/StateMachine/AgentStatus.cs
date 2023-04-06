using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;
    protected AgentStateAnimQueue mStateAnimQueue;
    protected int mCurAnimStateMeterLen;
    protected int mCurAnimStateMeterRecord;

    /// <summary>
    /// 等待执行的指令集合
    /// </summary>
    protected AgentCommandBuffer cmdBuffer;

    public virtual void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        cmdBuffer = new AgentCommandBuffer();
        mStateAnimQueue = new AgentStateAnimQueue();
        AgentStatusInfo statusInfo = mAgent.StatusGraph.GetStatusInfo(GetStatusName());
        mStateAnimQueue.Initialize(statusInfo);
    }

    public abstract string GetStatusName();

    /// <summary>
    /// TODO：所有子状态的这个逻辑，要重构
    /// </summary>
    /// <param name="cmds"></param>
    public abstract void OnCommands(AgentCommandBuffer cmds);

    protected void SetAnimStateMeterTimer(int meterLen)
    {
        mCurAnimStateMeterLen = meterLen;
        mCurAnimStateMeterRecord = 0;
    }

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

        float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
        if(duration > 0)
        {
            mAgent.AnimPlayer.CrossFadeToStateInNormalizedTime(stateName, state.stateLen, state.layer, state.normalizedTime, duration);
        }
    }

    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}", GetStatusName());
    }

    public virtual void OnExit() { }

    protected void StartAnimQueue()
    {
        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        if (state == null)
            return;

        AgentStatusCrossFadeToState(state);
        SetAnimStateMeterTimer(state.stateMeterLen);
    }

    protected void ResetAnimQueue()
    {
        mStateAnimQueue.Reset();
    }

    protected void AnimQueueMoveOn()
    {
        if (mStateAnimQueue == null)
            return;

        int ret = mStateAnimQueue.MoveNext();
        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();

        if (state == null)
            return;

        if (ret == AgentAnimDefine.AnimQueue_AnimKeep)
        {
            Log.Logic(LogLevel.Info, "UpdateAnimSpeed---------cur progress:{0}", mAgent.AnimPlayer.CurStateProgress);
            float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
            mAgent.AnimPlayer.UpdateAnimSpeedWithFix(state.layer, state.stateLen, duration);
        }
        else if (ret == AgentAnimDefine.AnimQueue_AnimMoveNext)
        {
            Log.Logic(LogLevel.Info, "AnimQueue_AnimMoveNext---------next state:{0}", state.stateName);
            AgentStatusCrossFadeToState(state);
        }

        SetAnimStateMeterTimer(state.stateMeterLen);
    }

    protected abstract void ActionHandleOnMeter(int meterIndex);

    public virtual void OnMeter(int meterIndex)
    {
        ActionHandleOnMeter(meterIndex);
        cmdBuffer.ClearBuffer();
    }

    public virtual void OnUpdate(float deltaTime)
    {

    }
}
