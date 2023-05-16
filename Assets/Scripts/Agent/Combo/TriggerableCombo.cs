public class TriggerableCombo
{
    /// <summary>
    /// combo数据
    /// </summary>
    private ComboData comboData;

    /// <summary>
    /// 是否可被触发
    /// </summary>
    private bool triggerable;

    /// <summary>
    /// 当前触发的招式index
    /// </summary>
    public int triggeredAt { get; private set; }

    /// <summary>
    /// 是否激活（只有激活的combo才能响应输入）
    /// 和triggerable的区别在于，triggerable在combo打完时重置为true，active在装备combo时激活
    /// </summary>
    private bool active;

    public void Reset()
    {
        triggerable = true;
        triggeredAt = -1;
    }

    public TriggerableCombo(ComboData comboData)
    {
        this.comboData = comboData;
        active = false;
        Reset();
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    public bool IsCombo(string comboName)
    {
        return comboData.comboName.Equals(comboName);
    }

    public float GetComboTransferDuration()
    {
        return comboData.transferStateDuration;
    }

    public string GetComboName()
    {
        return comboData.comboName;
    }

    public int GetComboStepCount()
    {
        return comboData.comboActionDatas.Length;
    }

    /// <summary>
    /// 获取当前触发的招式信息
    /// </summary>
    /// <returns></returns>
    public ComboActionData GetCurrentComboAction()
    {
        if(comboData == null)
        {
            Log.Error(LogLevel.Normal, "GetCurrentComboStep Error, combo is null !");
            return null;
        }

        if(comboData.comboActionDatas == null || comboData.comboActionDatas.Length == 0)
        {
            Log.Error(LogLevel.Normal, "GetCurrentComboStep Error, comboStepDatas is null or empty! combo name:{0}", comboData.comboName);
            return null;
        }

        if (!active)
            return null;

        if (!triggerable)
            return null;

        if (triggeredAt < 0)
            return null;

        if (triggeredAt >= comboData.comboActionDatas.Length)
            return null;

        return comboData.comboActionDatas[triggeredAt];
    }

    /// <summary>
    /// 用新的输入检测能否继续触发combo的下一招
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool TryTriggerOnNewInput(byte input)
    {
        if (!active)
            return false;

        if (!triggerable)
            return false;

        if(triggeredAt >= comboData.comboActionDatas.Length-1)
        {
            triggerable = false;
            return false;
        }

        ComboActionData nextAction = comboData.comboActionDatas[triggeredAt + 1];
        if(nextAction == null)
        {
            Log.Error(LogLevel.Normal, "TryTriggerOnNewInput Error, Combo [{0}] has null action at {1}", comboData.comboName, triggeredAt + 1);
            triggerable = false;
            return false;
        }

        if(input == nextAction.input)
        {
            triggeredAt++;
            return true;
        }

        // 错误的combo输入会打断combo
        triggerable = false;
        return false;
    }

    //public override string ToString()
    //{
    //    string str = "";

    //}
}
