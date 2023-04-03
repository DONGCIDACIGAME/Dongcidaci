using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ��ɫ����������
/// </summary>
public class AgentAnimPlayer
{
    private Animator mAnimator;

    /// <summary>
    /// ���ж�����Ϣ
    /// Լ�����ж������ƺ�״̬���е�״̬���Ʊ���һ��
    /// ������ȡ�����еĶ���ʱ���󼴿�֪����Ӧ��״̬Ĭ�϶���ʱ��
    /// </summary>
    private Dictionary<string, float> mAnimInfoMap;

    /// <summary>
    /// ��ǰ״̬����
    /// </summary>
    public string CurStateName { get; private set; }

    /// <summary>
    /// ��ǰ״̬�Ĳ��Ž���
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
    /// �󶨶���������
    /// </summary>
    /// <param name="animator">����������</param>
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
    /// ���¶��������ٶ�
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

        // ���ܺ�ͬһ��״̬���ж����ں�
        if (CurStateName.Equals(stateName))
            return false;

        CurStateName = stateName;
        UpdateAnimSpeed(animLen / duration);
        return true;
    }

    /// <summary>
    /// �ڹ涨ʱ��(��һ��)���ں���ָ���������������
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="stateLen">״̬ʱ��</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">�����ں���ռʱ��(��һ��)</param>
    /// <param name="duration">�¶�����Ԥ�ڲ���ʱ��</param>
    public void CrossFadeToStateInNormalizedTime(string stateName, float stateLen, int layer, float normalizedTime, float duration)
    {
        // ��һ��ʱ��Ϊ0�����ǲ��ںϣ�ֱ�Ӳ���
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
    /// �ڹ涨ʱ��(����ʱ��)���ں���ָ���������������
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="stateLen">״̬ʱ��</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="time">�����ں���ռʱ��(����ʱ��)</param>
    /// <param name="duration">�¶�����Ԥ�ڲ���ʱ��</param>
    public void CrossFadeToStateInFixedTime(string stateName, float stateLen, int layer, float time, float duration)
    {
        // ʱ��Ϊ0�����ǲ��ںϣ�ֱ�Ӳ���
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
    /// �ڹ涨ʱ���ڲ�����ָ������
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="stateLen">״̬ʱ��</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">������ʼʱ��(��һ��)</param>
    /// <param name="duration">�¶�����Ԥ�ڲ���ʱ��</param>
    public void PlayStateInTime(string stateName, float stateLen, int layer, float normalizedTime, float duration)
    {
        if (duration == 0)
            return;

        if (!BeforeChangeToAnimState(stateName,  stateLen, duration))
            return;

        mAnimator.Play(stateName, layer, normalizedTime);
    }
}
