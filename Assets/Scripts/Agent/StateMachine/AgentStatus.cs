using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;
    protected AgentStateAnimQueue mStateAnimQueue;
    protected MeterTimer mTimer;

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
            Log.Error(LogLevel.Info, "AgentStatusCrossFadeToState:{0},duration:{1} ", stateName, duration);
        }
    }

    private void SetTimer(int meterLen)
    {
        mTimer = MeterTimerCenter.Ins.SetTimer(meterLen, 1, OnMeterTimerEnd);
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

    private void OnMeterTimerEnd()
    {
        int ret = mStateAnimQueue.MoveNext();
        Log.Error(LogLevel.Info, "ret={0}", ret);
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

    public void OnMeter()
    {

    }

    public virtual void OnUpdate(float deltaTime)
    {
        //time += deltaTime;
        //// 本次动画结束
        //if (mTimer.working && mTimer.CheckEnd())
        //{
        //    Log.Error(LogLevel.Info, "cur time:{0}", time);
        //    time = 0;
        //    int ret = mStateAnimQueue.MoveNext();

        //    if (ret == AgentAnimDefine.AnimQueue_AnimKeep || ret == AgentAnimDefine.AnimQueue_AnimLoop)
        //    {
        //        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        //        SetAnimTimer(state.stateMeterLen);
        //        float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
        //        mAgent.AnimPlayer.UpdateAnimSpeed(state.stateLen / duration);
        //        Log.Logic(LogLevel.Info, "change speed on  state Loop ----- speed:{0},duration:{1}", state.stateLen / duration, duration);
        //    }
        //    else if (ret == AgentAnimDefine.AnimQueue_AnimMoveNext)
        //    {
        //        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        //        SetAnimTimer(state.stateMeterLen);
        //        AgentStatusCrossFadeToState(state);
        //        float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
        //        Log.Logic(LogLevel.Info, "change speed on chan  ge state change  ----- speed:{0}, duration:{1}", state.stateLen / duration, duration);
        //    }
        //    else if (ret == AgentAnimDefine.AnimQueue_AnimEnd)
        //    {

        //    }
        //}


    }

}
