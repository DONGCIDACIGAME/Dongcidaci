using GameEngine;

/// <summary>
/// 矩形范围的效果执行器
/// 由外部驱动，如果外部被打断则不会执行效果
/// </summary>
public class RectEffectExcutor : IGameUpdate, IRecycle
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
    /// 效果的执行时间
    /// </summary>
    private float mExcuteTime;

    /// <summary>
    /// 效果
    /// </summary>
    private RectEffect mEffect;

    /// <summary>
    /// 启动一个角色在击打点的效果执行器
    /// </summary>
    /// <param name="agt">谁</param>
    /// <param name="excuteTime">多久之后</param>
    /// <param name="effect">执行什么效果</param>
    public void Initialize(Agent agt, float excuteTime, RectEffect effect)
    {
        mTimer = 0;
        mAgt = agt;
        mExcuteTime = excuteTime;
        mEffect = effect;
    }

    public void Dispose()
    {
        mTimer = 0;
        mExcuteTime = 0;
        mAgt = null;
        mEffect = null;
    }

    private void Excute(Agent agt, RectEffect effect)
    {

    }

    public void Recycle()
    {
        Dispose();
        RectEffectExcutorPool.Ins.PushExcutor(this);
    }

    public void OnUpdate(float deltaTime)
    {
        mTimer += deltaTime;

        if(mTimer >= mExcuteTime)
        {
            Excute(mAgt, mEffect);

            Recycle();
        }
    }
}
