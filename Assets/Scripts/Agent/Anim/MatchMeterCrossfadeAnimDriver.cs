/// <summary>
/// 卡节拍的融合动画驱动
/// </summary>
public class MatchMeterCrossfadeAnimDriver : AgentAnimDriver, IMeterHandler
{
    private int mCurLoopEndMeter;

    private int mLoopRecord;

    public MatchMeterCrossfadeAnimDriver(Agent agt) : base(agt)
    {

    }

    /// <summary>
    /// 带截断的动画状态播放
    /// 
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public int StartPlay(string statusName, string stateName)
    {
        //Log.Error(LogLevel.Info, "PlayAnimStateWithCut=======================play {0}-{1}", statusName, stateName);

        Log.Logic(LogLevel.Info, "<color=blue>{0} MatchMeterCrossfadeAnimDriver StartPlay--statusName:{1}, stateName:{2}</color>", mAgent.GetAgentId(), statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "MatchMeterCrossfadeAnimDriver StartPlay Error, statusName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "MatchMeterCrossfadeAnimDriver StartPlay Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        AgentAnimStateInfo newStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        if (newStateInfo == null)
        {
            Log.Error(LogLevel.Normal, "MatchMeterCrossfadeAnimDriver StartPlay Error, status:{0} can not find state:{1}", statusName, stateName);
            return MeterManager.Ins.MeterIndex;
        }

        if (newStateInfo.meterLen == 0)
        {
            Log.Error(LogLevel.Normal, "MatchMeterCrossfadeAnimDriver StartPlay Error, state {0} anim meter len = 0!", newStateInfo.stateName);
            return MeterManager.Ins.MeterIndex;
        }

        Reset();

        PlayOnce(newStateInfo);

        int endMeter = MeterManager.Ins.MeterIndex;
        if(newStateInfo.loopTime == 0) //无限循环
        {
            endMeter = int.MaxValue;
        }
        else //有限循环
        {
            endMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, newStateInfo.meterLen * newStateInfo.loopTime);
        }

        // 动画结束拍
        return endMeter - 1;
    }

    private void PlayOnce(AgentAnimStateInfo animState)
    {
        if (animState == null)
            return;


        // 动画播完一次的逻辑结束拍
        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, animState.meterLen);
        // 当前到动画播完一次的逻辑结束拍的时长
        float duration = MeterManager.Ins.GetTimeToMeter(newMeterIndex);
        // 当前拍的起始位置到动画播完一次的逻辑结束拍的总时长
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);

        Log.Logic("PlayOnce-------anim:{0}---duration:{1}---curMeter:{2}----newMeter:{3}", animState.animName, duration, MeterManager.Ins.MeterIndex, newMeterIndex);
        if (mCurAnimState == null)
        {
            //mAgent.AnimPlayer.PlayState(animState.stateName, animState.animLen, animState.layer, 0, duration);
            mAgent.AnimPlayer.CrossFadeToAnim(animState.animName, animState.layer, animState.normalizedTime, animState.animLen, duration, totalMeterTime);
        }
        else if (mCurAnimState.animName.Equals(animState.animName))
        {
            //mAgent.AnimPlayer.UpdateAnimSpeedWithFix(mCurAnimState.layer, mCurAnimState.animLen, duration);
            mAgent.AnimPlayer.PlayAnim(animState.animName, mCurAnimState.animLen, 0, 0f, duration);
        }
        else
        {
            mAgent.AnimPlayer.CrossFadeToAnim(animState.animName, animState.layer, animState.normalizedTime, animState.animLen, duration, totalMeterTime);
        }

        mCurLoopEndMeter = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, animState.meterLen) - 1;
        mLoopRecord++;
        mCurAnimState = animState;
    }


    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }

    public void OnMeterEnter(int meterIndex)
    {
        if (mCurAnimState == null)
            return;

        if (meterIndex <= mCurLoopEndMeter)
            return;

        if (mCurAnimState != null && mCurAnimState.loopTime > 0 && mLoopRecord >= mCurAnimState.loopTime)
        {
            Reset();
            return;
        }

        PlayOnce(mCurAnimState);
    }

    public void Reset()
    {
        mCurLoopEndMeter = 0;
        mLoopRecord = 0;
        mCurAnimState = null;
    }
}
