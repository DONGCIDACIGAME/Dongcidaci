using UnityEngine;

public class ContinueMoveWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// ԭʼλ��
    /// </summary>
    private Vector3 mOriPos;

    /// <summary>
    /// ƫ�Ƶ�λ��
    /// </summary>
    public Vector3 MoveOffset;

    /// <summary>
    /// �ƶ���ʱ��
    /// </summary>
    public float MoveDuration;

    protected override void Initialize()
    {
        base.Initialize();
        mOriPos = this.transform.position;
    }

    public override void OnMeter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        mOriPos = this.transform.position;
        timeRecord = 0;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= MoveDuration)
            return;

        float progress = timeRecord / MoveDuration;

        // �������ŵı���
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
        if(timeRecord >= MoveDuration)
        {
            this.transform.position = mOriPos + MoveOffset;
        }
    }
}
