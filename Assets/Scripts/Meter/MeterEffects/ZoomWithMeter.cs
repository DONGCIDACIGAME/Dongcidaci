using UnityEngine;

public class ZoomWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// 时长（本节拍时长）
    /// </summary>
    protected float meterDuration;

    /// <summary>
    /// 缩放起始值
    /// </summary>
    public Vector3 zoomFrom;

    /// <summary>
    /// 缩放终止值
    /// </summary>
    public Vector3 zoomTo;

    /// <summary>
    /// 缩放曲线 
    /// 横轴表示时间的归一化
    /// 纵轴表示缩放进度的归一化
    /// </summary>
    public AnimationCurve Curve;
    protected override void Initialize()
    {
        base.Initialize();

        this.transform.localScale = zoomFrom;
    }


    public override void OnUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= meterDuration)
            return;

        // 通过时间（归一化）查出出当前的缩放进度（归一化）
        float progress = Curve.Evaluate((timeRecord % meterDuration)/meterDuration);
        // 计算缩放的比例
        Vector3 scale = Vector3.Lerp(zoomFrom, zoomTo, progress);
        this.transform.localScale = scale;
        timeRecord += deltaTime;
    }

    public override void OnMeter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTime();
        this.transform.localScale = zoomFrom;
        timeRecord = 0;
    }
}
