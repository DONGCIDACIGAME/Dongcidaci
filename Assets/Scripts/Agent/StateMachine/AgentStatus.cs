using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentStatus : IAgentStatus
{
    /// <summary>
    /// 切换状态的代理方法
    /// 后面考虑改为事件
    /// </summary>
    protected ChangeStatusDelegate ChangeStatus;


    protected Agent mAgent;

    /// <summary>
    /// 输入处理器
    /// </summary>
    protected IInputHandle mInputHandle;

    /// <summary>
    /// 当前逻辑状态结束节拍index
    /// </summary>
    protected int mCurLogicStateEndMeter;

    /// <summary>
    /// 等待执行的指令集缓存区
    /// </summary>
    protected AgentInputCommandBuffer cmdBuffer;

    /// <summary>
    /// 当前触发的combo
    /// </summary>
    protected TriggeredComboAction mCurTriggeredComboAction;



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

        float duration = MeterManager.Ins.GetTimeToMeter(state.stateMeterLen);
        if(duration > 0)
        {
            mCurLogicStateEndMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, state.stateMeterLen);
            float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, mCurLogicStateEndMeter);
            mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, state.layer, state.normalizedTime, duration, state.animLen, totalMeterTime);
        }
    }

    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}--cur meter:{1}", GetStatusName(), MeterManager.Ins.MeterIndex);
        mInputHandle.SetEnable(true);
        mCurLogicStateEndMeter = -1;
    }


    public virtual void OnExit() 
    {
        mInputHandle.SetEnable(false);
        cmdBuffer.ClearCommandBuffer();
        mCurTriggeredComboAction = null;
        mCurLogicStateEndMeter = -1;
    }

    protected virtual void CustomDispose() { }

    public void Dispose()
    {
        ChangeStatus = null;
        mAgent = null;
        mCurLogicStateEndMeter = -1;

        if(mInputHandle != null)
        {
            mInputHandle = null;
        }


        if(cmdBuffer != null)
        {
            cmdBuffer.Dispose();
            cmdBuffer = null;
        }

        CustomDispose();
    }

    protected abstract void CommandHandleOnMeter(int meterIndex);

    public virtual void OnMeter(int meterIndex)
    {
        CommandHandleOnMeter(meterIndex);
        cmdBuffer.ClearCommandBuffer();
        if(meterIndex == mCurLogicStateEndMeter)
        {
            mCurLogicStateEndMeter = -1;
        }
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
    public void ProgressWaitOnCommand(float waitMeterProgress, AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
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
            if(triggeredComboAction != null)
            {
                ChangeStatusOnComboCommand(cmd, triggeredComboAction);
            }
            else
            {
                ChangeStatusOnNormalCommand(cmd);
            }
        }
        else
        {
            PushInputCommandToBuffer(cmd.CmdType, cmd.Towards, triggeredComboAction);
        }
    }

    /// <summary>
    /// 输入指令暂存到buffer里，等待后续处理
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    public void PushInputCommandToBuffer(byte cmdType, Vector3 towards, TriggeredComboAction triggerdComboAction)
    {
        cmdBuffer.AddInputCommand(cmdType, towards);
        if(triggerdComboAction != null)
        {
            SetCurTriggeredCombo(triggerdComboAction);
        }
    }

    public void SetCurTriggeredCombo(TriggeredComboAction triggerdComboAction)
    {
        mCurTriggeredComboAction = triggerdComboAction;
    }

    protected void ChangeStatusOnNormalCommand(AgentInputCommand cmd)
    {
        if (cmd == null)
            return;

        ChangeStatusOnNormalCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter);
    }

    protected void ChangeToIdle()
    {
        ChangeStatus(AgentStatusDefine.IDLE, null);
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

    protected void ChangeStatusOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction)
    {
        if (cmd == null)
            return;

        if (triggeredComboAction == null)
            return;

        ChangeStatusOnComboCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboAction);
    }

    protected void ChangeStatusOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        if (cmdType == AgentCommandDefine.IDLE || cmdType == AgentCommandDefine.RUN
            || cmdType == AgentCommandDefine.BE_HIT)
        {
            ChangeStatusOnNormalCommand(cmdType, towards, triggerMeter);
            Log.Error(LogLevel.Normal, "ChangeStatusOnComboCommand Exception, 指令类型[{0}]应该不是combo的触发指令, 错误触发的combo:{0}", cmdType, triggeredComboAction.comboName);
            return;
        }

        if (cmdType == AgentCommandDefine.DASH)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            args.Add("comboAction", triggeredComboAction);
            ChangeStatus(AgentStatusDefine.DASH, args);
        }
        else if (cmdType == AgentCommandDefine.ATTACK_LONG || cmdType == AgentCommandDefine.ATTACK_SHORT)
        {
            Dictionary<string, object> args = new Dictionary<string, object>();
            args.Add("triggerCmd", cmdType);
            args.Add("towards", towards);
            args.Add("triggerMeter", triggerMeter);
            args.Add("combo", triggeredComboAction);
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

    protected virtual void CustomOnComboCommand(AgentInputCommand cmd, TriggeredComboAction triggeredComboAction) { }
   
    public void OnComboCommand(AgentInputCommand cmd, TriggeredComboAction combo)
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
