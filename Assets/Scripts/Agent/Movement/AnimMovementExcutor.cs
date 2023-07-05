using GameEngine;

/// <summary>
/// 动画移动执行器
/// </summary>
public class AnimMovementExcutor : IGameUpdate, IRecycle
{
    /// <summary>
    /// 计时器
    /// </summary>
    private float mTimer;

    /// <summary>
    /// 发起方
    /// 是否可以扩大适用范围至entity？
    /// </summary>
    private Agent mAgt;

    /// <summary>
    /// 执行时间
    /// </summary>
    private float mMoveEndTime;

    /// <summary>
    /// 移动距离
    /// </summary>
    private float mMoveDistance;

    /// <summary>
    /// 移动时间
    /// </summary>
    private float mMoveDuration;

    public bool active { get; private set; }

    /// <summary>
    /// 启动一个角色在击打点的效果执行器
    /// </summary>
    /// <param name="agt">谁</param>
    /// <param name="moveEndTime">移动结束时间</param>
    /// <param name="movement">移动数据</param>
    public void Initialize(Agent agt, float moveEndTime, float distance, float duration)
    {
        mTimer = 0;
        mAgt = agt;
        mMoveEndTime = moveEndTime;
        mMoveDistance = distance;
        mMoveDuration = duration;
        active = true;
    }

    public void Dispose()
    {
        active = false;
        mTimer = 0;
        mMoveEndTime = 0;
        mAgt = null;
        mMoveDistance = 0;
        mMoveDuration = 0;
    }

    public void Recycle()
    {
        RecycleReset();
        GamePoolCenter.Ins.MovementExcutorPool.Push(this);
    }

    public void OnUpdate(float deltaTime)
    {
        if (!active)
            return;

        mTimer += deltaTime;

        if (mTimer >= mMoveEndTime-mMoveDuration)
        {
            mAgt.MoveControl.MoveDistanceInTime(mMoveDistance, mMoveDuration);

            Recycle();
        }
    }

    public void RecycleReset()
    {
        Dispose();
    }
}
