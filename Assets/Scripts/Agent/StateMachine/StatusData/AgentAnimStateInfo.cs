[System.Serializable]
public class AgentAnimStateInfo
{
    /// <summary>
    /// 动画状态名称
    /// </summary>
    public string stateName;

    /// <summary>
    /// 循环次数
    /// </summary>
    public int loopTime;

    /// <summary>
    /// 动画的名称
    /// </summary>
    public string animName;

    /// <summary>
    /// 动画状态占几拍
    /// </summary>
    public int stateMeterLen;

    /// <summary>
    /// 动画所在layer
    /// </summary>
    public int layer;

    /// <summary>
    /// 与其他动画的融合时间
    /// </summary>
    public float normalizedTime;

    /// <summary>
    /// 动画长度
    /// </summary>
    public float animLen;

    /// <summary>
    /// 所有的hit点信息
    /// </summary>
    public HitPointInfo[] hitPoints;
}
