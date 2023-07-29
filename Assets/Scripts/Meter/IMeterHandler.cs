public interface IMeterHandler
{
    /// <summary>
    /// 节拍开始
    /// </summary>
    /// <param name="meterIndex">要开始的节拍</param>
    void OnMeterEnter(int meterIndex);

    /// <summary>
    /// 节拍结束
    /// </summary>
    /// <param name="meterIndex">要结束的节拍</param>
    void OnMeterEnd(int meterIndex);

    /// <summary>
    /// 节拍前的表现进入点
    /// </summary>
    /// <param name="meterIndex">要卡的节拍</param>
    void OnDisplayPointBeforeMeterEnter(int meterIndex);
}

