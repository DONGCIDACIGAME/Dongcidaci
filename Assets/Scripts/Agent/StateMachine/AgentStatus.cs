using System;
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
    /// 自定义动画驱动器
    /// </summary>
    protected CustomAnimDriver mCustomAnimDriver;

    /// <summary>
    /// 步进式动画驱动器
    /// </summary>
    protected StepLoopAnimDriver mStepLoopAnimDriver;

    /// <summary>
    /// 当前逻辑状态结束节拍index
    /// </summary>
    protected int mCurLogicStateEndMeter;

    /// <summary>
    /// 等待执行的指令集缓存区
    /// </summary>
    protected AgentInputCommandBuffer cmdBuffer;

    /// <summary>
    /// 当前触发的combo招式缓存
    /// </summary>
    protected TriggeredComboAction mCurTriggeredComboAction;

    protected Stack<MeterEndAction> mMeterEndActions;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agt"></param>
    /// <param name="cb"></param>
    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        cmdBuffer = new AgentInputCommandBuffer();
        mMeterEndActions = new Stack<MeterEndAction>();
        mCustomAnimDriver = new CustomAnimDriver(mAgent, GetStatusName());
        mStepLoopAnimDriver = new StepLoopAnimDriver(mAgent, GetStatusName());
    }

    /// <summary>
    /// 状态的自定义初始化方法
    /// </summary>
    public virtual void CustomInitialize(){ }

    /// <summary>
    /// 状态名称
    /// </summary>
    /// <returns></returns>
    public abstract string GetStatusName();

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}--cur meter:{1}", GetStatusName(), MeterManager.Ins.MeterIndex);
        cmdBuffer.ClearCommandBuffer();
        mMeterEndActions.Clear();
        mCurTriggeredComboAction = null;
        mInputHandle.SetEnable(true);
    }
    
    /// <summary>
    /// 结束状态
    /// </summary>
    public virtual void OnExit() 
    {
        mInputHandle.SetEnable(false);
        cmdBuffer.ClearCommandBuffer();
        mMeterEndActions.Clear();
        mCurTriggeredComboAction = null;
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
        ChangeStatus = null;
        mAgent = null;
        mCurLogicStateEndMeter = 0;

        if(mInputHandle != null)
        {
            mInputHandle = null;
        }

        if (mCustomAnimDriver != null)
        {
            mCustomAnimDriver.Dispose();
            mCustomAnimDriver = null;
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
        cmdBuffer.ClearCommandBuffer();
    }

    public void OnMeterEnd(int meterIndex)
    {
        CustomOnMeterEnd(meterIndex);

        while(mMeterEndActions.TryPop(out MeterEndAction action))
        {
            action.CheckAndExcute(meterIndex);
        }
    }

    public virtual void OnUpdate(float deltaTime)
    {
        
    }

    /// <summary>
    /// 执行命令切换状态
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboAction"></param>
    protected void ChangeStatusOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        string status = AgentCommandDefine.GetChangeToStatus(cmdType);
        if (string.IsNullOrEmpty(status))
        {
            Log.Error(LogLevel.Normal, "ChangeStatusOnCommand Failed, no matching status to cmdType:{0}", cmdType);
            return;
        }

        // 默认切换状态都带有 指令类型，指令方向，指令所属节拍信息
        Dictionary<string, object> args = new Dictionary<string, object>();
        args.Add("triggerCmd", cmdType);
        args.Add("towards", towards);
        args.Add("triggerMeter", triggerMeter);

        // 如果是combo的触发类型，并且触发了combo，就添加combo招式信息
        if (AgentCommandDefine.IsComboTrigger(cmdType) && triggeredComboAction != null)
        {
            args.Add("comboAction", triggeredComboAction);
        }

        ChangeStatus(status, args);
    }

    /// <summary>
    /// 输入指令暂存到buffer里，等待后续处理
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    public void PushInputCommandToBuffer(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggerdComboAction)
    {
        cmdBuffer.AddInputCommand(cmdType, towards, triggerMeter);
        if(triggerdComboAction != null)
        {
            mCurTriggeredComboAction = triggerdComboAction;
        }
    }

    protected void ChangeToIdle()
    {
        ChangeStatus(AgentStatusDefine.IDLE, null);
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

        if (!AgentCommandDefine.IsComboTrigger(cmd.CmdType))
        {
            Log.Error(LogLevel.Normal, "OnComboCommand Error,[{0}] is  not combo trigger command type!", cmd.CmdType);
            return;
        }

        CustomOnComboCommand(cmd, combo);
    }



    /// <summary>
    /// 执行Combo
    /// </summary>
    /// <param name="triggeredComboAction"></param>
    protected void ExcuteCombo(TriggeredComboAction triggeredComboAction)
    {
        if (triggeredComboAction == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, combo is null!");
            return;
        }

        ComboActionData actionData = triggeredComboAction.actionData;
        if (actionData == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, ComboActionData is null, combo name:{0}, index:{1}!", triggeredComboAction.comboData.comboName, triggeredComboAction.actionIndex);
            return;
        }

        mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(actionData.stateName);
        mAgent.ComboEffectsExcutor.Start(triggeredComboAction);
        if (actionData.endFlag)
        {
            float transferStateDuration = triggeredComboAction.comboData.transferStateDuration;
            mMeterEndActions.Push(new MeterEndAction(mCurLogicStateEndMeter - 1, () =>
            {
                
                Dictionary<string, object> args = new Dictionary<string, object>();
                args.Add("duration", transferStateDuration);
                ChangeStatus(AgentStatusDefine.TRANSFER, args);
            }));
        }

        mCurTriggeredComboAction = null;
        triggeredComboAction.Recycle();
    }


    /// <summary>
    /// 根据节拍进度执行combo
    /// 如果本拍的剩余时间占比=waitMeterProgress,就直接执指令，否则等下拍执行指令
    /// 其他情况等待下一拍执行
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboAction"></param>
    protected bool ConditionalExcuteCombo(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction)
    {
        // 当前拍的剩余时间
        float timeToNextMeter = MeterManager.Ins.GetTimeToMeter(1);
        // 当前拍的总时间
        float timeOfCurrentMeter = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, MeterManager.Ins.MeterIndex + 1);

        if (timeOfCurrentMeter <= 0)
        {
            Log.Error(LogLevel.Normal, "ProgressWaitOnCommand Error, 当前拍的总时间<=0, 当前拍:{0}", MeterManager.Ins.MeterIndex);
            return false ;
        }

        float progress = timeToNextMeter / timeOfCurrentMeter;
        if (progress >= GamePlayDefine.AttackMeterProgressWait)
        {
            mAgent.MoveControl.TurnTo(towards);
            ExcuteCombo(triggeredComboAction);
            return true;
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboAction);
            return false;
        }
    }
}
