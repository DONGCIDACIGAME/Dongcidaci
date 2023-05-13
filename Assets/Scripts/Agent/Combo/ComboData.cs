[System.Serializable]
public class ComboData
{
    /// <summary>
    /// combo名称
    /// </summary>
    public string comboName;


    /// <summary>
    /// combo招式数据
    /// </summary>
    public ComboStepData[] comboStepDatas;

    /// <summary>
    /// combo的转换状态名称
    /// </summary>
    public string transferStateName;

    /// <summary>
    /// combo的转换状态时间（占拍子的百分比时间）
    /// </summary>
    public float transferStateDuration;
}
