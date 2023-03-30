public class AgentStateAnimQueue
{
    private AgentAnimStateInfo[] mAnimStates;
    private int curStateIndex;
    private int curStateLoopRecord;
    private float timeRecord;
    private float lastStateMaxTimeRecord;

    public AgentAnimStateInfo GetCurAnimState()
    {
        if (mAnimStates == null || mAnimStates.Length == 0)
            return null;

        return mAnimStates[curStateIndex];
    }

    public void Initialize(AgentStatusInfo statusInfo)
    {
        if (statusInfo == null)
            return;

        mAnimStates = statusInfo.animStates;
        curStateIndex = 0;
        curStateLoopRecord = 0;
        lastStateMaxTimeRecord = 0;
    }
    
    public bool MoveNext(float deltaTime)
    {
        timeRecord += deltaTime;

        if (mAnimStates == null || mAnimStates.Length == 0)
            return false;

        AgentAnimStateInfo curStateInfo = mAnimStates[curStateIndex];
        if (curStateInfo == null)
        {
            Log.Error(LogLevel.Info, "AgentStateAnimQueue MoveNext, AgentAnimStateInfo is null at index:{0}", curStateIndex);
            return false;
        }

        // 规定 loopTime = 0 为无限循环
        if (curStateInfo.loopTime == 0)
            return false;


        if(timeRecord - lastStateMaxTimeRecord > curStateInfo.stateLen)
        {
            curStateLoopRecord++;
            lastStateMaxTimeRecord += curStateInfo.stateLen;
        }

        if(curStateLoopRecord >= curStateInfo.loopTime)
        {
            curStateLoopRecord = 0;
            curStateIndex++;

            // 规定动画队列也是循环播放的
            if(curStateIndex >= mAnimStates.Length)
            {
                curStateIndex = 0;
            }

            return true;
        }

        return false;
    }


}
