using GameSkillEffect;

[System.Serializable]
public class AgentActionData
{
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
    public SkEftDataCollection[] effectCollictions;
}
