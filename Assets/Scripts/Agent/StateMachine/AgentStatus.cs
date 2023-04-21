using System.Collections.Generic;
using UnityEngine;

public abstract class AgentStatus : IAgentStatus
{
    /// <summary>
    /// �л�״̬�Ĵ�����
    /// </summary>
    protected ChangeStatusDelegate ChangeStatus;


    protected Agent mAgent;

    /// <summary>
    /// ���봦����
    /// </summary>
    protected IInputHandle mInputHandle;

    /// <summary>
    /// ��ǰ����״̬�Ľ�����
    /// TODO:�����Ƿ���Բ���������
    /// </summary>
    protected int mCurAnimStateEndMeter;

    /// <summary>
    /// �ȴ�ִ�е�ָ���
    /// </summary>
    protected AgentInputCommandBuffer cmdBuffer;

    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        cmdBuffer = new AgentInputCommandBuffer();
    }

    public virtual void CustomInitialize()
    {

    }

    public abstract string GetStatusName();

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
            mCurAnimStateEndMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, state.stateMeterLen);
            float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, mCurAnimStateEndMeter);
            mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, state.layer, state.normalizedTime, duration, state.animLen, totalMeterTime);
        }
    }

    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}--cur meter:{1}", GetStatusName(), MeterManager.Ins.MeterIndex);
        mInputHandle.SetEnable(true);
    }


    public virtual void OnExit() 
    {
        mInputHandle.SetEnable(false);
        cmdBuffer.ClearCommandBuffer();
    }

    public virtual void Dispose()
    {
        ChangeStatus = null;
        mAgent = null;
        mInputHandle = null;
        mCurAnimStateEndMeter = 0;
        if(cmdBuffer != null)
        {
            cmdBuffer.Dispose();
            cmdBuffer = null;
        }
    }

    protected abstract void CommandHandleOnMeter(int meterIndex);

    public virtual void OnMeter(int meterIndex)
    {
        CommandHandleOnMeter(meterIndex);
        cmdBuffer.ClearCommandBuffer();
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
    public void ProgressWaitOnCommand(float waitMeterProgress, AgentInputCommand cmd)
    {
        // ��ǰ�ĵ�ʣ��ʱ��
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        // ��ǰ�ĵ���ʱ��
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex+1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, ��ǰ�ĵ���ʱ��<=0, ��ǰ��:{0}", MeterManager.Ins.MeterIndex);
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
    public void TimeWaitOnCommand(float waitTime, AgentInputCommand cmd)
    {
        // ��ǰ�ĵ�ʣ��ʱ��
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);

        if (timeToNextMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "TimeWaitOnCommand Error, ��ǰ�ĵ���ʱ��<=0, ��ǰ��:{0}", MeterManager.Ins.MeterIndex);
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

    public void DelayToMeterExcuteCommand(AgentInputCommand cmd)
    {
        cmdBuffer.AddInputCommand(cmd);
    }

    protected void ExcuteCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;

        ExcuteCommand(cmd.CmdType, cmd.Towards);
    }

    protected void ExcuteCommand(byte cmdType, Vector3 towards)
    {
        if (cmdType == AgentCommandDefine.IDLE)
        {
            ChangeStatus(AgentStatusDefine.IDLE);
        }
        else if (cmdType == AgentCommandDefine.RUN)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            ChangeStatus(AgentStatusDefine.RUN, args);
        }
        else if (cmdType == AgentCommandDefine.DASH)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            ChangeStatus(AgentStatusDefine.DASH, args);
        }
        else if (cmdType == AgentCommandDefine.ATTACK_HARD || cmdType == AgentCommandDefine.ATTACK_LIGHT)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            ChangeStatus(AgentStatusDefine.ATTACK, args);
        }
        else if (cmdType == AgentCommandDefine.BE_HIT)
        {
            ChangeStatus(AgentStatusDefine.BE_HIT);
        }
    }

    protected virtual void CustomOnCommand(AgentInputCommand cmd) { }

    public void OnCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;

        CustomOnCommand(cmd);
    }
}
