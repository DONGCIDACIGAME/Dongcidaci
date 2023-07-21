public class DefaultCrossfadeAnimDriver : AgentAnimDriver
{
    public DefaultCrossfadeAnimDriver(Agent agt) : base(agt)
    {
    }


    public void CrossFadeToState(string statusName, string stateName)
    {
        Log.Logic(LogLevel.Info, "<color=blue>{0} -- PlayAnim--statusName:{1}, stateName:{2}</color>", mAgent.GetAgentId(), statusName, stateName);

        if (string.IsNullOrEmpty(statusName))
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, statusName is null or empty!");
            return;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, stateName is null or empty!");
            return;
        }

        AgentAnimStateInfo newStateInfo = AgentHelper.GetAgentAnimStateInfo(mAgent, statusName, stateName);
        if (newStateInfo == null)
        {
            Log.Error(LogLevel.Normal, "PlayAnim Error, status:{0} can not find state:{1}", statusName, stateName);
            return;
        }

        // 同一个状态的动画无法进行融合
        // 转为从头完整播放
        if (mCurAnimState != null && mCurAnimState.stateName.Equals(stateName))
        {
            mAgent.AnimPlayer.PlayState(stateName, newStateInfo.animLen, newStateInfo.layer, 0, newStateInfo.animLen);
            mCurAnimState = newStateInfo;
            return;
        }

        mAgent.AnimPlayer.CrossFadeToState(stateName, newStateInfo.layer, newStateInfo.normalizedTime, newStateInfo.animLen);
        mCurAnimState = newStateInfo;
    }
}
