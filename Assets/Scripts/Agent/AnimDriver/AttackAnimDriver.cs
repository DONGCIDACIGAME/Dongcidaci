/// <summary>
/// �Զ���Ķ�������
/// </summary>
public class AttackAnimDriver : AgentAnimDriver
{
    public AttackAnimDriver(Agent agt, string statusName) : base(agt, statusName)
    {

    }

    public void Dispose()
    {
        mAnimStates = null;
        mCurAnimState = null;
    }

    public int PlayAnimState(string stateName)
    {
        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnimState Error, stateName is null or empty!");
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
            Log.Error(LogLevel.Normal, "PlayAnimState Error, no state named {0}!", stateName);
            return MeterManager.Ins.MeterIndex;
        }

        float duration = MeterManager.Ins.GetTimeToBaseMeter(mCurAnimState.stateMeterLen);
        if (duration == 0)
        {
            Log.Error(LogLevel.Normal, "PlayAnimState Error, time to target meter is 0,anim state len:{0}", mCurAnimState.stateMeterLen);
            return MeterManager.Ins.MeterIndex;
        }

        int endMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, mCurAnimState.stateMeterLen);
        float totalMeterTime = MeterManager.Ins.GetTotalMeterTime(MeterManager.Ins.MeterIndex, endMeterIndex);

        // ԭ�����߼���ֱ���ں�
        //mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);

        // ���߼�
        // ����������ڽ��ĵ�ǰ50%���Ϳ���һ�ģ�ֱ�Ӳ��ţ����ض�
        // ����������ڽ��ĵĺ�50%���Ϳ������ģ�ֱ�Ӳ��ţ������ض�
        float progress = duration / totalMeterTime;
        if(progress <= 0.5f)
        {
            mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime, duration, mCurAnimState.animLen, totalMeterTime);
            return endMeterIndex;
        }
        else
        {
            float newDuration = MeterManager.Ins.GetTimeToBaseMeter(mCurAnimState.stateMeterLen+1);
            mAgent.AnimPlayer.CrossFadeToStateStatic(stateName, mCurAnimState.layer, mCurAnimState.normalizedTime * (1 + progress), 0, newDuration, mCurAnimState.animLen);
            return endMeterIndex+1;
        }
    }

}