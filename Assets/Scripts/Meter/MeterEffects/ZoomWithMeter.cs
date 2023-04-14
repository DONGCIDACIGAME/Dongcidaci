using UnityEngine;

public class ZoomWithMeter : WithMeter
{
    /// <summary>
    /// 缩放起始值
    /// </summary>
    public Vector3 zoomFrom;

    /// <summary>
    /// 缩放终止值
    /// </summary>
    public Vector3 zoomTo;

    /// <summary>
    /// 奇数拍是否触发
    /// </summary>
    public bool OddTriggered;

    /// <summary>
    /// 偶数拍是否触发
    /// </summary>
    public bool EvenTriggered;

    protected override void Initialize()
    {
        base.Initialize();

        this.transform.localScale = zoomFrom;
    }


    public override void OnUpdate(float deltaTime)
    {
        if (timeRecord >= meterDuration)
        {
            return;
        }

        // 通过时间（归一化）查出出当前的缩放进度（归一化）
        float progress = Curve.Evaluate((timeRecord % meterDuration)/meterDuration);
        // 计算缩放的比例
        Vector3 scale = Vector3.Lerp(zoomFrom, zoomTo, progress);
        this.transform.localScale = scale;
        timeRecord += deltaTime;
    }

    public override void OnMeter(int meterIndex)
    {
        // 本拍为偶数拍
        if (meterIndex % 2 == 0 && !EvenTriggered)
            return;

        // 本拍为奇数拍
        if (meterIndex % 2 == 1 && !OddTriggered)
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTime();
        this.transform.localScale = zoomFrom;
        timeRecord = 0;
    }
}
