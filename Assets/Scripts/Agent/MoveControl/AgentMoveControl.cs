using UnityEngine;

public abstract class AgentMoveControl
{
    protected Agent mAgent;
    protected MoveControl mMoveCtl;
    protected TurnControl mTurnCtl;

    public AgentMoveControl(Agent agt)
    {
        mAgent = agt;
        mMoveCtl = new MoveControl(agt);
        mTurnCtl = new TurnControl(agt);
    }

    /// <summary>
    /// �ƶ������ⲿ������
    /// ���ⲿ�����ĺô��ǿ��Ը���״̬�����ƣ�ֻ����Ҫ�ƶ���״̬�����������ƶ�
    /// </summary>
    /// <param name="deltaTime">�ƶ����ʱ��</param>
    public virtual void Move(float deltaTime)
    {
        // �����ƶ�
        Vector3 pos = mAgent.GetPosition() + mAgent.GetTowards().normalized * mAgent.GetSpeed() * deltaTime;
        mAgent.SetPosition(pos);
    }

    public void Dash(float distance, float duration)
    {
        Vector3 towards = GamePlayDefine.InputDirection_NONE;
        // ��ȡת��ؼ���Ŀ��ת��
        if (mTurnCtl != null)
            towards = mTurnCtl.GetTowards();

        // ���û��Ŀ��ת����ʹ�ý�ɫ��ǰ����
        if (towards.Equals(GamePlayDefine.InputDirection_NONE))
            towards = mAgent.GetTowards();

        if (mMoveCtl != null)
            mMoveCtl.MoveTo(towards, distance, duration);
    }

    /// <summary>
    /// ת��
    /// </summary>
    /// <param name="towards">Ŀ�곯��</param>
    public void TurnTo(Vector3 towards)
    {
        if (mTurnCtl != null)
            mTurnCtl.TurnTo(towards);
    }


    public virtual void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        if (mMoveCtl != null)
            mMoveCtl.OnUpdate(deltaTime);

        if (mTurnCtl != null)
            mTurnCtl.OnUpdate(deltaTime);
    }
}
