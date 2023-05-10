using System.Collections.Generic;

public class ComboDetector : IMeterHandler
{
    private List<TriggerableCombo> mSortedTriggerableCombos;

    /// <summary>
    /// combo的逻辑完成拍
    /// </summary>
    private int comboLogicEndMeter;

    /// <summary>
    /// 当前触发的combo
    /// </summary>
    private TriggerableCombo mCurTriggeredCombo;

    public ComboDetector()
    {
        mSortedTriggerableCombos = new List<TriggerableCombo>();
    }

    public void SetComboActive(string comboName, bool active)
    {
        for(int i = 0;i<mSortedTriggerableCombos.Count;i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];
            if(tc.IsCombo(comboName))
            {
                tc.SetActive(active);
                return;
            }
        }
    }

    public TriggerableCombo GetCurTriggeredCombo()
    {
        return mCurTriggeredCombo;
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

        mSortedTriggerableCombos.Clear();
        comboLogicEndMeter = -1;

        for (int i = 0; i < comboGraph.comboDatas.Length;i++)
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
            for (int j = mSortedTriggerableCombos.Count-1; j >=0 ; j--)
            {
                TriggerableCombo triggerableCombo = mSortedTriggerableCombos[j];
                if(comboData.comboStepDatas.Length < triggerableCombo.GetComboStepCount())
                {
                    tc = new TriggerableCombo(comboData);
                    mSortedTriggerableCombos.Insert(j, tc);
                    added = true;
                    break ;
                }
            }

            if(!added)
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
    public int OnNewInput(byte newInput, int meterIndex)
    {
        // idle，run，be_hit都不是能够触发combo的命令类型
        if (newInput == AgentCommandDefine.IDLE
            || newInput == AgentCommandDefine.EMPTY 
            ||newInput == AgentCommandDefine.RUN 
            || newInput == AgentCommandDefine.BE_HIT)
        {
            return ComboDefine.ComboTriggerResult_NotComboTrigger;
        }

        // combo结束前的输入都会被丢弃
        if (meterIndex < comboLogicEndMeter)
            return ComboDefine.ComboTriggerResult_ComboExcuting;

        // 如果前一个combo的招式是结束招式，就重置combo嗅探器，重新开始检测combo
        if(mCurTriggeredCombo != null && mCurTriggeredCombo.GetCurrentComboStep().endFlag)
        {
            ResetAllCombo();
        }

        // 所有的combo都过一遍新的输入
        for (int i = 0; i < mSortedTriggerableCombos.Count; i++)
        {
            TriggerableCombo tc = mSortedTriggerableCombos[i];

            bool triggered = tc.TryTriggerOnNewInput(newInput);

            // 成功触发combo时，记录第一个被触发的combo
            if (triggered && mCurTriggeredCombo == null)
            {
                mCurTriggeredCombo = tc;
            }
        }

        if(mCurTriggeredCombo != null)
        {
            comboLogicEndMeter = meterIndex + mCurTriggeredCombo.GetCurrentComboStep().meterLen;
        }

        if(mCurTriggeredCombo != null)
        {
            return ComboDefine.ComboTriggerResult_Succeed;
        }
        else
        {
            Log.Error(LogLevel.Info, "--------------------------------newInput trigger combo failed,input:{0}", newInput);
            return ComboDefine.ComboTriggerResult_NoSuchCombo;
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
        mCurTriggeredCombo = null;
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
