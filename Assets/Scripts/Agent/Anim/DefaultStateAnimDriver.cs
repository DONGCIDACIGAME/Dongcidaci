public class DefaultStateAnimDriver : AgentAnimDriver
{
    public DefaultStateAnimDriver(Agent agt, string statusName) : base(agt)
    {

    }

    public int Play()
    {
        //if(mAnimStates == null || mAnimStates.Length == 0)
        //{
        //    Log.Error(LogLevel.Normal, "DefaultStateAnimDriver Play Error, mAnimStates is null or empty!");
        //    return MeterManager.Ins.MeterIndex;
        //}

        //mCurAnimState = mAnimStates[0];

        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "DefaultStateAnimDriver Play Error, default anim state is null!");
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "DefaultStateAnimDriver Play Error time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);

        // 原来的逻辑，直接融合
        mAgent.AnimPlayer.CrossFadeToStateDynamic(mCurAnimState.stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);
        return endMeterIndex;
    }

}
