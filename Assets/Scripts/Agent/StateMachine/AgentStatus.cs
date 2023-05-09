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
    /// 步进式动画驱动器
    /// </summary>
    protected StepLoopAnimDriver mStepLoopAnimDriver;

    /// <summary>
    /// 自定义动画驱动器
    /// </summary>
    protected CustomAnimDriver mCustomAnimDriver;

    /// <summary>
    /// 当前动画结束节拍index
    /// </summary>
    protected int mCurAnimStateEndMeter;

    /// <summary>
    /// 等待执行的指令集缓存区
    /// </summary>
    protected AgentInputCommandBuffer cmdBuffer;

    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        cmdBuffer = new AgentInputCommandBuffer();
        mStepLoopAnimDriver = new StepLoopAnimDriver(mAgent, GetStatusName());
        mCustomAnimDriver = new CustomAnimDriver(mAgent, GetStatusName());
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
        mStepLoopAnimDriver.Reset();
    }

    protected virtual void CustomDispose() { }

    public void Dispose()
    {
        ChangeStatus = null;
        mAgent = null;
        mCurAnimStateEndMeter = 0;

        if(mInputHandle != null)
        {
            mInputHandle = null;
        }


        if(cmdBuffer != null)
        {
            cmdBuffer.Dispose();
            cmdBuffer = null;
        }

        if (mStepLoopAnimDriver != null)
        {
            mStepLoopAnimDriver.Dispose();
            mStepLoopAnimDriver = null;
        }

        if (mCustomAnimDriver != null)
        {
            mCustomAnimDriver.Dispose();
            mCustomAnimDriver = null;
        }

        CustomDispose();
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
    /// 如果本拍的剩余时间占比=waitMeterProgress,就直接执指令，否则等下拍执行指令
    /// 其他情况等待下一拍执行
    /// </summary>
    /// <param name="waitMeterProgress"></param>
    public void ProgressWaitOnCommand(float waitMeterProgress, AgentInputCommand cmd)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToBaseMeter(1);
        // 当前拍的总时长
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex+1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时长=0, 当前节拍：{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;

        if(progress >= waitMeterProgress)
        {
            ChangeStatusOnNormalCommand(cmd);
        }
        else
        {
            PushInputCommandToBuffer(cmd.CmdType, cmd.Towards);
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
            Log.Error(LogLevel.Normal, "TimeWaitOnCommand Error, 当前拍的总时长=0, 当前节拍：{0}", MeterManager.Ins.MeterIndex);
            return;
        }

        if (timeToNextMeter <= waitTime)
        {
            ChangeStatusOnNormalCommand(cmd);
        }
        else
        {
            PushInputCommandToBuffer(cmd.CmdType, cmd.Towards);
        }
    }

    /// <summary>
    /// 输入指令暂存到buffer里，等待后续处理
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    public void PushInputCommandToBuffer(byte cmdType, Vector3 towards)
    {
        cmdBuffer.AddInputCommand(cmdType, towards);
    }

    protected void ChangeStatusOnNormalCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;

        ChangeStatusOnNormalCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter);
    }

    /// <summary>
    /// 接受到指令时，切换到新状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    protected void ChangeStatusOnNormalCommand(byte cmdType, Vector3 towards, int triggerMeter)
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
        else if (cmdType == AgentCommandDefine.ATTACK_LONG || cmdType == AgentCommandDefine.ATTACK_SHORT)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            args.Add("triggerMeter", triggerMeter);
            ChangeStatus(AgentStatusDefine.ATTACK, args);
        }
        else if (cmdType == AgentCommandDefine.BE_HIT)
        {
            ChangeStatus(AgentStatusDefine.BE_HIT);
        }
    }

    protected void ChangeStatusOnComboCommand(AgentInputCommand cmd, TriggerableCombo combo)
    {
        if (cmd == null)
            return;

        if (combo == null)
            return;

        ChangeStatusOnComboCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, combo);
    }

    protected void ChangeStatusOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggerableCombo combo)
    {
        if (cmdType == AgentCommandDefine.IDLE || cmdType == AgentCommandDefine.RUN
            || cmdType == AgentCommandDefine.BE_HIT)
        {
            ChangeStatusOnNormalCommand(cmdType, towards, triggerMeter);
            Log.Error(LogLevel.Normal, "ChangeStatusOnComboCommand Exception, 指令类型[{0}]应该不是combo的触发指令, 错误触发的combo:{0}", cmdType, combo.GetComboName());
            return;
        }

        if (cmdType == AgentCommandDefine.DASH)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            args.Add("TriggeredCombo", combo);
            ChangeStatus(AgentStatusDefine.DASH, args);
        }
        else if (cmdType == AgentCommandDefine.ATTACK_LONG || cmdType == AgentCommandDefine.ATTACK_SHORT)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            args.Add("triggerMeter", triggerMeter);
            args.Add("combo", combo);
            ChangeStatus(AgentStatusDefine.ATTACK, args);
        }
    }

    protected virtual void CustomOnNormalCommand(AgentInputCommand cmd) { }

    public void OnNormalCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
        {
            Log.Error(LogLevel.Normal, "OnNormalCommand Error, cmd is null!");
            return;
        }

        CustomOnNormalCommand(cmd);
    }

    protected virtual void CustomOnComboCommand(AgentInputCommand cmd, TriggerableCombo combo) { }
   
    public void OnComboCommand(AgentInputCommand cmd, TriggerableCombo combo)
    {
        if (cmd == null)
        {
            Log.Error(LogLevel.Normal, "OnComboCommand Error, cmd is null!");
            return;
        }

        if (combo == null)
        {
            Log.Error(LogLevel.Normal, "OnComboCommand Error, combo is null!");
            return;
        }

        CustomOnComboCommand(cmd, combo);
    }
}
