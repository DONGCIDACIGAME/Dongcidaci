using System.Collections.Generic;

public abstract class AgentStatus : IAgentStatus
{
    protected ChangeStatusDelegate ChangeStatus;
    protected Agent mAgent;
    protected AgentStateAnimQueue mStateAnimQueue;
    protected AgentMoveControl mMoveControl;
    protected IInputHandle mInputHandle;
    protected int mCurAnimStateEndMeter;

    /// <summary>
    /// �ȴ�ִ�е�ָ���
    /// </summary>
    protected AgentCommandBuffer cmdBuffer;

    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        cmdBuffer = new AgentCommandBuffer();
        mStateAnimQueue = new AgentStateAnimQueue();
        AgentStatusInfo statusInfo = mAgent.StatusGraph.GetStatusInfo(GetStatusName());
        mStateAnimQueue.Initialize(statusInfo);
    }

    public virtual void CustomInitialize()
    {

    }

    public abstract string GetStatusName();

    /// <summary>
    /// �յ�ָ���Ĵ����߼�
    /// </summary>
    /// <param name="cmds"></param>
    public abstract void OnCommands(AgentCommandBuffer cmds);

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
            mCurAnimStateEndMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.BaseMeterIndex, state.stateMeterLen);
            float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.BaseMeterIndex, mCurAnimStateEndMeter);
            mAgent.AnimPlayer.CrossFadeToState(stateName, state.layer, state.normalizedTime, duration, state.animLen, totalMeterTime);
        }
    }

    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}", GetStatusName());
        mInputHandle.SetEnable(true);
    }


    public virtual void OnExit() 
    {
        mInputHandle.SetEnable(false);
    }

    protected void StartAnimQueue()
    {
        AgentAnimStateInfo state = mStateAnimQueue.GetCurAnimState();
        if (state == null)
            return;

        AgentStatusCrossFadeToState(state);
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
            //Log.Logic(LogLevel.Info, "UpdateAnimSpeed---------cur progress:{0}", mAgent.AnimPlayer.CurStateProgress);
            float duration = MeterManager.Ins.GetTimeToBaseMeter(state.stateMeterLen);
            mCurAnimStateEndMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.BaseMeterIndex, state.stateMeterLen);
            mAgent.AnimPlayer.UpdateAnimSpeedWithFix(state.layer,state.animLen, duration);
        }
        else if (ret == AgentAnimDefine.AnimQueue_AnimMoveNext)
        {
            //Log.Logic(LogLevel.Info, "AnimQueue_AnimMoveNext---------next state:{0}", state.stateName);
            AgentStatusCrossFadeToState(state);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmds">ָ��buffer</param>
    /// <param name="cmd">Ŀ��ָ��</param>
    /// <param name="toStatus">ת������״̬</param>
    /// <returns>�Ƿ������������ָ��</returns>
    protected bool CommonHandleOnCmd(AgentCommandBuffer cmds,byte cmd, string toStatus)
    {
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        int curMeter = MeterManager.Ins.BaseMeterIndex;
        int targetMeter = MeterManager.Ins.GetMeterIndex(curMeter, 1);
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(curMeter, targetMeter);
        if (timeOfCurrentMeter < 0)
            return true;

        // �����⵽ָ��
        if (cmds.HasCommand(cmd))
        {
            if (timeToNextMeter / timeOfCurrentMeter >= GamePlayDefine.MinMeterProgressOnCmd)
            {
                ChangeStatus(toStatus);
                return true;
            }

            else if (timeToNextMeter <= GamePlayDefine.WaitMeterMaxTimeOnCmd)
            {
                cmdBuffer.AddCommand(cmd);
                return true;
            }
        }

        return false;
    }

    protected abstract void CommandHandleOnMeter(int meterIndex);

    public virtual void OnMeter(int meterIndex)
    {
        CommandHandleOnMeter(meterIndex);
        cmdBuffer.ClearBuffer();
        //Log.Error(LogLevel.Info, "Meter--{0}",meterIndex);
    }

    public virtual void OnUpdate(float deltaTime)
    {
        //Log.Error(LogLevel.Info, "OnUpdate anim progress-----------------------------------------------{0}", mAgent.AnimPlayer.CurStateProgress);
    }


    /// <summary>
    /// ���ݽ��Ľ��ȶ������������ȴ�
    /// ������ĵ�ʣ��ʱ��ռ��>=waitMeterProgress,��ֱ��ִ��,���������ִ��
    /// ��������ȴ���һ��ִ��
    /// </summary>
    /// <param name="waitMeterProgress"></param>
    public void ProgressWaitOnCommand(float waitMeterProgress, byte cmd)
    {
        // ��ǰ�ĵ�ʣ��ʱ��
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        // ��ǰ�ĵ���ʱ��
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.BaseMeterIndex, MeterManager.Ins.BaseMeterIndex+1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, ��ǰ�ĵ���ʱ��<=0, ��ǰ��:{0}", MeterManager.Ins.BaseMeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;

        if(progress >= waitMeterProgress)
        {
            ExcuteCommand(cmd);
        }
        else
        {
            DelayToMeterExcuteCommand(cmd);
        }
    }

    /// <summary>
    /// ���ݱ���ʣ��ʱ��������������ȴ�
    /// �������ʣ��ʱ��<waitTime,��ֱ��ִ�У�����ȴ���һ��ִ��
    /// </summary>
    /// <param name="waitTime"></param>
    public void TimeWaitOnCommand(float waitTime, byte cmd)
    {
        // ��ǰ�ĵ�ʣ��ʱ��
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);

        if (timeToNextMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "TimeWaitOnCommand Error, ��ǰ�ĵ���ʱ��<=0, ��ǰ��:{0}", MeterManager.Ins.BaseMeterIndex);
            return;
        }

        if (timeToNextMeter <= waitTime)
        {
            ExcuteCommand(cmd);
        }
        else
        {
            DelayToMeterExcuteCommand(cmd);
        }
    }

    /// <summary>
    /// �ӳٵ����Ĵ�ִ��ָ��
    /// </summary>
    /// <param name="cmd"></param>
    public void DelayToMeterExcuteCommand(byte cmd)
    {
        // ��ָ�����ָ�����
        // ����������ʱ��ᴦ��ָ������е�ָ��
        cmdBuffer.AddCommand(cmd);
    }

    /// <summary>
    /// ����ִ��ָ��
    /// </summary>
    /// <param name="cmd"></param>
    public void ExcuteCommand(byte cmd)
    {
        if(cmd == AgentCommandDefine.IDLE)
        {
            ChangeStatus(AgentStatusDefine.IDLE);
        }
        else if (cmd == AgentCommandDefine.RUN)
        {
            ChangeStatus(AgentStatusDefine.RUN);
        }
        else if (cmd == AgentCommandDefine.ATTACK_HARD || cmd == AgentCommandDefine.ATTACK_LIGHT)
        {
            ChangeStatus(AgentStatusDefine.ATTACK);
        }
        else if (cmd == AgentCommandDefine.BE_HIT)
        {
            ChangeStatus(AgentStatusDefine.BE_HIT);
        }
    }
}
