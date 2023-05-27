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

    /// <summary>
    /// 动画过程中的移动信息
    /// 该移动只应用于角色的朝向方向上
    /// 和hit点对应 movements[1] 表示 hitPoints[0] 到 hitPoints[1]的移动距离
    /// </summary>
    public Movement[] movements;
}
