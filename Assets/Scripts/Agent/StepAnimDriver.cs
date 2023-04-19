/// <summary>
/// 步进式动画驱动
/// 完全按照配置驱动动画进行
/// 步进式的动画驱动，动画将会按照配置自动循环，即走完一遍配置的动画循环后从头开始
/// </summary>
public class StepAnimDriver : AgentAnimDriver
{
    private int curStateIndex;
    private int curStateLoopRecord;

    public StepAnimDriver(Agent agt, string statusName) : base(agt, statusName)
    {
        curStateIndex = 0;
        curStateLoopRecord = 0;
    }

    protected int AnimKeepPlay()
    {
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "AnimKeepPlay Error, mCurAnimState is null!");
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToBaseMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "AnimKeepPlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        mAgent.AnimPlayer.UpdateAnimSpeedWithFix(mCurAnimState.layer, mCurAnimState.animLen, duration);
        return endMeterIndex;
    }

    protected int AnimMoveNextPlay()
    {
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "AnimMoveNextPlay Error, mCurAnimState is null!");
            return MeterManager.Ins.MeterIndex;
        }

        string stateName = mCurAnimState.stateName;
        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "AnimMoveNextPlay Error, mCurAnimState.stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToBaseMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "AnimMoveNextPlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);
        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);
        return endMeterIndex;
    }

    public int MoveNext()
    {
        if (mAnimStates == null)
        {
            Log.Error(LogLevel.Info, "StepAnimDriver MoveNext Error, mAnimStates is null!");
            return MeterManager.Ins.MeterIndex;
        }

        mCurAnimState = mAnimStates[curStateIndex];
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Info, "StepAnimDriver MoveNext, AgentAnimStateInfo is null at index:{0}", curStateIndex);
            return MeterManager.Ins.MeterIndex;
        }

        if (mCurAnimState.loopTime > 0)
        {
            curStateLoopRecord++;
        }

        // 规定循环次数设定为0时为无限循环
        if (mCurAnimState.loopTime == 0)
        {
            return AnimKeepPlay();
        }

        if (curStateLoopRecord <= mCurAnimState.loopTime)
        {
            return AnimKeepPlay();
        }

        // 当前动画状态的循环次数达到目标循环次数时，进入下一个动画状态
        curStateLoopRecord = 0;
        curStateIndex++;

        if (curStateIndex >= mAnimStates.Length)
        {
            curStateIndex = 0;
        }

        mCurAnimState = mAnimStates[curStateIndex];
        return AnimMoveNextPlay();
    }

    public void Dispose()
    {
        mAnimStates = null;
        Reset();
    }

    public void Reset()
    {
        curStateIndex = 0;
        curStateLoopRecord = 0;
        mCurAnimState = null;
    }

}