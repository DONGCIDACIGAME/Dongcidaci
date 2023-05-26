/// <summary>
/// Combo的一个招式数据
/// </summary>
[System.Serializable]
public class ComboStep
{
    /// <summary>
    /// 输入类型
    /// </summary>
    public byte input;

    /// <summary>
    /// 生效模式
    /// 0：覆盖模式
    /// 1：叠加模式
    /// </summary>
    public int mode;

    /// <summary>
    /// 角色的
    /// </summary>
    public AgentActionData agentActionData;

    /// <summary>
    /// 结束combo的标志
    /// </summary>
    public bool endFlag;
}
