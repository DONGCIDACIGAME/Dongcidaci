using System.Collections.Generic;

/// <summary>
/// Combo触发器
/// </summary>
public class ComboTrigger : IMeterHandler
{
    private List<TriggerableCombo> mSortedTriggerableCombos;
    private Agent mAgent;

    /// <summary>
    /// combo的逻辑完成拍
    /// </summary>
    private int comboLogicEndMeter;

    /// <summary>
    /// 重置标志
    /// </summary>
    private bool resetFlag;

    public void SetComboActive(string comboName, bool active)
    {
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];
            if (tc.IsCombo(comboName))
            {
                tc.SetActive(active);
                return;
            }
        }
    }

    /// <summary>
    /// 初始化
    /// 将所有的combo按照招式数量升序排列
    /// </summary>
    /// <param name="agent"></param>
    public void Initialize(Agent agent)
    {
        if (agent == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, agent is null!");
            return;
        }

        mAgent = agent;
        mSortedTriggerableCombos = new List<TriggerableCombo>();

        ComboDataGraph comboGraph = DataCenter.Ins.AgentComboGraphCenter.GetAgentComboGraph(agent.GetAgentId());
        if (comboGraph == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, comboGraph is null!");
            return;
        }

        if (comboGraph.comboDatas == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combos is null, agentId:{0},  agentName:{1}", comboGraph.agentId, comboGraph.agentName);
            return;
        }

        mSortedTriggerableCombos.Clear();
        comboLogicEndMeter = 0;

        for (int i = 0; i < comboGraph.comboDatas.Length; i++)
        {
            ComboData comboData = comboGraph.comboDatas[i];
            if (comboData == null)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, ComboGraph has null combo at index {0}, agent id:{1}, agent Name:{2}", i, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            if (comboData.comboSteps == null || comboData.comboSteps.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo [{0}] comboStepDatas is null or emtpty, agent id:{1}, agent Name:{2}", comboData.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            TriggerableCombo tc = null;
            bool added = false;
            for (int j = mSortedTriggerableCombos.Count - 1; j >= 0; j--)
            {
                TriggerableCombo triggerableCombo = mSortedTriggerableCombos[j];
                if (comboData.comboSteps.Length < triggerableCombo.GetComboStepCount())
                {
                    tc = new TriggerableCombo(comboData);
                    mSortedTriggerableCombos.Insert(j, tc);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                tc = new TriggerableCombo(comboData);
                mSortedTriggerableCombos.Add(tc);
            }
        }
    }

    private TriggeredComboStep Trigger(byte newInput, int meterIndex)
    {
        TriggeredComboStep tcs = null;

        // 所有的combo都过一遍新的输入
        for (int i = 0; i<mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && tcs == null)
            {
                // get from pool
                tcs = GamePoolCenter.Ins.TriggeredComboActionPool.Pop();

                ComboStep comboStep = tc.GetCurrentComboStep();

                // combo的逻辑结束拍=combo触发拍+combo招式持续拍-1
                comboLogicEndMeter = meterIndex + AgentHelper.GetAgentStateMeterLen(mAgent, comboStep.agentActionData.statusName, comboStep.agentActionData.stateName) - 1;
                Log.Error(LogLevel.Info, "Combo Trigger OnNewInput -------new triggered---meterIndex:{0}, comboLogicEndMeter:{1}", meterIndex, comboLogicEndMeter);

                // 记录这个被触发的招式，记录时不仅要记录招式数据，还要记录是谁触发的，combo的名字，招式的index, 触发在那一拍
                tcs.Initialize(mAgent.GetAgentId(), tc.GetComboData(), tc.triggeredAt, meterIndex, comboLogicEndMeter, comboStep);
            }
        }

        return tcs;
    }

    /// <summary>
    /// 出招匹配检测
    /// </summary>
    /// <param name="newInput">输入</param>
    /// <param name="meterIndex">输入对应的节拍index</param>
    /// <returns>触发combo的结果</returns>
    public int OnNewInput(byte newInput, int meterIndex, out TriggeredComboStep triggeredComboStep)
    {
        triggeredComboStep = null;
        if (meterIndex > comboLogicEndMeter && resetFlag)
        {
            ResetAllCombo();
        }

        // 不能触发combo的指令
        if (!AgentCommandDefine.IsComboTrigger(newInput))
        {
            return ComboDefine.ComboTriggerResult_NotComboTrigger;
        }

        Log.Error(LogLevel.Info, "Combo Trigger OnNewInput -------newInput:{0}---meterIndex:{1}", newInput, meterIndex);
        // 如果已经在combo招式的执行过程中，且新的输入的触发节拍不是在当前combo招式的结束拍，则该输入就被丢弃
        // |---a-----------b---|
        // 对于输入a，应该属于前面那个竖杠的拍子，对于输入b，应该属于后面竖杠的按个拍子
        // 所以这里的输入丢弃逻辑应该没有问题
        if (meterIndex <= comboLogicEndMeter)
        {
            Log.Error(LogLevel.Info, "Combo Trigger OnNewInput -------Excuting---meterIndex:{0}, comboLogicEndMeter:{1}", meterIndex, comboLogicEndMeter);
            return ComboDefine.ComboTriggerResult_ComboExcuting;
        }

        //// 所有的combo都过一遍新的输入
        //for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        //{
        //    TriggerableCombo tc = mSortedTriggerableCombos[i];

        //    bool triggered = tc.TryTriggerOnNewInput(newInput);

        //    // 成功触发combo时，记录第一个被触发的combo
        //    if (triggered && triggeredComboStep == null)
        //    {
        //        // get from pool
        //        triggeredComboStep = GamePoolCenter.Ins.TriggeredComboActionPool.Pop();

        //        ComboStep comboStep = tc.GetCurrentComboStep();

        //        // combo的逻辑结束拍=combo触发拍+combo招式持续拍-1
        //        comboLogicEndMeter = meterIndex + AgentHelper.GetAgentStateMeterLen(mAgent, comboStep.agentActionData.statusName, comboStep.agentActionData.stateName) - 1;
        //        Log.Error(LogLevel.Info, "Combo Trigger OnNewInput -------new triggered---meterIndex:{0}, comboLogicEndMeter:{1}", meterIndex, comboLogicEndMeter);

        //        // 记录这个被触发的招式，记录时不仅要记录招式数据，还要记录是谁触发的，combo的名字，招式的index, 触发在那一拍
        //        triggeredComboStep.Initialize(mAgent.GetAgentId(), tc.GetComboData(), tc.triggeredAt, meterIndex, comboLogicEndMeter, comboStep);
        //    }
        //}

        // 1. 尝试在原有可触发的combo上（只要前面的输入都对得上就是可触发的）继续触发下一个combo招式
        triggeredComboStep = Trigger(newInput, meterIndex);

        // 如果触发了combo
        if (triggeredComboStep != null)
        {
            // 记录combo结束标签
            resetFlag = triggeredComboStep.comboStep.endFlag;

            // 返回combo触发成功
            return ComboDefine.ComboTriggerResult_Succeed;
        }

        // 如果没能继续触发当前可触发的combo，就重置所有combo，从头开始检测
        ResetAllCombo();

        // 2. 所有combo已经重置过，从头开始测试combo
        triggeredComboStep = Trigger(newInput, meterIndex);

        // 如果触发了combo
        if (triggeredComboStep != null)
        {
            // 记录combo结束标签
            resetFlag = triggeredComboStep.comboStep.endFlag;

            // 返回combo触发成功
            return ComboDefine.ComboTriggerResult_Succeed;
        }


        // 3. 如果还是没能触发combo，就是真的触发失败了
        return ComboDefine.ComboTriggerResult_Failed;
    }

    /// <summary>
    /// 重置所有的combo
    /// 用于重新开始combo检测
    /// </summary>
    public void ResetAllCombo()
    {
        for(int i = 0;i<mSortedTriggerableCombos.Count;i++)
        {
            mSortedTriggerableCombos[i].Reset();
        }
        resetFlag = false;
    }

    public void Dispose()
    {
        ResetAllCombo();
        resetFlag = false;
        mSortedTriggerableCombos = null;
    }

    public void OnMeterEnter(int meterIndex)
    {

    }

    public void OnMeterEnd(int meterIndex)
    {
        // 是combo的结束招式
        // 重置所有combo，等待重新检测
        if (meterIndex >= comboLogicEndMeter && resetFlag)
        {
            ResetAllCombo();
        }
    }
}
