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
        mMoveCtl.MoveToPosition(pos);
    }

    /// <summary>
    /// 朝指定方向指定时间移动指定距离
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="distance"></param>
    public virtual void MoveTowards(Vector3 towards, float distance, float duration)
    {
        if (mMoveCtl != null)
            mMoveCtl.MoveTowards(towards.normalized, distance, duration);
    }

    /// <summary>
    /// 移动到目标位置
    /// </summary>
    /// <param name="targetPos"></param>
    public virtual void MoveToPosition(Vector3 targetPos)
    {
        if (mMoveCtl != null)
            mMoveCtl.MoveToPosition(targetPos);
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
