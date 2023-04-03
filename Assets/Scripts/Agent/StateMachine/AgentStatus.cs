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
    /// 每一位代表一个指令
    /// 如果位数不够后续扩展为int
    /// </summary>
    protected byte commands;

    /// <summary>
    /// 添加指令
    /// </summary>
    /// <param name="command"></param>
    protected void AddCommand(byte command)
    {
        commands |= command;
    }

    protected byte PeekCommand()
    {
        for(int i = 0;i<8;i++)
        {
            int ret = AgentCommandDefine.COMMANDS[i] & commands;
            if (ret == 1)
            {
                return AgentCommandDefine.COMMANDS[i];
            }
        }

        return AgentCommandDefine.EMPTY;
    }

    /// <summary>
    /// 清除指令集合
    /// </summary>
    protected void ClearCommands()
    {
        commands = 0;
    }

    /// <summary>
    /// 判断是否有指令
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    protected bool HasCommand(byte command)
    {
        return (commands & command) == 1;
    }

    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt; 
    }

    public abstract string GetStatusName();

    public abstract void OnAction(byte action);

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
        AgentStatusInfo statusInfo = mAgent.StatusGraph.GetStatusInfo(GetStatusName());
        if (statusInfo != null)
        {
            StartAnimQueue(statusInfo);
        }

    }

    public virtual void OnExit()
    {

    }

    protected void StartAnimQueue(AgentStatusInfo statusInfo)
    {
        mStateAnimQueue = new AgentStateAnimQueue();
        mStateAnimQueue.Initialize(statusInfo);

        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        if (state == null)
            return;

        AgentStatusCrossFadeToState(state);
        SetAnimStateMeterTimer(state.stateMeterLen);
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
            mAgent.AnimPlayer.UpdateAnimSpeed(state.stateLen / duration);
        }
        else if (ret == AgentAnimDefine.AnimQueue_AnimMoveNext)
        {
            AgentStatusCrossFadeToState(state);
        }

        SetAnimStateMeterTimer(state.stateMeterLen);
    }

    protected abstract void ActionHandleOnMeter(int meterIndex);

    public virtual void OnMeter(int meterIndex)
    {
        ActionHandleOnMeter(meterIndex);
        ClearCommands();
    }

    public virtual void OnUpdate(float deltaTime)
    {

    }
}
