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
    protected TriggeredComboStep mCurTriggeredComboStep;

    /// <summary>
    /// 节拍结束时的待执行行为
    /// </summary>
    protected Stack<MeterEndAction> mMeterEndActions;

    /// <summary>
    /// 状态的默认行为数据
    /// </summary>
    protected AgentActionData statusDefaultActionData;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="agt"></param>
    /// <param name="cb"></param>
    public void Initialize(Agent agt, ChangeStatusDelegate cb)
    {
        ChangeStatus = cb;
        mAgent = agt;
        statusDefaultActionData = AgentHelper.GetAgentDefaultStatusActionData(agt, GetStatusName());
        cmdBuffer = new AgentInputCommandBuffer();
        mMeterEndActions = new Stack<MeterEndAction>();
        mCustomAnimDriver = new CustomAnimDriver(mAgent);
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
    /// 状态默认的行为逻辑
    /// </summary>
    public abstract void StatusDefaultAction(byte cmdType, Vector3 towards, int triggerMeter, AgentActionData agentActionData);

    /// <summary>
    /// 进入状态
    /// </summary>
    /// <param name="context"></param>
    public virtual void OnEnter(Dictionary<string, object> context) 
    {
        Log.Logic(LogLevel.Info, "OnEnter Status:{0}--cur meter:{1}", GetStatusName(), MeterManager.Ins.MeterIndex);
        cmdBuffer.ClearCommandBuffer();
        mMeterEndActions.Clear();
        if(AgentStatusDefine.IsResetComboStatus(GetStatusName()))
        {
            mAgent.Combo_Trigger.ResetAllCombo();
        }
        mCurTriggeredComboStep = null;
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
        mCurTriggeredComboStep = null;
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

        if(mCurTriggeredComboStep != null && meterIndex >= mCurTriggeredComboStep.endMeter)
        {

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
    /// <param name="triggeredComboStep"></param>
    protected void ChangeStatusOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        string status = AgentCommandDefine.GetChangeToStatus(cmdType);
        if (string.IsNullOrEmpty(status))
        {
            Log.Error(LogLevel.Normal, "ChangeStatusOnCommand Failed, no matching status to cmdType:{0}", cmdType);
            return;
        }

        // 默认切换状态都带有 指令类型，指令方向，指令所属节拍信息
        Dictionary<string, object> args = new Dictionary<string, object>();
        args.Add("cmdType", cmdType);
        args.Add("towards", towards);
        args.Add("triggerMeter", triggerMeter);

        // 如果是combo的触发类型，并且触发了combo，就添加combo招式信息
        if (AgentCommandDefine.IsComboTrigger(cmdType) && triggeredComboStep != null)
        {
            args.Add("comboAction", triggeredComboStep);
        }

        ChangeStatus(status, args);
    }

    /// <summary>
    /// 输入指令暂存到buffer里，等待后续处理
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    public void PushInputCommandToBuffer(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        cmdBuffer.AddInputCommand(cmdType, towards, triggerMeter);
        if(triggeredComboStep != null)
        {
            mCurTriggeredComboStep = triggeredComboStep;
        }
    }

    protected void ChangeToIdle()
    {
        ChangeStatus(AgentStatusDefine.IDLE, null);
    }

    protected virtual void CustomOnCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep) { }

    public void OnCommand(AgentInputCommand cmd, TriggeredComboStep triggeredComboStep)
    {
        if (cmd == null)
        {
            Log.Error(LogLevel.Normal, "OnNormalCommand Error, cmd is null!");
            return;
        }

        CustomOnCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, triggeredComboStep);
    }

    //protected virtual void CustomOnComboCommand(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboAction triggeredComboAction) { }
   
    //public void OnComboCommand(AgentInputCommand cmd, TriggeredComboAction combo)
    //{
    //    if (cmd == null)
    //    {
    //        Log.Error(LogLevel.Normal, "OnComboCommand Error, cmd is null!");
    //        return;
    //    }

    //    if (combo == null)
    //    {
    //        Log.Error(LogLevel.Normal, "OnComboCommand Error, combo is null!");
    //        return;
    //    }

    //    if (!AgentCommandDefine.IsComboTrigger(cmd.CmdType))
    //    {
    //        Log.Error(LogLevel.Normal, "OnComboCommand Error,[{0}] is  not combo trigger command type!", cmd.CmdType);
    //        return;
    //    }

    //    CustomOnComboCommand(cmd.CmdType, cmd.Towards, cmd.TriggerMeter, combo);
    //}



    /// <summary>
    /// 执行Combo
    /// </summary>
    /// <param name="triggeredComboAction"></param>
    protected void ExcuteCombo(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
    {
        if (triggeredComboStep == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, combo is null!");
            return;
        }

        ComboStep comboStep = triggeredComboStep.comboStep;
        if (comboStep == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, comboStep is null, combo name:{0}, index:{1}!", triggeredComboStep.comboData.comboName, triggeredComboStep.stepIndex);
            return;
        }

        // 如果是覆盖模式
        if(comboStep.mode == ComboDefine.ComboMode_Overwrite)
        {
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
        }
        // 如果是叠加模式
        else if(comboStep.mode == ComboDefine.ComboMode_Overlay)
        {
            StatusDefaultAction(cmdType, towards, triggerMeter, triggeredComboStep.comboStep.agentActionData);
        }

        mAgent.Effects_Excutor.Start(triggeredComboStep);
        if (comboStep.endFlag)
        {
            float transferStateDuration = triggeredComboStep.comboData.transferStateDuration;
            mMeterEndActions.Push(new MeterEndAction(mCurLogicStateEndMeter, () =>
            {
                
                Dictionary<string, object> args = new Dictionary<string, object>();
                args.Add("duration", transferStateDuration);
                ChangeStatus(AgentStatusDefine.TRANSFER, args);
            }));
        }

        mCurTriggeredComboStep = null;
        triggeredComboStep.Recycle();
    }


    /// <summary>
    /// 根据节拍进度执行
    /// 如果本拍的剩余时间占比=waitMeterProgress,就直接执指令，否则等下拍执行指令
    /// 其他情况等待下一拍执行
    /// </summary>
    /// <param name="cmdType"></param>
    /// <param name="towards"></param>
    /// <param name="triggerMeter"></param>
    /// <param name="triggeredComboStep"></param>
    protected bool ConditionalExcute(byte cmdType, Vector3 towards, int triggerMeter, TriggeredComboStep triggeredComboStep)
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
            if(triggeredComboStep != null)
            {
                ExcuteCombo(cmdType, towards, triggerMeter, triggeredComboStep);
            }
            else
            {
                StatusDefaultAction(cmdType, towards, triggerMeter, triggeredComboStep.comboStep.agentActionData);
            }
            return true;
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
            return false;
        }
    }
}
