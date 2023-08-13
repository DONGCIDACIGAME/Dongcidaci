/// <summary>
/// Combo的一个招式数据
/// </summary>
[System.Serializable]
public class ComboStep
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public int input;

    /// <summary>
    /// 攻击行为数据
    /// </summary>
    public AgentActionData attackActionData;

    /// <summary>
    /// 过度行为数据
    /// </summary>
    public AgentActionData transitionData;

    /// <summary>
    /// 结束combo的标志
    /// </summary>
    public bool endFlag;
}
