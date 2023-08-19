/// <summary>
/// 步进式动画驱动
/// 完全按照配置驱动动画进行
/// 步进式的动画驱动，动画将会按照配置自动循环，即走完一遍配置的动画循环后从头开始
/// </summary>
public class StepLoopAnimDriver : AgentAnimDriver, IMeterHandler
{
    // 所有动画
    protected AgentAnimStateInfo[] mAnimStates;

    //private int curStateIndex;
    //private int curStateLoopRecord;
    //private bool inDriving;

    private int mCurLoopEndMeter;
    private int mLoopRecord;
    private int mCurStateIndex;


    public StepLoopAnimDriver(Agent agt) : base(agt)
    {

    }

    //protected int AnimRepeatePlay()
    //{
    //    if (mCurAnimState == null)
    //    {
    //        Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimRepeatePlay Error, mCurAnimState is null!");
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    float duration = MeterManager.Ins.GetTimeToMeterWithOffset(mCurAnimState.meterLen);
    //    if (duration == 0)
    //    {
    //        Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimRepeatePlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.meterLen);
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.meterLen);
    //    mAgent.AnimPlayer.UpdateAnimSpeedWithFix(mCurAnimState.layer, mCurAnimState.animLen, duration);

    //    // 动画结束拍=当前拍+动画持续拍-1
    //    return newMeterIndex -1;
    //}

    //protected int AnimMoveNextPlay()
    //{
    //    if (mCurAnimState == null)
    //    {
    //        Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, mCurAnimState is null!");
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    string stateName = mCurAnimState.stateName;
    //    if (string.IsNullOrEmpty(stateName))
    //    {
    //        Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, mCurAnimState.stateName is null or empty!");
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    float duration = MeterManager.Ins.GetTimeToMeterWithOffset(mCurAnimState.meterLen);
    //    if (duration == 0)
    //    {
    //        Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.meterLen);
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.meterLen);
    //    float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);
    //    mAgent.AnimPlayer.CrossFadeToState(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, mCurAnimState.animLen, duration, totalMeterTime);

    //    // 动画结束拍=当前拍+动画持续拍-1
    //    return newMeterIndex -1;
    //}

    //public int MoveNext11()
    //{
    //    if (mAnimStates == null)
    //    {
    //        Log.Error(LogLevel.Info, "StepLoopAnimDriver StepAnimDriver MoveNext Error, mAnimStates is null!");
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    mCurAnimState = mAnimStates[curStateIndex];
    //    if (mCurAnimState == null)
    //    {
    //        Log.Error(LogLevel.Info, "StepLoopAnimDriver StepAnimDriver MoveNext, AgentAnimStateInfo is null at index:{0}", curStateIndex);
    //        return MeterManager.Ins.MeterIndex;
    //    }

    //    if(!inDriving)
    //    {
    //        inDriving = true;
    //        return AnimMoveNextPlay();
    //    }

    //    // 规定循环次数设定为0时为无限循环，不再记录循环次数
    //    if (mCurAnimState.loopTime == 0)
    //    {
    //        return AnimRepeatePlay();
    //    }

    //    curStateLoopRecord++;
    //    if (curStateLoopRecord < mCurAnimState.loopTime)
    //    {
    //        return AnimRepeatePlay();
    //    }

    //    // 当前动画状态的循环次数达到目标循环次数时，进入下一个动画状态
    //    curStateLoopRecord = 0;
    //    curStateIndex++;

    //    if (curStateIndex >= mAnimStates.Length)
    //    {
    //        curStateIndex = 0;
    //    }

    //    mCurAnimState = mAnimStates[curStateIndex];
    //    return AnimMoveNextPlay();
    //}

    public void StartPlay(AgentAnimStateInfo[] animStates)
    {
        if (animStates == null)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver StartPlay animStates failed, animStates is null!");
            return;
        }

        if(animStates.Length == 0)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver StartPlay animStates failed, animStates length is 0!");
            return;
        }

        Reset();
        mAnimStates = animStates;

        AgentAnimStateInfo animState = mAnimStates[mCurStateIndex];
        PlayOnce(animState);
    }

    public void StartPlay(string status)
    {
        AgentStatusInfo statusInfo = AgentHelper.GetAgentStatusInfo(mAgent, status);
        if (statusInfo == null)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver StartPlay status failed, agent:{0} do not has status {1}", mAgent.GetName(), status);
            return;
        }
        StartPlay(statusInfo.animStates);
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

        if (mCurAnimState == null)
        {
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


    private AgentAnimStateInfo MoveNext()
    {
        mCurStateIndex = (mCurStateIndex + 1) % mAnimStates.Length;
        return mAnimStates[mCurStateIndex];
    }

    public void OnMeterEnter(int meterIndex)
    {
        if (mCurAnimState == null)
            return;

        if (meterIndex <= mCurLoopEndMeter)
            return;

        if (mCurAnimState != null && mCurAnimState.loopTime > 0 && mLoopRecord >= mCurAnimState.loopTime)
        {
            AgentAnimStateInfo animState = MoveNext();
            PlayOnce(animState);
        }
        else
        {
            PlayOnce(mCurAnimState);
        }
    }

    public void OnMeterEnd(int meterIndex)
    {

    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {

    }


    public override void Dispose()
    {
        base.Dispose();
        Reset();
    }

    public void Reset()
    {
        //curStateIndex = 0;
        //curStateLoopRecord = 0;
        mAnimStates = null;
        mCurAnimState = null;
        //inDriving = false;
        mCurLoopEndMeter = 0;
        mCurStateIndex = 0;
    }


}