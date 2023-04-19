using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ��ɫ����������
/// </summary>
public class AgentAnimPlayer
{
    public Animator mAnimator;

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
    }

    public bool IsState(int layer, string stateName)
    {
        if (mAnimator == null)
            return false;

        AnimatorStateInfo state = mAnimator.GetCurrentAnimatorStateInfo(layer);
        return state.IsName(stateName);
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
    /// �÷������ںϺ󶯻�����ʼ֡�̶���
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">�����ں���ռʱ��(��һ��)</param>
    /// <param name="newStateStartProgress">��״̬����ʼ֡�ڶ����еı���</param>
    /// <param name="targetDuration">�¶�����Ԥ�ڲ���ʱ��</param>
    /// <param name="animLen">������ԭʼʱ���������ٶ�Ϊ1ʱ��</param>
    public void CrossFadeToStateStatic(string stateName, int layer, float normalizedTime, float newStateStartProgress, float targetDuration, float animLen)
    {
        if (targetDuration <= 0)
        {
            Log.Error(LogLevel.Normal, "CrossFadeToStateStatic Error, targetDuration must be greater than 0!");
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
        float animSpeed = animLen / targetDuration;

        // ���¶��������ٶ�
        UpdateAnimSpeed(animSpeed);

        // �����¶�����ƫ��ʱ��
        float timeOffset =  targetDuration * newStateStartProgress;
        mAnimator.CrossFadeInFixedTime(stateName, normalizedTime * targetDuration, layer, timeOffset);
    }


    /// <summary>
    /// �ڹ涨ʱ��(��һ��)���ں���ָ���������������
    /// �÷������ںϺ󶯻�����ʼ֡�Ƕ�̬��
    /// </summary>
    /// <param name="stateName">״̬����</param>
    /// <param name="layer">�������ڲ㼶</param>
    /// <param name="normalizedTime">�����ں���ռʱ��(��һ��)</param>
    /// <param name="targetDuration">�¶�����Ԥ�ڲ���ʱ��</param>
    /// <param name="animLen">������ԭʼʱ���������ٶ�Ϊ1ʱ��</param>
    /// <param name="targetFullTime">������ȫ��������Ҫ��Ԥ��ʱ�����������ںϺ�ӵ�0֡��ʼ���ţ�</param>
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
        float animSpeed = animLen / targetFullTime;

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
        float timeOffset = targetFullTime - targetDuration;
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
