using UnityEngine;

public class ZoomWithMeter : WithMeter
{
    /// <summary>
    /// ������ʼֵ
    /// </summary>
    public Vector3 zoomFrom;

    /// <summary>
    /// ������ֵֹ
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

        // ͨ��ʱ�䣨��һ�����������ǰ�����Ž��ȣ���һ����
        float progress = Curve.Evaluate((timeRecord % meterDuration)/meterDuration);
        // �������ŵı���
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
