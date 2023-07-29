using GameEngine;
using GameSkillEffect;

/// <summary>
/// 每一个 combo hit点的效果执行器
/// 由外部驱动，如果外部被打断则不会执行效果
/// </summary>
public class ComboEffectExcutor : IGameUpdate, IRecycle
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
    /// 描述某一个combo在hit点打出的效果
    /// Modified by Weng 0708
    /// </summary>
    private ComboHitEffectsData mEffects;


    public bool active { get; private set; }

    /// <summary>
    /// 启动一个角色在击打点的效果执行器
    /// </summary>
    /// <param name="agt">谁</param>
    /// <param name="excuteTime">多久之后</param>
    /// <param name="effect">执行什么效果</param>
    public void Initialize(Agent agt, float excuteTime, ComboHitEffectsData hitEffects)
    {
        mTimer = 0;
        mAgt = agt;
        mExcuteTime = excuteTime;
        mEffects = hitEffects;
        active = true;
    }

    public void Dispose()
    {
        active = false;
        mTimer = 0;
        mExcuteTime = 0;
        mAgt = null;
        mEffects = null;
    }

    private void Excute(Agent agt, ComboHitEffectsData hitEffects)
    {
        //Log.Logic(LogLevel.Info, "{0} excute effect {1}", agt.GetAgentId(), effect.effectType);
        // Added by weng 0703
        agt.SkillEftHandler.OnExcHitPointComboEffects(hitEffects);
    }

    public void Recycle()
    {
        RecycleReset();
        GamePoolCenter.Ins.ComboEffectExcutorPool.Push(this);
    }

    public void OnGameUpdate(float deltaTime)
    {
        if (!active)
            return;

        if (mExcuteTime == 0)
            return;

        mTimer += deltaTime;

        if(mTimer >= mExcuteTime)
        {
            Excute(mAgt, mEffects);

            Recycle();
        }
    }

    public void RecycleReset()
    {
        Dispose();
    }


}
