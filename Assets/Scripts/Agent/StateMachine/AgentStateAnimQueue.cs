using GameEngine;
public class AgentStateAnimQueue
{
    private AgentAnimStateInfo[] mAnimStates;
    private int curStateIndex;
    private int curStateLoopRecord;

    public void Initialize()
    {

    }

    public void Dispose()
    {
        Reset();
        mAnimStates = null;
    }

    public void Reset()
    {
        curStateIndex = 0;
        curStateLoopRecord = 0;
    }

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
    }
    
    public int MoveNext()
    {
        if (mAnimStates == null)
            return AgentAnimDefine.AnimQueue_Error;

        AgentAnimStateInfo curStateInfo = mAnimStates[curStateIndex];
        if (curStateInfo == null)
        {
            Log.Error(LogLevel.Info, "AgentStateAnimQueue MoveNext, AgentAnimStateInfo is null at index:{0}", curStateIndex);
            return AgentAnimDefine.AnimQueue_Error;
        }

        if (curStateInfo.loopTime > 0)
        {
            curStateLoopRecord++;
        }

        // 规定循环次数设定为0时为无限循环
        if(curStateInfo.loopTime == 0)
        {
            return AgentAnimDefine.AnimQueue_AnimKeep;
        }

        if (curStateLoopRecord < curStateInfo.loopTime)
        {
            return AgentAnimDefine.AnimQueue_AnimKeep;
        }

        // 当前动画状态的循环次数达到目标循环次数时，进入下一个动画状态
        curStateLoopRecord = 0;
        curStateIndex++;


        if(curStateIndex >= mAnimStates.Length)
        {
            curStateIndex = 0;
        }
        return AgentAnimDefine.AnimQueue_AnimMoveNext;
    }


}
