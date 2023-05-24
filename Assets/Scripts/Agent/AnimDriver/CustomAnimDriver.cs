/// <summary>
/// 自定义的动画驱动
/// </summary>
public class CustomAnimDriver : AgentAnimDriver
{
    public CustomAnimDriver(Agent agt, string statusName) : base(agt, statusName)
    {

    }

    /// <summary>
    /// 不带截断的动画状态播放
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int PlayAnimStateWithoutCut(string stateName)
    {
        Log.Error(LogLevel.Info, "PlayAnimStateWithoutCut=======================play {0}", stateName);

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        for (int i = 0; i < mAnimStates.Length; i++)
        {
            AgentAnimStateInfo stateInfo = mAnimStates[i];
            if (stateInfo == null)
                continue;

            if (stateInfo.stateName.Equals(stateName))
            {
                mCurAnimState = stateInfo;
            }
        }

        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut PlayAnimState Error, no state named {0}!", stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut PlayAnimState Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);

        // 原来的逻辑，直接融合
        mAgent.AnimPlayer.CrossFadeToStateStatic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, 0, mCurAnimState.animLen, totalMeterTime);
        return endMeterIndex;
    }

    /// <summary>
    /// 带截断的动画状态播放
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int PlayAnimStateWithCut(string stateName)
    {
        Log.Error(LogLevel.Info, "PlayAnimStateWithCut=======================play {0}", stateName);

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut PlayAnimState Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        for(int i = 0; i < mAnimStates.Length; i++)
        {
            AgentAnimStateInfo stateInfo = mAnimStates[i];
            if (stateInfo == null)
                continue;

            if(stateInfo.stateName.Equals(stateName))
            {
                mCurAnimState = stateInfo;
            }
        }

        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut PlayAnimState Error, no state named {0}!", stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut PlayAnimState Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);

        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);

        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex -1;
    }

}
