/// <summary>
/// 卡节拍的融合动画驱动
/// </summary>
public class MatchMeterCrossfadeAnimDriver : AgentAnimDriver
{
    public MatchMeterCrossfadeAnimDriver(Agent agt) : base(agt)
    {

    }


    /// <summary>
    /// 带截断的动画状态播放
    /// 
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int CrossFadeToState(string statusName, string stateName)
    {
        //Log.Error(LogLevel.Info, "PlayAnimStateWithCut=======================play {0}-{1}", statusName, stateName);

        Log.Logic(LogLevel.Info, "<color=blue>{0} CrossFadeToState--statusName:{1}, stateName:{2}</color>", mAgent.GetAgentId(), statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, statusName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        AgentAnimStateInfo newStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        if (newStateInfo == null)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, status:{0} can not find state:{1}", statusName, stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeterWithOffset(newStateInfo.meterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, time to target meter is 0,anim meter len:{0}", newStateInfo.meterLen);
            return MeterManager.Ins.MeterIndex;
        }


        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, newStateInfo.meterLen);

        // 同一个状态的动画无法进行融合
        // 转为从头完整播放
        if (mCurAnimState != null && mCurAnimState.stateName.Equals(stateName))
        {
            mAgent.AnimPlayer.PlayState(stateName, newStateInfo.animLen, newStateInfo.layer, 0, duration);
            mCurAnimState = newStateInfo;
            return newMeterIndex - 1;
        }

        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);
        //Log.Error(LogLevel.Info, "CrossFadeToState ---------- {0} ---{1} ----{2} --- {3}", statusName, stateName, duration, totalMeterTime);

        mAgent.AnimPlayer.CrossFadeToState(stateName, newStateInfo.layer, newStateInfo.normalizedTime, newStateInfo.animLen, duration, totalMeterTime);
        
        mCurAnimState = newStateInfo;
        
        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex -1;
    }

    public void Reset()
    {
        mCurAnimState = null;
    }
}
