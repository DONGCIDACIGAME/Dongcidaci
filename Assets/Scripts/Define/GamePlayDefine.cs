public static class GamePlayDefine
{
    /// <summary>
    /// 输入和节拍检测时的容差
    /// 向前和向后各
    /// </summary>
    public const float MeterCheckTolerance = 0.3f;

    /// <summary>
    /// 当检测到指令时，距离下一拍的时间t，如果t<=WaitMeterMaxTime，则等待下一拍到了在开始执行指令
    /// </summary>
    public const float WaitMeterMaxTimeOnCmd = 0.2f;

    /// <summary>
    /// 当检测到指令时，当前拍的剩余时长占总时长的比例p，如果p>=MinMeterProgressOnCmd，则直接开始执行指令
    /// </summary>
    public const float MinMeterProgressOnCmd = 0.8f;


}
