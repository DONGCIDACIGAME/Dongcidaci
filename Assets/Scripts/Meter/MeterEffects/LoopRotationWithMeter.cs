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


    protected override void Initialize()
    {
        base.Initialize();

        mOriRotation = this.transform.rotation.eulerAngles;
    }


    public override void OnMeterEnter(int meterIndex)
    {

    }

    public override void OnMeterEnd(int meterIndex)
    {

    }

    public override void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        meterTriggered = CheckTrigger(meterIndex);
        if (!meterTriggered)
            return;

        mOriRotation = this.transform.rotation.eulerAngles;
        timeRecord = 0;
    }

    public override void OnGameUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= GamePlayDefine.DisplayTimeToMatchMeter)
            return;

        float progress = timeRecord / GamePlayDefine.DisplayTimeToMatchMeter;
        if (progress >= 0.5f)
        {
            progress = 1f - progress;
        }

        progress *= 2;

        Vector3 rotateOffset = Vector3.Lerp(Vector3.zero, RotationOffset, progress);
        Vector3 rotate = mOriRotation + rotateOffset;
        this.transform.rotation = Quaternion.Euler(rotate);
        timeRecord += deltaTime;
        if (timeRecord >= GamePlayDefine.DisplayTimeToMatchMeter)
        {
            this.transform.rotation = Quaternion.Euler(mOriRotation);
        }
    }
}
