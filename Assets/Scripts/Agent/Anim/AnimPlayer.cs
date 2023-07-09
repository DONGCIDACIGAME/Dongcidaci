using UnityEngine;

/// <summary>
///  角色动画播放器
/// </summary>
public class AnimPlayer
{
    public Animator mAnimator;

    /// <summary>
    /// 当前状态名称
    /// </summary>
    public string CurStateName { get; private set; }

    /// <summary>
    /// 当前状态的播放进度
    /// </summary>
    public float CurStateProgress
    {
        get
        {
            return mAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }
    }

    /// <summary>
    /// 上个状态的实际播放时间
    /// TODO：是否要放在这里
    /// </summary>
    private float mLastStateLen;



    public AnimPlayer()
    {
        CurStateName = string.Empty;
    }

    public void Dispose()
    {
        mAnimator = null;
        mLastStateLen = 0;
    }

    /// <summary>
    /// 绑定动画控制器
    /// </summary>
    /// <param name="animator">动画控制器</param>
    public void Initialize(Animator animator)
    {
        if(animator == null)
        {
            Log.Error(LogLevel.Critical, "BindAnimator Failed, animator is null!");
            return;
        }
        mAnimator = animator;
    }

    public bool IsState(int layer, string stateName)
    {
        if (mAnimator == null)
            return false;

        AnimatorStateInfo state = mAnimator.GetCurrentAnimatorStateInfo(layer);
        return state.IsName(stateName);
    }

    /// <summary>
    /// 更新动画播放速度（带补偿）
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="stateLen"></param>
    /// <param name="duration"></param>
    public void UpdateAnimSpeedWithFix(int layer, float stateLen, float duration)
    {
        if (duration == 0)
            return;

        if (mAnimator == null)
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeedWithFix Error, animator is null!");
            return;
        }

        float timeOffset = 0;
        // TODO: 这里的逻辑应该是有问题的，不能根据两个魔法数去判定
        // 根据上次的播放进度对动画速度进行补偿
        // 如果是 x.9xx 这种播放进度 说明上次还没有播完，需要把未播放的时间扣除掉
        // 如果是 x.0xx这种播放进度 说明上次播放超了一部分，需要补上一些不放时间
        // 超出这两个的范围，说明这次播放遇到了问题，导致播放进度出现了较大的偏差，需要输出错误日志以便查看

        AnimatorStateInfo stateInfo = mAnimator.GetCurrentAnimatorStateInfo(layer);
        float progressOffset = stateInfo.normalizedTime % 1;

        if (progressOffset < 0.2f)
        { 
            timeOffset = progressOffset * mLastStateLen;
        }
        else if (progressOffset > 0.8f)
        {
            timeOffset = (progressOffset - 1) * mLastStateLen;
        }
        else
        {
            // 出现较大偏差，强制将动画定格至最后一帧
            mAnimator.Play(stateInfo.fullPathHash, layer, 1);
            Log.Error(LogLevel.Info, "UpdateAnimSpeedWithFix Error, 播放进度出现较大偏差，请查看！progressOffset:{0}, anim state:{1}", progressOffset, stateInfo.fullPathHash);
        }

