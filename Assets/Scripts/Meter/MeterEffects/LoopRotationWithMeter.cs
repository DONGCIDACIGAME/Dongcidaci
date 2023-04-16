using UnityEngine;

public class LoopRotationWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// 原始角度
    /// </summary>
    private Vector3 mOriRotation;

    /// <summary>
    /// 旋转的角度
    /// </summary>
    public Vector3 RotationOffset;

    /// <summary>
    /// 旋转的时间
    /// </summary>
    public float RotateDuration;

    protected override void Initialize()
    {
        base.Initialize();

        mOriRotation = this.transform.rotation.eulerAngles;
    }


    public override void OnMeter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        mOriRotation = this.transform.rotation.eulerAngles;
        timeRecord = 0;
    }

    public override void OnUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= RotateDuration)
            return;

        float progress = timeRecord / RotateDuration;
        if (progress >= 0.5f)
        {
            progress = 1f - progress;
        }

        progress *= 2;

        Vector3 rotateOffset = Vector3.Lerp(Vector3.zero, RotationOffset, progress);
        Vector3 rotate = mOriRotation + rotateOffset;
        this.transform.rotation = Quaternion.Euler(rotate);
        timeRecord += deltaTime;
        if (timeRecord >= RotateDuration)
        {
            this.transform.rotation = Quaternion.Euler(mOriRotation);
        }
    }
}
