using UnityEngine;

public class ZoomWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// ʱ����������ʱ����
    /// </summary>
    protected float meterDuration;

    /// <summary>
    /// ������ʼֵ
    /// </summary>
    public Vector3 zoomFrom;

    /// <summary>
    /// ������ֵֹ
    /// </summary>
    public Vector3 zoomTo;

    /// <summary>
    /// �������� 
    /// �����ʾʱ��Ĺ�һ��
    /// �����ʾ���Ž��ȵĹ�һ��
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

        // ͨ��ʱ�䣨��һ�����������ǰ�����Ž��ȣ���һ����
        float progress = Curve.Evaluate((timeRecord % meterDuration)/meterDuration);
        // �������ŵı���
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
