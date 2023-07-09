public class CrossfadeByTimeAnimDriver : AgentAnimDriver
{
    public CrossfadeByTimeAnimDriver(Agent agt) : base(agt)
    {
    }


    public void PlayAnim(string statusName, string stateName, float animTime)
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
            mAgent.AnimPlayer.PlayState(stateName, newStateInfo.animLen, newStateInfo.layer, 0, animTime);
            mCurAnimState = newStateInfo;
            return;
        }

        mAgent.AnimPlayer.CrossFadeToStateDynamic(stateName, newStateInfo.layer, newStateInfo.normalizedTime, animTime, newStateInfo.animLen, animTime);
        mCurAnimState = newStateInfo;
    }
}