        mAnimator.speed = stateLen / (duration + timeOffset);
        Log.Logic(LogLevel.Info, "UpdateAnimSpeed----speed:{0}, progressOffset:{1}, timeOffset:{2},mCurStateRealTime:{3}, duration:{4}",
            mAnimator.speed, progressOffset, timeOffset, mLastStateLen, duration);
        mLastStateLen = duration;
    }

    /// <summary>
    /// 更新动画播放速度
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="stateLen"></param>
    /// <param name="duration"></param>
    public void UpdateAnimSpeed(float speed)
    {
        if(speed < 0)
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeed Error, anim speed can not be negtive!");
            return;
        }

        if (mAnimator == null)
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeed Error, animator is null!");
            return;
        }

        mAnimator.speed = speed;
    }


    /// <summary>
    /// 在规定时间(归一化)内融合至指定动画并播放完成
    /// 该方法的融合后动画的起始帧是动态的
    /// </summary>
    /// <param name="stateName">状态名称</param>
    /// <param name="layer">动画所在层级</param>
    /// <param name="normalizedTime">动画融合所占时间(归一化)</param>
    /// <param name="targetDuration">新动画的预期播放时间</param>
    /// <param name="animLen">动画的原始时长（播放速度为1时）</param>
    /// <param name="targetFullTime">动画完全播完所需要的预期时长（即假设融合后从第0帧开始播放）</param>
    public void CrossFadeToStateDynamic(string stateName, int layer, float normalizedTime, float targetDuration, float animLen, float targetFullTime)
    {
        if (targetFullTime <= 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToStateDynamic Error, targetFullTime must be greater than 0!");
            return;
        }

        if (targetDuration <= 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToStateDynamic Error, targetDuration must be greater than 0!");
            return;
        }

        // 不能和同一个状态进行动画融合
        if (CurStateName.Equals(stateName))
            return;

        CurStateName = stateName;
        // 归一化时间为0，就是不融合，直接播放
        if (normalizedTime == 0)
        {
            PlayState(stateName, animLen, layer, 0, targetDuration);
            return;
        }

        // 按照配置文件中的节拍时长设置动画速度
        float animSpeed = animLen / targetFullTime;

        // 更新动画播放速度
        UpdateAnimSpeed(animSpeed);

        /*
         *  假如是一个2拍的动画
         *  1.原始动画时长
         * |------------------------------|------------------------------|
         * 
         *  2.按照节拍播完的时长
         * |-----------------|-----------------|
         * 
         * 3.游戏进程  0:开始融合  >:融合过程
         * |-----------------|-----------------|-----------------|-----------------|-----------------|-----------------|
         * |-------0>>-----|-----------------|
         *  A动画     B动画
         *                  |-----------------------| inTime（想要动画播放的时长）
         *                  |-----------------|-----------------|totalMeterLen（B动画原始时长）
         *                                                    |-----------|timeOffset（把B动画向前移动的时长）
         *                                                    
         * 由1，2可以计算出一个动画播放的速度，按照这个速度播放可以保证动画在游戏的两拍内播完
         * 由3可以计算出动画的偏移时长，向前偏移后可以保证动画在节拍处播放完成
         * 动画的融合时间是一个独立值，只是融合过程需要花多久，上面两步是保证卡点的关键计算
         */

        // 计算新动画的偏移时长（即按照上面的播放速度去播放动画又需要在结束拍卡点时，新动画的融合起点时间需要后移）
        float timeOffset = targetFullTime - targetDuration;
        mAnimator.CrossFadeInFixedTime(stateName, normalizedTime * targetDuration, layer, timeOffset);
    }


    /// <summary>
    /// 在规定时间内播放完指定动画
    /// </summary>
    /// <param name="stateName">状态名称</param>
    /// <param name="animLen">动画的原始时长（播放速度为1时）</param>
    /// <param name="layer">动画所在层级</param>
    /// <param name="normalizedTime">动画开始时间(归一化)</param>
    /// <param name="targetDuration">新动画的预期播放时间</param>
    public void PlayState(string stateName, float animLen, int layer, float normalizedTime, float targetDuration)
    {
        if (targetDuration <= 0)
        {
            Log.Error(LogLevel.Normal, "PlayStateInTime Error,  mush greater than 0!");
            return;
        }

        CurStateName = stateName;
        // 按照播放进度计算，剩余动画的播放完成的原始动画时间
        float animTime = animLen * (1-normalizedTime);
        // 计算动画播放速度
        float speed = animTime / targetDuration;
        // 更新动画播放速度
        UpdateAnimSpeed(speed);

        mAnimator.Play(stateName, layer, normalizedTime);
    }
}
