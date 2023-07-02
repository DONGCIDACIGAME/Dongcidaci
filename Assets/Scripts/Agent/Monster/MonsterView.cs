public class MonsterView : AgentView
{
    /// <summary>
    /// 是否打印行为树日志
    /// </summary>
    public bool LogAI;
    private bool mLastLogAI;

    public override void OnMyUpdate(Agent agt, float deltaTime)
    {
        base.OnMyUpdate(agt, deltaTime);


        Monster monster = agt as Monster;
        if (mLastLogAI != LogAI)
        {
            monster.BehaviourTree.SetLogEnable(LogAI);
        }

        mLastLogAI = LogAI;
    }
}
