using UnityEngine;

public abstract class AgentMoveControl
{
    protected Agent mAgent;
    protected MoveControl mMoveCtl;
    protected TurnControl mTurnCtl;

    public AgentMoveControl(Agent agt)
    {
        mAgent = agt;
        mMoveCtl = new MoveControl(agt);
        mTurnCtl = new TurnControl(agt);
    }

    /// <summary>
    /// 移动（由外部驱动）
    /// 由外部驱动的好处是可以根据状态来控制，只有需要移动的状态才驱动进行移动
    /// </summary>
    /// <param name="deltaTime">移动间隔时间</param>
    public virtual void Move(float deltaTime)
    {
        // 控制移动
        Vector3 pos = mAgent.GetPosition() + mAgent.GetTowards().normalized * mAgent.GetSpeed() * deltaTime;
        mAgent.SetPosition(pos);
    }

    public void Dash(float distance, float duration)
    {
        Vector3 towards = GamePlayDefine.InputDirection_NONE;
        // 获取转向控件的目标转向
        if (mTurnCtl != null)
            towards = mTurnCtl.GetTowards();

        // 如果没有目标转向，则使用角色当前朝向
        if (towards.Equals(GamePlayDefine.InputDirection_NONE))
            towards = mAgent.GetTowards();

        if (mMoveCtl != null)
            mMoveCtl.MoveTo(towards, distance, duration);
    }

    /// <summary>
    /// 转向
    /// </summary>
    /// <param name="towards">目标朝向</param>
    public void TurnTo(Vector3 towards)
    {
        if (mTurnCtl != null)
            mTurnCtl.TurnTo(towards);
    }


    public virtual void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        if (mMoveCtl != null)
            mMoveCtl.OnUpdate(deltaTime);

        if (mTurnCtl != null)
            mTurnCtl.OnUpdate(deltaTime);
    }
}
