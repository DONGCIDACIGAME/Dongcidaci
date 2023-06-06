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
    /// 移动
    /// </summary>
    private Movement mMovement;

    public bool active { get; private set; }

    /// <summary>
    /// 启动一个角色在击打点的效果执行器
    /// </summary>
    /// <param name="agt">谁</param>
    /// <param name="moveEndTime">移动结束时间</param>
    /// <param name="movement">移动数据</param>
    public void Initialize(Agent agt, float moveEndTime, Movement movement)
    {
        mTimer = 0;
        mAgt = agt;
        mMoveEndTime = moveEndTime;
        mMovement = movement;
        active = true;
    }

    public void Dispose()
    {
        active = false;
        mTimer = 0;
        mMoveEndTime = 0;
        mAgt = null;
        mMovement = null;
    }

    public void Recycle()
    {
        Dispose();
        GamePoolCenter.Ins.MovementExcutorPool.Push(this);
    }

    public void OnUpdate(float deltaTime)
    {
        if (!active)
            return;

        mTimer += deltaTime;

        if (mTimer >= mMoveEndTime-mMovement.duration)
        {
            mAgt.MoveControl.MoveDistanceInTime(mMovement.distance, mMovement.duration);

            Recycle();
        }
    }
}