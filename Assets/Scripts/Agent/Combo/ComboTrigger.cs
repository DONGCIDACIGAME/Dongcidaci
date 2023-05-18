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
        comboLogicEndMeter = -1;

        for (int i = 0; i < comboGraph.comboDatas.Length; i++)
        {
            ComboData comboData = comboGraph.comboDatas[i];
            if (comboData == null)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, ComboGraph has null combo at index {0}, agent id:{1}, agent Name:{2}", i, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            if (comboData.comboActionDatas == null || comboData.comboActionDatas.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo [{0}] comboStepDatas is null or emtpty, agent id:{1}, agent Name:{2}", comboData.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            TriggerableCombo tc = null;
            bool added = false;
            for (int j = mSortedTriggerableCombos.Count - 1; j >= 0; j--)
            {
                TriggerableCombo triggerableCombo = mSortedTriggerableCombos[j];
                if (comboData.comboActionDatas.Length < triggerableCombo.GetComboStepCount())
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

    /// <summary>
    /// 出招匹配检测
    /// </summary>
    /// <param name="newInput">输入</param>
    /// <param name="meterIndex">输入对应的节拍index</param>
    /// <returns>触发combo的结果</returns>
    public int OnNewInput(byte newInput, int meterIndex, out TriggeredComboAction action)
    {
        action = null;

        // 不能触发combo的指令
        if (!AgentCommandDefine.IsComboTrigger(newInput))
        {
            return ComboDefine.ComboTriggerResult_NotComboTrigger;
        }

        // 如果已经在combo招式的执行过程中，且新的输入的触发节拍不是在当前combo招式的结束拍，则该输入就被丢弃
        // |---a-----------b---|
        // 对于输入a，应该属于前面那个竖杠的拍子，对于输入b，应该属于后面竖杠的按个拍子
        // 所以这里的输入丢弃逻辑应该没有问题
        if (comboLogicEndMeter >= 0 && meterIndex != comboLogicEndMeter)
            return ComboDefine.ComboTriggerResult_ComboExcuting;

        // 重新开始触发，或者在触发在上一个combo的结束拍
        comboLogicEndMeter = -1;

        // 所有的combo都过一遍新的输入
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && action == null)
            {
                // get from pool
                action = GamePoolCenter.Ins.TriggeredComboActionPool.Pop();

                // 记录这个被触发的招式，记录时不仅要记录招式数据，还要记录是谁触发的，combo的名字，招式的index, 触发在那一拍
                action.Initialize(mAgent.GetAgentId(), tc.GetComboName(), tc.triggeredAt, meterIndex, tc.GetCurrentComboAction());
            }
        }

        if(action != null)
        {
            comboLogicEndMeter = meterIndex + AgentHelper.GetAgentStateMeterLen(mAgent, action.actionData.statusName, action.actionData.stateName);
            
            // 如果当前触发的combo招式是combo的最后一招，就重置所有的combo再次开始检测
            if(action.actionData.endFlag)
            {
                ResetAllCombo();
            }
            return ComboDefine.ComboTriggerResult_Succeed;
        }
        else
        {
            Log.Error(LogLevel.Info, "Trigger Combo failed!");
            for(int i =0;i<mSortedTriggerableCombos.Count;i++)
            {
                Log.Error(LogLevel.Info, mSortedTriggerableCombos[i].ToString());
            }
            return ComboDefine.ComboTriggerResult_Failed;
        }
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
        comboLogicEndMeter = -1;
    }

    public void Dispose()
    {
        ResetAllCombo();
        comboLogicEndMeter = -1;
        mSortedTriggerableCombos = null;
    }

    public void OnMeter(int meterIndex)
    {
       if(meterIndex == comboLogicEndMeter)
        {
            comboLogicEndMeter = -1;
        }
    }
}
