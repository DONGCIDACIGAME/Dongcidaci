using UnityEngine;
public class MoveWithMeter : WithMeter
{
    /// <summary>
    /// ԭʼλ��
    /// </summary>
    private Vector3 mOriPos;

    public Vector3 MoveOffset;

    /// <summary>
    /// �������Ƿ񴥷�
    /// </summary>
    public bool OddTriggered;

    /// <summary>
    /// ż�����Ƿ񴥷�
    /// </summary>
    public bool EvenTriggered;
    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void OnMeter(int meterIndex)
    {
        // ����Ϊż����
        if (meterIndex % 2 == 0 && !EvenTriggered)
            return;

        // ����Ϊ������
        if (meterIndex % 2 == 1 && !OddTriggered)
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
