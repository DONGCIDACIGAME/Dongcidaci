
/// <summary>
/// Added by weng 0708
/// Combo的类型种类
/// </summary>
public enum ComboType
{
    // 常规连招
    RegularCombo = 0,
    // 蓄力连招
    ChargeCombo,
    // 冲刺连招
    DashCombo,
    // 终极大招
    UltimateCombo
}



[System.Serializable]
public class ComboData
{
    /// <summary>
    /// combo Id
    /// </summary>
    public int comboUID;

    /// <summary>
    /// combo名称
    /// </summary>
    public string comboName;

    /// <summary>
    /// 这个combo的类别
    /// </summary>
    public ComboType comboType;

    /// <summary>
    /// combo招式数据
    /// </summary>
    public ComboStep[] comboSteps;

    /// <summary>
    /// combo的转换状态时间（占拍子的百分比时间）
    /// </summary>
    public float transferStateDuration;


}
