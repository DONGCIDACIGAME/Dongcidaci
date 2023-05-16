/// <summary>
/// Combo的一个招式数据
/// </summary>
[System.Serializable]
public class ComboActionData
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public byte input;

    /// statusName+stateName能够匹配出一个动画状态
    
    /// <summary>
    /// 状态名称
    /// </summary>
    public string statusName;

    /// <summary>
    /// 动画名称
    /// </summary>
    public string stateName;

    /// <summary>
    /// 招式在所有击打点上的效果数组
    /// </summary>
    public ComboHitEffect[] effects;

    /// <summary>
    /// 结束combo的标志
    /// </summary>
    public bool endFlag;
}
