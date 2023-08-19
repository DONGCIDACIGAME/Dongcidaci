[System.Serializable]
public class AgentStatusInfo
{
    /// <summary>
    /// 角色状态名称
    /// </summary>
    public string statusName;

    /// <summary>
    /// 角色状态默认行为数据
    /// </summary>
    public AgentActionData defaultAciton;

    /// <summary>
    /// 角色状态下所有的动画信息
    /// </summary>
    public AgentAnimStateInfo[] animStates;
}
