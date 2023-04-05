using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  角色动画播放器
/// </summary>
public class AgentAnimPlayer
{
    private Animator mAnimator;

    /// <summary>
    /// 所有动画信息
    /// 约定所有动画名称和状态机中的状态名称保持一致
    /// 这样读取到所有的动画时长后即可知道对应的状态默认动画时长
    /// </summary>
    private Dictionary<string, float> mAnimInfoMap;

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

    private float mCurStateDefaultTime;
    private float mCurStateRealTime;



    public AgentAnimPlayer()
    {
        mAnimInfoMap = new Dictionary<string, float>();
        CurStateName = string.Empty;
    }

    /// <summary>
    /// 绑定动画控制器
    /// </summary>
    /// <param name="animator">动画控制器</param>
    public void BindAnimator(Animator animator)
    {
        if(animator == null)
        {
            Log.Error(LogLevel.Critical, "BindAnimator Failed, animator is null!");
            return;
        }
        mAnimator = animator;

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        if(clips != null)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                AnimationClip clip = clips[i];
                string animName = clip.name;
                float animLen = clip.length;

                if(!mAnimInfoMap.ContainsKey(animName))
                {
                    mAnimInfoMap.Add(animName, animLen);
                }
            }
        }
    }

    public bool IsAnim(int layer, string animName)
    {
        if (mAnimator == null)
            return false;

        AnimatorStateInfo state = mAnimator.GetCurrentAnimatorStateInfo(layer);
        return state.IsName(animName);
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
        // 根据上次的播放进度对动画速度进行补偿
        // 如果是 x.9xx 这种播放进度 说明上次还没有播完，需要把未播放的时间扣除掉
        // 如果是 x.0xx这种播放进度 说明上次播放超了一部分，需要补上一些不放时间
        // 超出这两个的范围，说明这次播放遇到了问题，导致播放进度出现了较大的偏差，需要输出错误日志以便查看
        float progressOffset = mAnimator.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1;

        if (progressOffset < 0.1f)
        {
            timeOffset = progressOffset * mCurStateRealTime;
        }
        else if (progressOffset > 0.9f)
        {
            timeOffset = (progressOffset - 1) * mCurStateRealTime;
        }
        else
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeedWithFix Error, 播放进度出现较大偏差，请查看！");
        }

        mAnimator.speed = stateLen / (duration + timeOffset);
        //mAnimator.speed = stateLen / duration;
        Log.Logic(LogLevel.Info, "UpdateAnimSpeed----speed:{0}, progressOffset:{1}, timeOffset:{2},mCurStateRealTime:{3}, duration:{4}",
            mAnimator.speed, progressOffset, timeOffset, mCurStateRealTime, duration);

        mCurStateDefaultTime = stateLen;
        mCurStateRealTime = duration;
    }

    /// <summary>
    /// 更新动画播放速度
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="stateLen"></param>
    /// <param name="duration"></param>
    public void UpdateAnimSpeed(float stateLen, float duration)
    {
        if (duration == 0)
            return;

        if (mAnimator == null)
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeed Error, animator is null!");
            return;
        }

        mAnimator.speed = stateLen / duration;
        mCurStateDefaultTime = stateLen;
        mCurStateRealTime = duration;
    }

    private bool BeforeChangeToAnimState(string stateName, int layer, float stateLen, float duration)
    {
        if (duration <= 0)
            return false;

        if (mAnimator == null)
        {
            Log.Error(LogLevel.Critical, "CrossFadeAnimInTime Error, mAnimator is null!");
            return false;
        }

        if (string.IsNullOrEmpty(stateName))
        {
            Log.Error(LogLevel.Critical, "CrossFadeToStateInTime Error, target state name is null or empty!");
            return false;
        }

        // 不能和同一个状态进行动画融合
        if (CurStateName.Equals(stateName))
            return false;

        CurStateName = stateName;
        UpdateAnimSpeed(stateLen, duration);
        return true;
    }

    /// <summary>
    /// 在规定时间(归一化)内融合至指定动画并播放完成
    /// </summary>
    /// <param name="stateName">状态名称</param>
    /// <param name="stateLen">状态时间</param>
    /// <param name="layer">动画所在层级</param>
    /// <param name="normalizedTime">动画融合所占时间(归一化)</param>
    /// <param name="duration">新动画的预期播放时间</param>
    public void CrossFadeToStateInNormalizedTime(string stateName, float stateLen, int layer, float normalizedTime, float duration)
    {
        // 归一化时间为0，就是不融合，直接播放
        if (normalizedTime == 0)
        {
            PlayStateInTime(stateName, stateLen, layer, 0, duration);
            return;
        }

        if (!BeforeChangeToAnimState(stateName, layer, stateLen, duration))
            return;

        mAnimator.CrossFade(stateName, normalizedTime, layer);
    }


    ///// <summary>
    ///// 在规定时间(绝对时间)内融合至指定动画并播放完成
    ///// </summary>
    ///// <param name="stateName">状态名称</param>
    ///// <param name="stateLen">状态时间</param>
    ///// <param name="layer">动画所在层级</param>
    ///// <param name="time">动画融合所占时间(绝对时间)</param>
    ///// <param name="duration">新动画的预期播放时间</param>
    //public void CrossFadeToStateInFixedTime(string stateName, float stateLen, int layer, float time, float duration)
    //{
    //    // 时间为0，就是不融合，直接播放
    //    if (time == 0)
    //    {
    //        PlayStateInTime(stateName, stateLen, layer, 0, duration);
    //        return;
    //    }

    //    if (!BeforeChangeToAnimState(stateName, layer, stateLen, duration))
    //        return;
    //    mAnimator.CrossFadeInFixedTime(stateName, time, layer);
    //}


    /// <summary>
    /// 在规定时间内播放完指定动画
    /// </summary>
    /// <param name="stateName">状态名称</param>
    /// <param name="stateLen">状态时间</param>
    /// <param name="layer">动画所在层级</param>
    /// <param name="normalizedTime">动画开始时间(归一化)</param>
    /// <param name="duration">新动画的预期播放时间</param>
    public void PlayStateInTime(string stateName, float stateLen, int layer, float normalizedTime, float duration)
    {
        if (duration == 0)
            return;

        if (!BeforeChangeToAnimState(stateName, layer, stateLen*(1-normalizedTime), duration))
            return;

        mAnimator.Play(stateName, layer, normalizedTime);
    }
}
