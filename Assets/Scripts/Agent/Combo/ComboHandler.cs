using System.Collections.Generic;

public class ComboHandler
{
    private List<TriggerableCombo> mSortedTriggerableCombos;

    /// <summary>
    /// 上次触发combo的节拍index
    /// </summary>
    private int lastTriggeredMeter;

    public ComboHandler()
    {

    }


    public void Initialize(ComboDataGraph comboGraph)
    {
        if (comboGraph == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, comboGraph is null!");
            return;
        }

        if(comboGraph.comboDatas == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combos is null, agentId:{0},  agentName:{1}",comboGraph.agentId, comboGraph.agentName);
            return;
        }

        mSortedTriggerableCombos = new List<TriggerableCombo>();

        for(int i = 0; i < comboGraph.comboDatas.Length;i++)
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
            for (int j = mSortedTriggerableCombos.Count-1; j >=0 ; j--)
            {
                TriggerableCombo triggerableCombo = mSortedTriggerableCombos[j];
                if(triggerableCombo.GetComboStepCount() <= comboData.comboStepDatas.Length)
                {
                    tc = new TriggerableCombo(comboData);
                    mSortedTriggerableCombos.Insert(j, tc);
                    return ;
                }
            }

            tc = new TriggerableCombo(comboData);
            mSortedTriggerableCombos.Add(tc);
        }
    }

    /// <summary>
    /// 出招匹配检测
    /// </summary>
    /// <param name="newInput">输入</param>
    /// <param name="meterIndex">输入对应的节拍index</param>
    /// <returns>返回第一个被触发的combo</returns>
    public TriggerableCombo OnNewInput(byte newInput, int meterIndex)
    {
        // 同一拍只能触发一次combo
        if (meterIndex == lastTriggeredMeter)
            return null;

        TriggerableCombo firstTriggeredCombo = null;

        // 所有的combo都过一遍新的输入
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && firstTriggeredCombo == null)
            {
               firstTriggeredCombo = tc;
            }
        }

        if(firstTriggeredCombo != null)
        {
            lastTriggeredMeter = meterIndex;
        }

        // 返回第一个被触发的combo
        return firstTriggeredCombo;
    }

    /// <summary>
    /// 一个完整的combo打完时，要重置一下combohandler，重新开始下一次的combo处理
    /// </summary>
    public void Reset()
    {
        for(int i = 0;i<mSortedTriggerableCombos.Count;i++)
        {
            mSortedTriggerableCombos[i].Reset();
        }
    }

    public void Dispose()
    {
        mSortedTriggerableCombos = null;
        lastTriggeredMeter = -1;
    }
}
