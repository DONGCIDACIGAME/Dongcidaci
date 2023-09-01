using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentStatus : IAgentStatus
{

    protected Agent mAgent;

    /// <summary>
    /// 当前执行中的指令
    /// </summary>
    protected int mCurExcutingCmdType;

    /// <summary>
    /// 根据节拍融合的动画驱动器
    /// </summary>
    protected MatchMeterCrossfadeAnimDriver mMatchMeterCrossfadeAnimDriver;

    /// <summary>
    /// 步进式动画驱动器
    /// </summary>
    protected StepLoopAnimDriver mStepLoopAnimDriver;

    /// <summary>
    /// 正常融合的动画驱动器
    /// </summary>
    protected DefaultCrossfadeAnimDriver mDefaultCrossFadeAnimDriver;


    /// <summary>
    /// 当前逻辑状态结束节拍index
    /// </summary>
    protected int mCurLogicStateEndMeter;

    /// <summary>
    /// 等待执行的指令集缓存区
    /// </summary>
    protected AgentCommandBuffer cmdBuffer;

    /// <summary>
    /// 节拍结束时的待执行行为
    /// </summary>
    protected Stack<MeterEndAction> mMeterEndActions;

    /// <summary>
    /// 状态的默认行为数据
    /// </summary>
    private AgentActionData mStatusDefaultActionData;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agt"></param>
    /// <param name="cb"></param>
    public void Initialize(Agent agt)
    {
        mAgent = agt;
        mStatusDefaultActionData = AgentHelper.GetAgentDefaultStatusActionData(agt, GetStatusName());
        cmdBuffer = new AgentCommandBuffer();
        mMeterEndActions = new Stack<MeterEndAction>();
        mMatchMeterCrossfadeAnimDriver = new MatchMeterCrossfadeAnimDriver(mAgent);
        mStepLoopAnimDriver = new StepLoopAnimDriver(mAgent);
        mDefaultCrossFadeAnimDriver = new DefaultCrossfadeAnimDriver(mAgent);
    }

    /// <summary>
    /// 状态的自定义初始化方法
    /// </summary>g
    public virtual void CustomInitialize(){ }

    /// <summary>
    /// 状态名称
    /// </summary>
    /// <returns></returns>
    public abstract string GetStatusName();

   
    /// <summary>
    /// 获取指令切换的状态类型
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public abstract string GetChangeToStatus(int cmdType);


    /// <summary>
    /// 进入状态
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnEnter(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}--cur meter:{1}", GetStatusName(), MeterManager.Ins.MeterIndex);
        cmdBuffer.ClearCommandBuffer();
        mMeterEndActions.Clear();
        if(AgentStatusDefine.IsResetComboStatus(GetStatusName()))
        {
            mAgent.Combo_Trigger.ResetAllCombo();
        }
        mCurExcutingCmdType = cmdType;
        mCurLogicStateEndMeter = -1;
    }
    
    /// <summary>
    /// 结束状态
    /// </summary>
    public virtual void OnExit() 
    {
        cmdBuffer.ClearCommandBuffer();
        mMeterEndActions.Clear();
        mStepLoopAnimDriver.Reset();
        mMatchMeterCrossfadeAnimDriver.Reset();
        mCurExcutingCmdType = AgentCommandDefine.EMPTY;
    }

    /// <summary>
    /// 状态的自定义
    /// </summary>
    protected virtual void CustomDispose() { }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        mAgent = null;
        mCurLogicStateEndMeter = 0;

        if (mMatchMeterCrossfadeAnimDriver != null)
        {
            mMatchMeterCrossfadeAnimDriver.Dispose();
            mMatchMeterCrossfadeAnimDriver = null;
        }

        if (mStepLoopAnimDriver != null)
        {
            mStepLoopAnimDriver.Dispose();
            mStepLoopAnimDriver = null;
        }

        if (cmdBuffer != null)
        {
            cmdBuffer.Dispose();
            cmdBuffer = null;
        }

        CustomDispose();
    }

    protected abstract void CustomOnMeterEnter(int meterIndex);

    protected abstract void CustomOnMeterEnd(int meterIndex);


    public virtual void OnMeterEnter(int meterIndex)
    {
        CustomOnMeterEnter(meterIndex);
        mStepLoopAnimDriver.OnMeterEnter(meterIndex);
        mMatchMeterCrossfadeAnimDriver.OnMeterEnter(meterIndex);
        //cmdBuffer.ClearCommandBuffer();
    }

    public void OnMeterEnd(int meterIndex)
    {
        CustomOnMeterEnd(meterIndex);
        mStepLoopAnimDriver.OnMeterEnd(meterIndex);
        mMatchMeterCrossfadeAnimDriver.OnMeterEnd(meterIndex);
        while (mMeterEndActions.TryPop(out MeterEndAction action))
        {
            action.CheckAndExcute(meterIndex);
        }
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        mStepLoopAnimDriver.OnDisplayPointBeforeMeterEnter(meterIndex);
        mMatchMeterCrossfadeAnimDriver.OnDisplayPointBeforeMeterEnter(meterIndex);
    }

    public virtual void OnGameUpdate(float deltaTime)
    {
        mDefaultCrossFadeAnimDriver.OnGameUpdate(deltaTime);
    }

    /// <summary>
    /// 执行命令切换状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboStep"></param>
    protected void ChangeStatusOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string,object> args, TriggeredComboStep triggeredComboStep)
    {
        string status = GetChangeToStatus(cmdType);
        if (string.IsNullOrEmpty(status))
        {
            Log.Error(LogLevel.Normal, "ChangeStatusOnCommand Failed, no matching status to cmdType:{0}", cmdType);
            return;
        }

        // 默认切换状态都带有 指令类型，指令方向，指令所属节拍信息, 触发的combo招式数据
        GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), status, cmdType, towards, triggerMeter, args, triggeredComboStep);
    }

    /// <summary>
    /// 输入指令暂存到buffer里，等待后续处理
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    public void PushInputCommandToBuffer(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep)
    {
        cmdBuffer.AddInputCommand(cmdType, towards, triggerMeter, args, triggeredComboStep);
    }

    protected abstract void CustomOnCommand(int cmdType, Vector3 towards, int triggerMeter, Dictionary<string, object> args, TriggeredComboStep triggeredComboStep);

    public void OnCommand(AgentCommand cmd, TriggeredComboStep triggeredComboStep)
    {
        if (cmd == null)
        {
            Log.Error(LogLevel.Normal, "OnNormalCommand Error, cmd is null!");
            return;
        }

        CustomOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, cmd.Args, triggeredComboStep);

        mCurExcutingCmdType = cmd.CmdType;
    }


    /// <summary>
    /// 执行Combo
    /// </summary>
    /// <param name="triggeredComboStep"></param>
    public void ExcuteCombo(int triggerMeter, float timeLost, TriggeredComboStep triggeredComboStep)
    {
        if (triggeredComboStep == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, combo is null!");
            return;
        }

        AgentActionData actionData = triggeredComboStep.comboStep.attackActionData;
        if (actionData == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, actionData is null, combo name:{0}, index:{1}!", triggeredComboStep.comboData.comboName, triggeredComboStep.stepIndex);
            return;
        }

        // 2. 效果执行器开始执行
        // changed by weng 0708
        // 执行效果时，需要把combo的一些信息同时传入
        mAgent.EffectExcutorCtl.Start(actionData.statusName, actionData.stateName, timeLost, actionData.effectCollictions);
    }

    /// <summary>
    /// 默认行为数据
    /// </summary>
    /// <returns></returns>
    protected AgentActionData GetStatusDefaultActionData()
    {
        return mStatusDefaultActionData;
    }
}
