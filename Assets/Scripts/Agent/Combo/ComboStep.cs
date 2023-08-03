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
    /// 角色的
    /// </summary>
    public AgentActionData agentActionData;

    /// <summary>
    /// 过度动画名称
    /// </summary>
    public AgentActionData transitionData;

    /// <summary>
    /// 结束combo的标志
    /// </summary>
    public bool endFlag;
}
