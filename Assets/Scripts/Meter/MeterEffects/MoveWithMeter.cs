using UnityEngine;
public class MoveWithMeter : WithMeter
{
    /// <summary>
    /// 原始位置
    /// </summary>
    private Vector3 mOriPos;

    public Vector3 MoveOffset;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void OnMeter(int meterIndex)
    {
        if (!CheckTrigger(meterIndex))
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTime();
        mOriPos = this.transform.position;
        timeRecord = 0;
    }

    public override void OnUpdate(float deltaTime)
    {


        if (timeRecord >= meterDuration)
        {
            return;
        }

        // 通过时间（归一化）查出出当前的缩放进度（归一化）
        float progress = Curve.Evaluate((timeRecord % meterDuration) / meterDuration);
        // 计算缩放的比例
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
    }
}
