[System.Serializable]
public class Movement 
{
    /// <summary>
    /// 的移动的距离
    /// </summary>
    public float distance;

    /// <summary>
    /// 时间配置类型
    /// 0：节拍进度
    /// 1：绝对时间
    /// </summary>
    public int timeType;

    /// <summary>
    /// 开始占比
    /// </summary>
    public float startTime;

    /// <summary>
    /// 结束占比
    /// </summary>
    public float endTime;
}
