[System.Serializable]
public class AgentAnimStateInfo
{
    /// <summary>
    /// 状态名称
    /// </summary>
    public string stateName;

    /// <summary>
    /// 动画名称
    /// </summary>
    public string animName;

    /// <summary>
    /// 循环次数
    /// </summary>
    public int loopTime;

    /// <summary>
    /// 动画状态占几拍
    /// </summary>
    public int meterLen;

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

    public AgentAnimStateInfo Copy()
    {
        AgentAnimStateInfo newStateInfo = new AgentAnimStateInfo()
        {
            stateName = this.stateName,
            animName = this.animName,
            layer = this.layer,
            loopTime = this.loopTime,
            animLen = this.animLen,
            meterLen = this.meterLen,
            normalizedTime = this.normalizedTime,
            hitPoints = this.hitPoints,
            movements = this.movements
        };

        return newStateInfo;
    }
}
