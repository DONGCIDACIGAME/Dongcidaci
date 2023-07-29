using UnityEngine;

public class ContinueMoveWithMeter : BehaviourWithMeter
{
    /// <summary>
    /// 原始位置
    /// </summary>
    private Vector3 mOriPos;

    /// <summary>
    /// 偏移的位移
    /// </summary>
    public Vector3 MoveOffset;


    protected override void Initialize()
    {
        base.Initialize();
        mOriPos = this.transform.position;
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

        mOriPos = this.transform.position;
        timeRecord = 0;
    }

    public override void OnGameUpdate(float deltaTime)
    {
        if (!UpdateEnable || !meterTriggered)
            return;

        if (timeRecord >= GamePlayDefine.DisplayTimeToMatchMeter)
            return;

        float progress = timeRecord / GamePlayDefine.DisplayTimeToMatchMeter;

        // 计算偏移量
        Vector3 offset = Vector3.Lerp(Vector3.zero, MoveOffset, progress);
        this.transform.position = mOriPos + offset;
        timeRecord += deltaTime;
        if(timeRecord >= GamePlayDefine.DisplayTimeToMatchMeter)
        {
            this.transform.position = mOriPos + MoveOffset;
        }
    }
}
