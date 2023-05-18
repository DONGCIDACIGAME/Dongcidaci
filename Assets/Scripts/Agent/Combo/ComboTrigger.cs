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
    public int OnNewInput(byte newInput, int meterIndex, out ExcutiveComboAction action)
    {
        action = null;

        // be_hit不是combo触发器，而且be_hit不会等待combo结束，在be_hit时立即处理
        if (newInput == AgentCommandDefine.BE_HIT
            || newInput == AgentCommandDefine.IDLE
            || newInput == AgentCommandDefine.EMPTY
            || newInput == AgentCommandDefine.RUN)
        {
            return ComboDefine.ComboTriggerResult_NotComboTrigger;
        }

        // 如果输入的触发节拍index不是上一个combo招式的结束拍，则该输入就被丢弃
        // |---a-----------b---|
        // 对于输入a，应该属于前面那个竖杠的拍子，对于输入b，应该属于后面竖杠的按个拍子
        // 所以这里的输入丢弃逻辑应该没有问题
        if (comboLogicEndMeter >= 0 && meterIndex != comboLogicEndMeter)
            return ComboDefine.ComboTriggerResult_ComboExcuting;

        // 所有的combo都过一遍新的输入
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && action == null)
            {
                action = GamePoolCenter.Ins.ExcutiveComboActionPool.Pop();
                action.Initialize(mAgent.GetAgentId(), tc.GetComboName(), tc.triggeredAt, tc.GetCurrentComboAction());
            }
        }

        if(action != null)
        {
            comboLogicEndMeter = meterIndex + AgentHelper.GetAgentStateMeterLen(mAgent, action.actionData.statusName, action.actionData.stateName);
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
    /// 一个完整的combo打完时，要重置一下combohandler，重新开始下一次的combo处理
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
       
    }
}
