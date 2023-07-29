using UnityEngine;

public class CurveMoveWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// 时长（本节拍时长）
    /// </summary>
    protected float meterDuration;

    /// <summary>
    /// 原始位置
    /// </summary>
    private Vector3 mOriPos;

    /// <summary>
    /// 偏移的位移
    /// </summary>
    public Vector3 MoveOffset;

    /// <summary>
    /// 缩放曲线 
    /// 横轴表示时间的归一化
    /// 纵轴表示缩放进度的归一化
    /// </summary>
    public AnimationCurve Curve;

    public override void OnMeterEnter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTotalTime();
        mOriPos = this.transform.position;
        timeRecord = 0;
    }

    public override void OnGameUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= meterDuration)
            return;

        // 通过时间（归一化）查出出当前进度（归一化）
        float progress = Curve.Evaluate((timeRecord % meterDuration) / meterDuration);
        // 计算移动的距离
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
        if (timeRecord >= meterDuration)
        {
            this.transform.position = mOriPos;
        }
    }

    public override void OnMeterEnd(int meterIndex)
    {
        
    }

    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        
    }
}
