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
        if (!CheckTrigger(meterIndex))
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTime();
        this.transform.localScale = zoomFrom;
        timeRecord = 0;
    }
}
