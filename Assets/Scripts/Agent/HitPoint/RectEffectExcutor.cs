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


    public bool Active { get; private set; }

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
        Active = true;
    }

    public void Dispose()
    {
        Active = false;
        mTimer = 0;
        mExcuteTime = 0;
        mAgt = null;
        mEffect = null;
    }

    private void Excute(Agent agt, RectEffect effect)
    {
        Log.Logic(LogLevel.Info, "{0} excute effect {1}", agt.GetAgentId(), effect.effectType);
    }

    public void Recycle()
    {
        Dispose();
        GamePoolCenter.Ins.RectEffectExcutorPool.Push(this);
    }

    public void OnUpdate(float deltaTime)
    {
        if (mExcuteTime == 0)
            return;

        mTimer += deltaTime;

        if(mTimer >= mExcuteTime)
        {
            Excute(mAgt, mEffect);

            Recycle();
        }
    }
}
