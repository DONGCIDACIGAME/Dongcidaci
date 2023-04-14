using UnityEngine;
public class MoveWithMeter : WithMeter
{
    /// <summary>
    /// ԭʼλ��
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

        // ͨ��ʱ�䣨��һ�����������ǰ�����Ž��ȣ���һ����
        float progress = Curve.Evaluate((timeRecord % meterDuration) / meterDuration);
        // �������ŵı���
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
    }
}
