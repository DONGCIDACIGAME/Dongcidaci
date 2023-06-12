using GameEngine;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentStatus : IAgentStatus
{

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
    public void Initialize(Agent agt)
    {
        mAgent = agt;
        statusDefaultActionData = AgentHelper.GetAgentDefaultStatusActionData(agt, GetStatusName());
        cmdBuffer = new AgentInputCommandBuffer();
        mMeterEndActions = new Stack<MeterEndAction>();
        mCustomAnimDriver = new CustomAnimDriver(mAgent);
        mStepLoopAnimDriver = new StepLoopAnimDriver(mAgent, GetStatusName());
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
        mCurLogicStateEndMeter = -1;
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
        mStepLoopAnimDriver.Reset();
        mCustomAnimDriver.Reset();
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
            args.Add("comboStep", triggeredComboStep);
        }

        GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), status, args);
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

    /// <summary>
    /// 执行Combo
    /// </summary>
    /// <param name="triggeredComboAction"></param>
    protected void ExcuteCombo(byte cmdType, Vector3 towards, int triggerMeter, ref TriggeredComboStep triggeredComboStep)
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

        Log.Error(LogLevel.Info, "ExcuteCombo---comboName:{0}", triggeredComboStep.comboData.comboName);

        // 1. 转向
        mAgent.MoveControl.TurnTo(towards);

        // 如果是叠加模式，执行默认逻辑
        if (comboStep.mode == ComboDefine.ComboMode_Overlay)
        {
            StatusDefaultAction(cmdType, towards, triggerMeter, triggeredComboStep.comboStep.agentActionData);
        }

        // 如果是覆盖模式，播放动画
        if (comboStep.mode == ComboDefine.ComboMode_Overwrite)
        {
            mCurLogicStateEndMeter = mCustomAnimDriver.PlayAnimStateWithCut(comboStep.agentActionData.statusName, comboStep.agentActionData.stateName);
        }


        AgentActionData actionData = triggeredComboStep.comboStep.agentActionData;
        if (actionData == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteCombo Error, actionData is null, combo name:{0}, index:{1}!", triggeredComboStep.comboData.comboName, triggeredComboStep.stepIndex);
            return;
        }

        // 3. 效果执行器开始执行
        mAgent.EffectExcutorCtl.Start(actionData.statusName, actionData.stateName, actionData.effectCollictions);

        // 4. 如果是结束招式
        if (comboStep.endFlag)
        {
            float transferStateDuration = triggeredComboStep.comboData.transferStateDuration;

            // 在节拍结束时切换到过度状态
            mMeterEndActions.Push(new MeterEndAction(mCurLogicStateEndMeter, () =>
            {
                
                Dictionary<string, object> args = new Dictionary<string, object>();
                args.Add("duration", transferStateDuration);
                GameEventSystem.Ins.Fire("ChangeAgentStatus", mAgent.GetAgentId(), AgentStatusDefine.TRANSFER, args);
            }));
        }

        // 回收combostep
        triggeredComboStep.Recycle();

        // 清理
        triggeredComboStep = null;
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
        if (triggerMeter <= mCurLogicStateEndMeter)
            return false;

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
            if(triggeredComboStep != null)
            {
                ExcuteCombo(cmdType, towards, triggerMeter, ref triggeredComboStep);
            }
            else
            {
                StatusDefaultAction(cmdType, towards, triggerMeter, GetAgentActionData());
            }
            return true;
        }
        else
        {
            PushInputCommandToBuffer(cmdType, towards, triggerMeter, triggeredComboStep);
            return false;
        }
    }

    /// <summary>
    /// 获取角色的行为数据
    /// 如果当前触发了combo，就使用combo的行为数据
    /// 否则，使用状态默认的行为数据
    /// </summary>
    /// <returns></returns>
    private AgentActionData GetAgentActionData()
    {
        if(mCurTriggeredComboStep != null)
        {
            return mCurTriggeredComboStep.comboStep.agentActionData;
        }

        return AgentHelper.GetAgentDefaultStatusActionData(mAgent, GetStatusName());
    }
}
