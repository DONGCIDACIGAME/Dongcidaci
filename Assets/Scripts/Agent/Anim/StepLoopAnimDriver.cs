/// <summary>
/// 步进式动画驱动
/// 完全按照配置驱动动画进行
/// 步进式的动画驱动，动画将会按照配置自动循环，即走完一遍配置的动画循环后从头开始
/// </summary>
public class StepLoopAnimDriver : AgentAnimDriver
{
    // 所有动画
    protected AgentAnimStateInfo[] mAnimStates;

    private int curStateIndex;
    private int curStateLoopRecord;
    private bool inDriving;
    private string mStatusName;


    public StepLoopAnimDriver(Agent agt, string statusName) : base(agt)
    {
        curStateIndex = 0;
        curStateLoopRecord = 0;
        inDriving = false;
        mStatusName = statusName;
        if (agt != null)
        {
            AgentStatusInfo statusInfo = AgentHelper.GetAgentStatusInfo(agt, statusName);
            if (statusInfo != null)
            {
                mAnimStates = statusInfo.animStates;
            }
        }
    }

    protected int AnimRepeatePlay()
    {
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimRepeatePlay Error, mCurAnimState is null!");
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimRepeatePlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        mAgent.AnimPlayer.UpdateAnimSpeedWithFix(mCurAnimState.layer, mCurAnimState.animLen, duration);

        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex -1;
    }

    protected int AnimMoveNextPlay()
    {
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, mCurAnimState is null!");
            return MeterManager.Ins.MeterIndex;
        }

        string stateName = mCurAnimState.stateName;
        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, mCurAnimState.stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "StepLoopAnimDriver AnimMoveNextPlay Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, newMeterIndex);
        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);

        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex -1;
    }

    public int MoveNext()
    {
        if (mAnimStates == null)
        {
            Log.Error(LogLevel.Info, "StepLoopAnimDriver StepAnimDriver MoveNext Error, mAnimStates is null!");
            return MeterManager.Ins.MeterIndex;
        }

        mCurAnimState = mAnimStates[curStateIndex];
        if (mCurAnimState == null)
        {
            Log.Error(LogLevel.Info, "StepLoopAnimDriver StepAnimDriver MoveNext, AgentAnimStateInfo is null at index:{0}", curStateIndex);
            return MeterManager.Ins.MeterIndex;
        }

        if(!inDriving)
        {
            inDriving = true;
            return AnimMoveNextPlay();
        }

        // 规定循环次数设定为0时为无限循环，不再记录循环次数
        if (mCurAnimState.loopTime == 0)
        {
            return AnimRepeatePlay();
        }

        curStateLoopRecord++;
        if (curStateLoopRecord < mCurAnimState.loopTime)
        {
            return AnimRepeatePlay();
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

    public override void Dispose()
    {
        base.Dispose();
        mAnimStates = null;
        Reset();
    }

    public void Reset()
    {
        curStateIndex = 0;
        curStateLoopRecord = 0;
        mCurAnimState = null;
        inDriving = false;
    }

}