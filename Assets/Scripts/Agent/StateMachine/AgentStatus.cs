using System.Collections.Generic;
using UnityEngine;

public abstract class AgentStatus : IAgentStatus
{
    /// <summary>
    /// 切换状态的代理方法
    /// </summary>
    protected ChangeStatusDelegate ChangeStatus;


    protected Agent mAgent;

    /// <summary>
    /// 输入处理器
    /// </summary>
    protected IInputHandle mInputHandle;

    /// <summary>
    /// 当前动画状态的结束拍
    /// TODO:这里是否可以不放在这里
    /// </summary>
    protected int mCurAnimStateEndMeter;

    /// <summary>
    /// 等待执行的指令集合
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
    /// 根据节拍进度对命令处理的条件等待
    /// 如果本拍的剩余时间占比>=waitMeterProgress,就直接执行,否则等下拍执行
    /// 其他情况等待下一拍执行
    /// </summary>
    /// <param name="waitMeterProgress"></param>
    public void ProgressWaitOnCommand(float waitMeterProgress, AgentInputCommand cmd)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex+1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
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
    /// 根据本拍剩余时间对命令处理的条件等待
    /// 如果本拍剩余时间<waitTime,就直接执行，否则等待下一拍执行
    /// </summary>
    /// <param name="waitTime"></param>
    public void TimeWaitOnCommand(float waitTime, AgentInputCommand cmd)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);

        if (timeToNextMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "TimeWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
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
