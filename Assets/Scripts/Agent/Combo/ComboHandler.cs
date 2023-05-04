using System.Collections.Generic;

public class ComboHandler
{
    /// <summary>
    /// 出招表
    /// 按照要求的出招数量进行了降序排列
    /// </summary>
    private List<Combo> mSortedCombos;

    private List<ComboMove> mTriggeredComboActions;

    public ComboHandler()
    {
        mSortedCombos = new List<Combo>();
        mTriggeredComboActions = new List<ComboMove>();
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

            if (cmb.comboMoves == null || cmb.comboMoves.Length == 0)
            {
                Log.Error(LogLevel.Normal, "ComboHandler Initialize Error, combo comboMoves is null or emtpty, combo name: {0}, agent id:{1}, agent Name:{2}", cmb.comboName, comboGraph.agentId, comboGraph.agentName);
                continue;
            }

            for(int j = mSortedCombos.Count-1; j >=0 ; j--)
            {
                Combo _cmb = mSortedCombos[j];
                if(_cmb.comboMoves.Length <= cmb.comboMoves.Length)
                {
                    mSortedCombos.Insert(j, cmb);
                    break;
                }
            }

            mSortedCombos.Add(cmb);
        }


    }

    /// <summary>
    /// 出招匹配检测
    /// </summary>
    /// <param name="newInput"></param>
    /// <param name="inputRecord"></param>
    /// <param name="cmb"></param>
    /// <returns></returns>
    private bool CheckMatchCombo(byte newInput, Combo cmb, out ComboMove cm)
    {
        cm = null;

        if (cmb.comboMoves == null || cmb.comboMoves.Length == 0)
        {
            Log.Error(LogLevel.Info, "CheckMatchCombo Error, cmb.comboMoves null or empty!");
            return false;
        }

        int len = cmb.comboMoves.Length;

        if (len < mTriggeredComboActions.Count + 1)
            return false;

        int index;
        for(index = 0; index < mTriggeredComboActions.Count; index++)
        {
            if(cmb.comboMoves[index].moveType != mTriggeredComboActions[index].moveType)
            {
                return false;
            }
        }
    
        if(newInput != cmb.comboMoves[index].moveType)
        {
            return false;
        }

        cm = cmb.comboMoves[index];
        return true;
    }

    private void AddComboActionRecord(ComboMove cm)
    {
        mTriggeredComboActions.Add(cm);
    }

    public void ClearComboActionRecord()
    {
        mTriggeredComboActions.Clear();
    }

    public bool OnCmd(byte cmd, out Combo combo, out ComboMove comboMove)
    {
        combo = null;
        comboMove = null;

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

            if(CheckMatchCombo(cmd, cmb, out ComboMove cm))
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
                return true;
            }
        }
        
        //Log.Error(LogLevel.Info, "ComboHandler OnInput  Error, no match combo with cmd:{0}", cmd);
        return false;
    }

}
