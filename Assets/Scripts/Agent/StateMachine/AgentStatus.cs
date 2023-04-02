using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;
    protected AgentStateAnimQueue mStateAnimQueue;
    protected MeterTimer mTimer;

    /// <summary>
    /// �ȴ�ִ�е�ָ���
    /// ÿһλ����һ��ָ��
    /// ���λ������������չΪint
    /// </summary>
    protected byte commands;

    /// <summary>
    /// ���ָ��
    /// </summary>
    /// <param name="command"></param>
    protected void AddCommand(byte command)
    {
        commands |= command;
    }

    /// <summary>
    /// ���ָ���
    /// </summary>
    protected void ClearCommands()
    {
        commands = 0;
    }

    /// <summary>
    /// �ж��Ƿ���ָ��
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

        float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
        if(duration > 0)
        {
            mAgent.AnimPlayer.CrossFadeToStateInNormalizedTime(stateName, state.stateLen, state.layer, state.normalizedTime, duration);
        }
    }

    /// <summary>
    /// TODO������ʹ�ý��Ķ�ʱ��
    /// ��Ϊ�ڽ�������ʱ���ж��Ƿ��п�ִ��ָ��Ƿ���Դ��
    /// </summary>
    /// <param name="meterLen"></param>
    private void SetTimer(int meterLen)
    {
        mTimer = MeterTimerCenter.Ins.SetTimer(meterLen, 1, OnMeterTimerEnd);
    }

    public virtual void OnEnter(Dictionary<string, object> context)
    {
        AgentStatusInfo statusInfo = mAgent.StatusGraph.GetStatusInfo(GetStatusName());
        if (statusInfo == null)
            return;

        mStateAnimQueue = new AgentStateAnimQueue();
        mStateAnimQueue.Initialize(statusInfo);

        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        SetTimer(state.stateMeterLen);
        AgentStatusCrossFadeToState(state);
    }

    public virtual void OnExit()
    {
        if(mTimer != null)
        {
            MeterTimerCenter.Ins.RemoveTimer(mTimer);
        }
    }

    /// <summary>
    /// TODO: ������߼�ҪŲ�������status�У�ÿ��status ��������
    /// </summary>
    private void OnMeterTimerEnd()
    {
        int ret = mStateAnimQueue.MoveNext();
        if (ret == AgentAnimDefine.AnimQueue_AnimKeep || ret == AgentAnimDefine.AnimQueue_AnimLoop)
        {
            AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
            
            float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
            SetTimer(state.stateMeterLen);
            mAgent.AnimPlayer.UpdateAnimSpeed(state.stateLen / duration);
            //Log.Logic(LogLevel.Info, "change speed on  state Loop ----- speed:{0},duration:{1}", state.stateLen / duration, duration);
        }
        else if (ret == AgentAnimDefine.AnimQueue_AnimMoveNext)
        {
            AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
            SetTimer(state.stateMeterLen);
            AgentStatusCrossFadeToState(state);
            //float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
            //Log.Logic(LogLevel.Info, "change speed on chan  ge state change  ----- speed:{0}, duration:{1}", state.stateLen / duration, duration);
        }
        else if (ret == AgentAnimDefine.AnimQueue_AnimEnd)
        {

        }
    }

    public void OnMeter(int meterIndex)
    {

    }

    public virtual void OnUpdate(float deltaTime)
    {

    }
}
