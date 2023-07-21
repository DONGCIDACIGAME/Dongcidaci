using GameEngine;
using UnityEngine;

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
    /// 移动开始时间
    /// </summary>
    private float mMoveStartTime;

    /// <summary>
    /// 移动结束时间
    /// </summary>
    private float mMoveEndTime;

    /// <summary>
    /// 移动距离
    /// </summary>
    private float mMoveDistance;

    /// <summary>
    /// 移动方向类型
    /// </summary>
    private int mMoveTowardsType;

    /// <summary>
    /// 移动方向
    /// </summary>
    private Vector3 mMoveTowards;

    /// <summary>
    /// 移动速度
    /// </summary>
    private float mMoveStep;

    public bool active { get; private set; }

    /// <summary>
    /// 启动一个角色在击打点的效果执行器
    /// </summary>
    /// <param name="agt">谁</param>
    /// <param name="moveStartTime">从当前帧开始计时，多久后开始移动</param>
    /// <param name="moveEndTime">从当前帧开始计时，多久后结束移动</param>
    /// <param name="towardsType">朝向类型 0：实时朝向 1：固定朝向</param>
    /// <param name="towards">朝哪个方向/param>
    /// <param name="distance">移动多远</param>
    public void Initialize(Agent agt, float moveStartTime, float moveEndTime, int towardsType, Vector3 towards, float distance)
    {
        // 如果移动距离<=0
        if (mMoveDistance <= 0)
        {
            Recycle();
        }

        // 如果移动时间<=0
        if (mMoveEndTime - mMoveStartTime <= 0)
        {
            Vector3 targetPos = agt.GetPosition() + mMoveTowards.normalized * mMoveDistance;
            agt.MoveControl.MoveToPosition(targetPos);
            Recycle();
        }


        mTimer = 0;
        mAgt = agt;
        mMoveStartTime = moveStartTime;
        mMoveEndTime = moveEndTime;
        mMoveDistance = distance;
        mMoveTowardsType = towardsType;
        mMoveTowards = towards.normalized;
        mMoveStep = mMoveDistance / (moveEndTime - moveStartTime);
        active = true;
    }

    public void Dispose()
    {
        active = false;
        mTimer = 0;
        mMoveEndTime = 0;
        mAgt = null;
        mMoveDistance = 0;
        mMoveStartTime = 0;
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

        Vector3 towards = DirectionDef.none;
        if(mTimer >= mMoveStartTime && mTimer < mMoveEndTime)
        {
            if (mMoveTowardsType.Equals(DirectionDef.RealTowards))
            {
                towards = mAgt.GetTowards();
            }
            else if (mMoveTowardsType.Equals(DirectionDef.FixedTowards))
            {
                towards = mMoveTowards;
            }

            Vector3 curPos = mAgt.GetPosition();
            Vector3 targetPos = curPos + towards * mMoveStep * deltaTime;
            mAgt.MoveControl.MoveToPosition(targetPos);
        }
        else if(mTimer > mMoveEndTime)
        {
            Recycle();
        }
    }

    public void RecycleReset()
    {
        Dispose();
    }
}
