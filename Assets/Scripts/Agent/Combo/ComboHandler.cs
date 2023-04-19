using System.Collections.Generic;

public class ComboHandler
{
    /// <summary>
    /// ���б�
    /// ����Ҫ��ĳ������������˽�������
    /// </summary>
    private List<Combo> mSortedCombos;

    /// <summary>
    /// �Ѿ����������Ч�������¼
    /// ���mComboRecord+�µ�������ɵ������������г��б�ƥ�䲻�ϣ� ���ж�����ʧ�ܣ���ճ��м�¼
    /// </summary>
    private List<byte> mComboRecord;

    public ComboHandler()
    {
        mSortedCombos = new List<Combo>();
        mComboRecord = new List<byte>();
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

            if (cmb.inputList == null || cmb.inputList.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo input list is null or emtpty, combo name: {0}, agent id:{1}, agent Name:{2}", cmb.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            int addIndex = mSortedCombos.Count;
            for(int j = 0; j < mSortedCombos.Count; j++)
            {
                Combo _cmb = mSortedCombos[j];
                if(cmb.inputList.Length > _cmb.inputList.Length)
                {
                    addIndex = j;
                    break;
                }
            }

            mSortedCombos.Insert(addIndex, cmb);
        }
    }

    /// <summary>
    /// ����ƥ����
    /// </summary>
    /// <param name="newInput"></param>
    /// <param name="inputRecord"></param>
    /// <param name="cmb"></param>
    /// <returns></returns>
    private bool CheckMatchCombo(byte newInput, List<byte> inputRecord, Combo cmb)
    {
        if (cmb.inputList.Length > inputRecord.Count + 1)
            return false;

        if (newInput != cmb.inputList[0])
            return false;

        if(inputRecord.Count > 0)
        {
            for (int i = 1; i < cmb.inputList.Length; i++)
            {
                if (cmb.inputList[i] != inputRecord[i - 1])
                    return false;
            }
        }

        return true;
    }

    private void ResetRecord(Combo cmb)
    {
        mComboRecord.Clear();
        for(int i = 0;i<cmb.inputList.Length;i++)
        {
            mComboRecord.Add(cmb.inputList[i]);
        }
    }

    public Combo OnInput(byte input)
    {
        if (mSortedCombos == null || mSortedCombos.Count == 0)
            return null;

        for (int i = 0; i < mSortedCombos.Count; i++)
        {
            Combo cmb = mSortedCombos[i];

            if (cmb == null)
                continue;

            if(CheckMatchCombo(input, mComboRecord, cmb))
            {
                ResetRecord(cmb);
                return cmb;
            }
        }

        return null;
    }

}
