using UnityEngine;

public class CurveMoveWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// ʱ����������ʱ����
    /// </summary>
    protected float meterDuration;

    /// <summary>
    /// ԭʼλ��
    /// </summary>
    private Vector3 mOriPos;

    /// <summary>
    /// ƫ�Ƶ�λ��
    /// </summary>
    public Vector3 MoveOffset;

    /// <summary>
    /// �������� 
    /// �����ʾʱ��Ĺ�һ��
    /// �����ʾ���Ž��ȵĹ�һ��
    /// </summary>
    public AnimationCurve Curve;

    public override void OnMeter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        meterDuration = MeterManager.Ins.GetCurrentMeterTime();
        mOriPos = this.transform.position;
        timeRecord = 0;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= meterDuration)
            return;

        // ͨ��ʱ�䣨��һ�����������ǰ���ȣ���һ����
        float progress = Curve.Evaluate((timeRecord % meterDuration) / meterDuration);
        // �����ƶ��ľ���
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
        if (timeRecord >= meterDuration)
        {
            this.transform.position = mOriPos;
        }
    }
}
