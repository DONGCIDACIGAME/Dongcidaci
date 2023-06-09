using UnityEngine;

public static class GamePlayDefine
{
    public const float AttackMeterCheckTolerance = 0.4f;
    /// <summary>
    /// 攻击指令的节拍检测偏移
    /// </summary>
    public const float AttackMeterCheckOffset = 0f;

    /// <summary>
    /// 冲刺指令的节拍检测总容差
    /// </summary>
    public const float DashMeterCheckTolerance = 0.4f;
    /// <summary>
    /// 冲刺指令的节拍检测偏移
    /// </summary>
    public const float DashMeterCheckOffset = 0f;

    /// <summary>
    /// 攻击指令执行的节拍进度判定
    /// </summary>
    public const float AttackMeterProgressWait = 0.5f;

    /// <summary>
    /// 冲刺指令执行的节拍进度判定
    /// </summary>
    public const float DashMeterProgressWait = 0.5f;

    /// <summary>
    /// 冲刺时间占一拍时间的百分比
    /// </summary>
    public const float DashMeterTime = 0.8f;
}