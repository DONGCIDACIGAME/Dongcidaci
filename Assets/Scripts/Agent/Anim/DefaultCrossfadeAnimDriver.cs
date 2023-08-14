using GameEngine;

public class DefaultCrossfadeAnimDriver : AgentAnimDriver, IGameUpdate
{
    public DefaultCrossfadeAnimDriver(Agent agt) : base(agt)
    {
    }


    public int CrossFadeToState(string statusName, string stateName)
    {
        Log.Logic(LogLevel.Info, "<color=blue>{0} -- PlayAnim--statusName:{1}, stateName:{2}</color>", mAgent.GetAgentId(), statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, statusName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, stateName is null or empty!");
            return MeterManager.Ins.MeterIndex;
        }

        AgentAnimStateInfo newStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        if (newStateInfo == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, status:{0} can not find state:{1}", statusName, stateName);
            return MeterManager.Ins.MeterIndex;
        }

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, newStateInfo.meterLen);

        // 同一个状态的动画无法进行融合
        // 转为从头完整播放
        if (mCurAnimState != null && mCurAnimState.stateName.Equals(stateName))
        {
            mAgent.AnimPlayer.PlayState(stateName, newStateInfo.animLen, newStateInfo.layer, 0, newStateInfo.animLen);
            mCurAnimState = newStateInfo;
            return newMeterIndex-1;
        }

        mAgent.AnimPlayer.CrossFadeToState(stateName, newStateInfo.layer, newStateInfo.normalizedTime, newStateInfo.animLen);
        mCurAnimState = newStateInfo;

        // 动画结束拍=当前拍+动画持续拍-1
        return newMeterIndex - 1;
    }


    public int CrossFadeToState(string stateName, int layer, int loopTime,int meterLen, float normalizedTime, float animLen)
    {
        AgentAnimStateInfo newStateInfo = new AgentAnimStateInfo()
        {
            stateName = stateName,
            layer = layer,
            loopTime = loopTime,
            meterLen = meterLen,
            normalizedTime = normalizedTime,
            animLen = animLen,
        };

        int newMeterIndex = MeterManager.Ins.GetMeterIndex(MeterManager.Ins.MeterIndex, newStateInfo.meterLen);
        // 同一个状态的动画无法进行融合
        // 转为从头完整播放
        if (mCurAnimState != null && mCurAnimState.stateName.Equals(stateName))
        {
            mAgent.AnimPlayer.PlayState(stateName, newStateInfo.animLen, newStateInfo.layer, 0, newStateInfo.animLen);
            mCurAnimState = newStateInfo;
            return newMeterIndex - 1;
        }

        mAgent.AnimPlayer.CrossFadeToState(stateName, newStateInfo.layer, newStateInfo.normalizedTime, newStateInfo.animLen);
        mCurAnimState = newStateInfo;
        return newMeterIndex - 1;
    }

    public void OnGameUpdate(float deltaTime)
    {
       
    }
}
