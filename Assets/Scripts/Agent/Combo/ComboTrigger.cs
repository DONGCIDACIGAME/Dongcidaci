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

    public ComboTrigger()
    {
        mSortedTriggerableCombos = new List<TriggerableCombo>();
    }

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

    public void Initialize(Agent agent)
    {
        if (agent == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, agent is null!");
            return;
        }

        mAgent = agent;
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

            if (comboData.comboStepDatas == null || comboData.comboStepDatas.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo [{0}] comboStepDatas is null or emtpty, agent id:{1}, agent Name:{2}", comboData.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            TriggerableCombo tc = null;
            bool added = false;
            for (int j = mSortedTriggerableCombos.Count - 1; j >= 0; j--)
            {
                TriggerableCombo triggerableCombo = mSortedTriggerableCombos[j];
                if (comboData.comboStepDatas.Length < triggerableCombo.GetComboStepCount())
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
    public int OnNewInput(byte newInput, int meterIndex, out TriggerableCombo combo)
    {
        combo = null;

        // be_hit不是combo触发器，而且be_hit不会等待combo结束，在be_hit时立即处理
        if (newInput == AgentCommandDefine.BE_HIT
            || newInput == AgentCommandDefine.IDLE
            || newInput == AgentCommandDefine.EMPTY
            || newInput == AgentCommandDefine.RUN)
        {
            return ComboDefine.ComboTriggerResult_NotComboTrigger;
        }

        // combo结束前的输入都会被丢弃
        if (meterIndex < comboLogicEndMeter)
            return ComboDefine.ComboTriggerResult_ComboExcuting;

        // 所有的combo都过一遍新的输入
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && combo == null)
            {
                combo = tc;
            }
        }

        if(combo != null)
        {
            ComboStepData csd = combo.GetCurrentComboStep();
            comboLogicEndMeter = meterIndex + AgentHelper.GetAgentStateMeterLen(mAgent, csd.statusName, csd.stateName);

            // 如果是结束招式，就做个reset标记，在节拍到了的时候重新开始combo检测
            if(csd.endFlag)
            {
                resetFlag = true;
            }
            return ComboDefine.ComboTriggerResult_Succeed;
        }
        else
        {
            return ComboDefine.ComboTriggerResult_Failed;
        }
    }

    /// <summary>
    /// 一个完整的combo打完时，要重置一下combohandler，重新开始下一次的combo处理
    /// 但是记录的上次combo的触发拍和结束拍不能清除，否则会导致重复触发
    /// </summary>
    public void ResetAllCombo()
    {
        for(int i = 0;i<mSortedTriggerableCombos.Count;i++)
        {
            mSortedTriggerableCombos[i].Reset();
        }
    }

    public void Dispose()
    {
        ResetAllCombo();
        comboLogicEndMeter = -1;
        mSortedTriggerableCombos = null;
    }

    public void OnMeter(int meterIndex)
    {
        if(meterIndex == comboLogicEndMeter && resetFlag)
        {
            ResetAllCombo();
        }
    }
}
