/// <summary>
/// 自定义的动画驱动
/// </summary>
public class CustomAnimDriver : AgentAnimDriver
{
    public CustomAnimDriver(Agent agt) : base(agt)
    {

    }

    /// <summary>
    /// 不带截断的动画状态播放
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int PlayAnimStateWithoutCut(string statusName, string stateName)
    {
        //Log.Error(LogLevel.Info, "PlayAnimStateWithoutCut=======================play {0}", stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut Error, statusName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        mCurAnimState = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);

        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut Error, status:{0} can not find state:{1}", statusName, stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithoutCut Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);

        // 原来的逻辑，直接融合
        mAgent.AnimPlayer.CrossFadeToStateStatic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, 0, mCurAnimState.animLen, totalMeterTime);

        // 动画的移动
        mAgent.MovementExcutorCtl.Start(statusName, stateName, mCurAnimState.movements);

        return endMeterIndex;
    }

    /// <summary>
    /// 带截断的动画状态播放
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int PlayAnimStateWithCut(string statusName, string stateName)
    {
        //Log.Error(LogLevel.Info, "PlayAnimStateWithCut=======================play {0}-{1}", statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut Error, statusName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        mCurAnimState = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);

        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut Error, status:{0} can not find state:{1}", statusName, stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "PlayAnimStateWithCut Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);

        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);

        mAgent.MovementExcutorCtl.Start(statusName, stateName, mCurAnimState.movements);
        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex -1;
    }

}
