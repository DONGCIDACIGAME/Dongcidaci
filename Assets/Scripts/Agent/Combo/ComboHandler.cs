using System.Collections.Generic;

public class ComboHandler
{
    /// <summary>
    /// 出招表
    /// 按照要求的出招数量进行了降序排列
    /// </summary>
    private List<Combo> mSortedCombos;

    /// <summary>
    /// 已经打出来的有效的输入记录
    /// 如果mComboRecord+新的输入组成的招数，跟所有出招表都匹配不上， 则判定出招失败，清空出招记录
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
    /// 出招匹配检测
    /// </summary>
    /// <param name="newInput"></param>
    /// <param name="inputRecord"></param>
    /// <param name="cmb"></param>
    /// <returns></returns>
    private bool CheckMatchCombo(byte newInput, List<byte> inputRecord, Combo cmb)
    {
        if (cmb.inputList == null || cmb.inputList.Length == 0)
        {
            Log.Error(LogLevel.Info, "CheckMatchCombo Error, cmb.inputList null or empty!");
            return false;
        }

        int len = cmb.inputList.Length;

        if (len > inputRecord.Count + 1)
            return false;

        if (newInput != cmb.inputList[len-1])
            return false;

        if(inputRecord.Count > 0)
        {
            for (int i = 0; i < len-1; i++)
            {
                if (cmb.inputList[i] != inputRecord[i])
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

    public Combo OnCmd(byte cmd)
    {
        if (mSortedCombos == null || mSortedCombos.Count == 0)
        {
            Log.Error(LogLevel.Info, "ComboHandler OnInput  Error, mSortedCombos null or empty!");
            return null;
        }


        for (int i = 0; i < mSortedCombos.Count; i++)
        {
            Combo cmb = mSortedCombos[i];

            if (cmb == null)
                continue;

            if(CheckMatchCombo(cmd, mComboRecord, cmb))
            {
                ResetRecord(cmb);
                return cmb;
            }
        }

        //Log.Error(LogLevel.Info, "ComboHandler OnInput  Error, no match combo with cmd:{0}", cmd);
        return null;
    }

}
