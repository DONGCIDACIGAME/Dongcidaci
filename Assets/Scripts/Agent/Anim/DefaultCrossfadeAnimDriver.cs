using GameEngine;

public class DefaultCrossfadeAnimDriver : AgentAnimDriver, IGameUpdate
{
    /// <summary>
    /// 当前动画循环结束时间
    /// </summary>
    private float mCurLoopEndTime;

    /// <summary>
    /// 计时器
    /// </summary>
    private float mTimeRecord;

    /// <summary>
    /// 当前动画循环次数
    /// </summary>
    private int mLoopRecord;


    public DefaultCrossfadeAnimDriver(Agent agt) : base(agt)
    {

    }


    public float StartPlay(string statusName, string stateName)
    {
        Log.Logic(LogLevel.Info, "<color=blue>{0} DefaultCrossfadeAnimDriver StartPlay--statusName:{1}, stateName:{2}</color>", mAgent.GetAgentId(), statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay Error, statusName is null or empty!");
            return 0;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay Error, stateName is null or empty!");
            return 0;
        }

        AgentAnimStateInfo newStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        if (newStateInfo == null)
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay Error, status:{0} can not find state:{1}", statusName, stateName);
            return 0;
        }

        if(newStateInfo.animLen == 0)
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay Error, status:{0} anim len = 0!", statusName, stateName);
            return 0;
        }


        if (newStateInfo.meterLen == 0)
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay Error, state {0} anim meter len = 0!", newStateInfo.stateName);
            return 0;
        }

        Reset();

        PlayOnce(newStateInfo);

        float endTime = 0;
        if (newStateInfo.loopTime == 0) //无限循环
        {
            endTime = float.MaxValue;
        }
        else //有限循环
        {
            endTime = newStateInfo.animLen * newStateInfo.loopTime;
        }

        // 动画结束拍
        return endTime;
    }

    public float StartPlay(string stateName, string animName, int layer, int loopTime, int meterLen, float normalizedTime, float animLen)
    {
        if(animLen == 0)
        {
            Log.Error(LogLevel.Normal, "DefaultCrossfadeAnimDriver StartPlay {0} Error, anim len = 0!", animName);
            return 0;
        }


        AgentAnimStateInfo newStateInfo = new AgentAnimStateInfo()
        {
            stateName = stateName,
            animName = animName,
            layer = layer,
            loopTime = loopTime,
            meterLen = meterLen,
            normalizedTime = normalizedTime,
            animLen = animLen,
        };

        Reset();

        PlayOnce(newStateInfo);

        float endTime = 0;
        if (newStateInfo.loopTime == 0) //无限循环
        {
            endTime = float.MaxValue;
        }
        else //有限循环
        {
            endTime = newStateInfo.animLen * newStateInfo.loopTime;
        }

        // 动画结束拍
        return endTime;
    }


    private void PlayOnce(AgentAnimStateInfo animState)
    {
        //Log.Logic("PlayOnce//{0}",animState.stateName);
        if (animState == null)
            return;

        // 当前到动画播完一次的逻辑结束拍的时长
        if (mCurAnimState == null)
        {
            //mAgent.AnimPlayer.PlayState(animState.stateName, animState.animLen, animState.layer, 0, animState.animLen);
            mAgent.AnimPlayer.CrossFadeToState(animState.animName, animState.layer, animState.normalizedTime, animState.animLen);
        }
        else if (mCurAnimState.animName.Equals(animState.animName))
        {
            mAgent.AnimPlayer.PlayAnim(animState.animName, animState.animLen, animState.layer, 0, animState.animLen);
            //mAgent.AnimPlayer.UpdateAnimSpeedWithFix(mCurAnimState.layer, mCurAnimState.animLen, animState.animLen);
        }
        else
        {
            mAgent.AnimPlayer.CrossFadeToState(animState.animName, animState.layer, animState.normalizedTime, animState.animLen);
        }

        mCurLoopEndTime = animState.animLen;
        mTimeRecord = 0;
        mLoopRecord++;
        mCurAnimState = animState;
    }


    public void OnGameUpdate(float deltaTime)
    {
        if (mCurAnimState == null)
            return;

        mTimeRecord += deltaTime;

        if (mTimeRecord <= mCurLoopEndTime)
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
        mCurLoopEndTime = 0;
        mLoopRecord = 0;
        mCurAnimState = null;
    }
}
