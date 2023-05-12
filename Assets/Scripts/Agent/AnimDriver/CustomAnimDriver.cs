/// <summary>
/// 自定义的动画驱动
/// </summary>
public class CustomAnimDriver : AgentAnimDriver
{
    public CustomAnimDriver(Agent agt, string statusName) : base(agt, statusName)
    {

    }

    public int PlayAnimState(string stateName)
    {
        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "CustomAnimDriver PlayAnimState Error, stateName is null or empty!");
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
            Log.Error(LogLevel.Normal, "CustomAnimDriver PlayAnimState Error, no state named {0}!", stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToBaseMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "CustomAnimDriver PlayAnimState Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);

        // 原来的逻辑，直接融合
        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);
        return endMeterIndex;
    }

}
