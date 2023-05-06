using System.Collections.Generic;

public class ComboHandler
{
    /// <summary>
    /// ���б�
    /// ����Ҫ��ĳ���������������������
    /// </summary>
    private List<Combo> mSortedCombos;

    /// <summary>
    /// �Ѿ���������ʽ����
    /// </summary>
    private List<ComboStep> mTriggeredComboActions;

    /// <summary>
    /// �ϴδ���combo�Ľ���index
    /// </summary>
    private int lastComboMeterIndex;

    public ComboHandler()
    {
        mSortedCombos = new List<Combo>();
        mTriggeredComboActions = new List<ComboStep>();
    }


    public void Initialize(ComboGraph comboGraph)
    {
        if (comboGraph == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, comboGraph is null!");
            return;
        }

        if(comboGraph.combos == null)
        {
            Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combos is null, agentId:{0},  agentName:{1}",comboGraph.agentId, comboGraph.agentName);
            return;
        }

        mSortedCombos.Clear();

        for(int i = 0; i < comboGraph.combos.Length;i++)
        {
            Combo cmb = comboGraph.combos[i];
            if (cmb == null)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, ComboGraph has null combo at index {0}, agent id:{1}, agent Name:{2}", i, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            if (cmb.comboSteps == null || cmb.comboSteps.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo comboMoves is null or emtpty, combo name: {0}, agent id:{1}, agent Name:{2}", cmb.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            for(int j = mSortedCombos.Count-1; j >=0 ; j--)
            {
                Combo _cmb = mSortedCombos[j];
                if(_cmb.comboSteps.Length <= cmb.comboSteps.Length)
                {
                    mSortedCombos.Insert(j, cmb);
                    break;
                }
            }

            mSortedCombos.Add(cmb);
        }


    }

    /// <summary>
    /// ����ƥ����
    /// </summary>
    /// <param name="newInput"></param>
    /// <param name="inputRecord"></param>
    /// <param name="cmb"></param>
    /// <returns></returns>
    private bool CheckMatchCombo(byte newInput, Combo cmb, out ComboStep cm)
    {
        cm = null;

        if (cmb.comboSteps == null || cmb.comboSteps.Length == 0)
        {
            Log.Error(LogLevel.Info, "CheckMatchCombo Error, cmb.comboMoves null or empty!");
            return false;
        }

        int len = cmb.comboSteps.Length;

        if (len < mTriggeredComboActions.Count + 1)
            return false;

        int index;
        for(index = 0; index < mTriggeredComboActions.Count; index++)
        {
            if(cmb.comboSteps[index].input != mTriggeredComboActions[index].input)
            {
                return false;
            }
        }
    
        if(newInput != cmb.comboSteps[index].input)
        {
            return false;
        }

        cm = cmb.comboSteps[index];
        return true;
    }

    private void AddComboActionRecord(ComboStep cm)
    {
        mTriggeredComboActions.Add(cm);
    }

    public void ClearComboActionRecord()
    {
        mTriggeredComboActions.Clear();
    }

    public bool TryTriggerCombo(byte cmd, int meterIndex, out Combo combo, out ComboStep comboMove)
    {
        combo = null;
        comboMove = null;

        // ͬһ��ֻ�ܴ���һ��combo
        if (meterIndex == lastComboMeterIndex)
            return false;

        if (mSortedCombos == null || mSortedCombos.Count == 0)
        {
            Log.Error(LogLevel.Info, "ComboHandler OnInput  Error, mSortedCombos null or empty!");
            return false;
        }

        for (int i = 0; i < mSortedCombos.Count; i++)
        {
            Combo cmb = mSortedCombos[i];

            if (cmb == null)
                continue;

            if(CheckMatchCombo(cmd, cmb, out ComboStep cm))
            {
                combo = cmb;
                comboMove = cm;
                if(comboMove.endFlag)
                {
                    ClearComboActionRecord();
                }
                else
                {
                    AddComboActionRecord(comboMove);
                }
                lastComboMeterIndex = meterIndex;
                return true;
            }
        }
        
        //Log.Error(LogLevel.Info, "ComboHandler OnInput  Error, no match combo with cmd:{0}", cmd);
        return false;
    }

    public void Dispose()
    {
        mSortedCombos = null;
        mTriggeredComboActions = null;
        lastComboMeterIndex = -1;
    }
}
