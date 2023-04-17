using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ��ɫ����������
/// </summary>
public class AgentAnimPlayer
{
    public Animator mAnimator;

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

    /// <summary>
    /// �ϸ�״̬��ʵ�ʲ���ʱ��
    /// TODO���Ƿ�Ҫ��������
    /// </summary>
    private float mLastStateLen;



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
    /// ���¶��������ٶȣ���������
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
        // TODO: ������߼�Ӧ����������ģ����ܸ�������ħ����ȥ�ж�
        // �����ϴεĲ��Ž��ȶԶ����ٶȽ��в���
        // ����� x.9xx ���ֲ��Ž��� ˵���ϴλ�û�в��꣬��Ҫ��δ���ŵ�ʱ��۳���
        // ����� x.0xx���ֲ��Ž��� ˵���ϴβ��ų���һ���֣���Ҫ����һЩ����ʱ��
        // �����������ķ�Χ��˵����β������������⣬���²��Ž��ȳ����˽ϴ��ƫ���Ҫ���������־�Ա�鿴

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
            // ���ֽϴ�ƫ�ǿ�ƽ��������������һ֡
            mAnimator.Play(stateInfo.fullPathHash, layer, 1);
            Log.Error(LogLevel.Info, "UpdateAnimSpeedWithFix Error, ���Ž��ȳ��ֽϴ�ƫ���鿴��progressOffset:{0}, anim state:{1}", progressOffset, stateInfo.ToString());
        }

        mAnimator.speed = stateLen / (duration + timeOffset);
        Log.Logic(LogLevel.Info, "UpdateAnimSpeed----speed:{0}, progressOffset:{1}, timeOffset:{2},mCurStateRealTime:{3}, duration:{4}",
            mAnimator.speed, progressOffset, timeOffset, mLastStateLen, duration);
        mLastStateLen = duration;
    }

    /// <summary>
    /// ���¶��������ٶ�
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
    /// �ڹ涨ʱ��(��һ��)���ں���ָ���������������
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">�����ں���ռʱ��(��һ��)</param>
    /// <param name="targetDuration">�¶�����Ԥ�ڲ���ʱ��</param>
    /// <param name="animLen">������ԭʼʱ���������ٶ�Ϊ1ʱ��</param>
    /// <param name="totalMeterLen">���Ŷ����Ľ�����ʱ��</param>
    public void CrossFadeToState(string stateName, int layer, float normalizedTime, float targetDuration, float animLen, float totalMeterLen)
    {
        if (totalMeterLen <= 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, total meter len must be greater than 0!");
            return;
        }

        if (targetDuration <= 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToState Error, targetDuration must be greater than 0!");
            return;
        }

        // ���ܺ�ͬһ��״̬���ж����ں�
        if (CurStateName.Equals(stateName))
            return;

        // ��һ��ʱ��Ϊ0�����ǲ��ںϣ�ֱ�Ӳ���
        if (normalizedTime == 0)
        {
            PlayState(stateName, animLen, layer, 0, targetDuration);
            return;
        }

        // ���������ļ��еĽ���ʱ�����ö����ٶ�
        float animSpeed = animLen / totalMeterLen;

        // ���¶��������ٶ�
        UpdateAnimSpeed(animSpeed);

        /*
         *  ������һ��2�ĵĶ���
         *  1.ԭʼ����ʱ��
         * |------------------------------|------------------------------|
         * 
         *  2.���ս��Ĳ����ʱ��
         * |-----------------|-----------------|
         * 
         * 3.��Ϸ����  0:��ʼ�ں�  >:�ںϹ���
         * |-----------------|-----------------|-----------------|-----------------|-----------------|-----------------|
         * |-------0>>-----|-----------------|
         *  A����     B����
         *                  |-----------------------| inTime����Ҫ�������ŵ�ʱ����
         *                  |-----------------|-----------------|totalMeterLen��B����ԭʼʱ����
         *                                                    |-----------|timeOffset����B������ǰ�ƶ���ʱ����
         *                                                    
         * ��1��2���Լ����һ���������ŵ��ٶȣ���������ٶȲ��ſ��Ա�֤��������Ϸ�������ڲ���
         * ��3���Լ����������ƫ��ʱ������ǰƫ�ƺ���Ա�֤�����ڽ��Ĵ��������
         * �������ں�ʱ����һ������ֵ��ֻ���ںϹ�����Ҫ����ã����������Ǳ�֤����Ĺؼ�����
         */

        // �����¶�����ƫ��ʱ��������������Ĳ����ٶ�ȥ���Ŷ�������Ҫ�ڽ����Ŀ���ʱ���¶������ں����ʱ����Ҫ���ƣ�
        float timeOffset = totalMeterLen - targetDuration;
        mAnimator.CrossFadeInFixedTime(stateName, normalizedTime * targetDuration, layer, timeOffset);


    }


    /// <summary>
    /// �ڹ涨ʱ���ڲ�����ָ������
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="animLen">������ԭʼʱ���������ٶ�Ϊ1ʱ��</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">������ʼʱ��(��һ��)</param>
    /// <param name="targetDuration">�¶�����Ԥ�ڲ���ʱ��</param>
    public void PlayState(string stateName, float animLen, int layer, float normalizedTime, float targetDuration)
    {
        if (targetDuration <= 0)
        {
            Log.Error(LogLevel.Normal, "PlayStateInTime Error,  mush greater than 0!");
            return;
        }

        // ���ղ��Ž��ȼ��㣬ʣ�ද���Ĳ�����ɵ�ԭʼ����ʱ��
        float animTime = animLen * (1-normalizedTime);
        // ���㶯�������ٶ�
        float speed = animTime / targetDuration;
        // ���¶��������ٶ�
        UpdateAnimSpeed(speed);

        mAnimator.Play(stateName, layer, normalizedTime);
    }
}
