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
    /// 更新动画播放速度
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="duration"></param>
    public void UpdateAnimSpeed(float speed)
    {
        if(mAnimator == null)
        {
            Log.Error(LogLevel.Info, "UpdateAnimSpeed Error, animator is null!");
            return;
        }
  
        mAnimator.speed = speed;
        Log.Logic(LogLevel.Info, "UpdateAnimSpeed----speed:{0}", speed);
    }

    private bool BeforeChangeToAnimState(string stateName, float animLen, float duration)
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
        UpdateAnimSpeed(animLen / duration);
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

        if (!BeforeChangeToAnimState(stateName, stateLen, duration))
            return;

        mAnimator.CrossFade(stateName, normalizedTime, layer);
    }


    /// <summary>
    /// 在规定时间(绝对时间)内融合至指定动画并播放完成
    /// </summary>
    /// <param name="stateName">状态名称</param>
    /// <param name="stateLen">状态时间</param>
    /// <param name="layer">动画所在层级</param>
    /// <param name="time">动画融合所占时间(绝对时间)</param>
    /// <param name="duration">新动画的预期播放时间</param>
    public void CrossFadeToStateInFixedTime(string stateName, float stateLen, int layer, float time, float duration)
    {
        // 时间为0，就是不融合，直接播放
        if (time == 0)
        {
            PlayStateInTime(stateName, stateLen, layer, 0, duration);
            return;
        }

        if (!BeforeChangeToAnimState(stateName, stateLen, duration))
            return;
        mAnimator.CrossFadeInFixedTime(stateName, time, layer);
    }


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

        if (!BeforeChangeToAnimState(stateName,  stateLen, duration))
            return;

        mAnimator.Play(stateName, layer, normalizedTime);
    }
}
