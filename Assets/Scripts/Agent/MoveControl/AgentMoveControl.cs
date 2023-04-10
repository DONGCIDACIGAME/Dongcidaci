using UnityEngine;

public abstract class AgentMoveControl
{
    protected Agent mAgent;
    public AgentMoveControl(Agent agt)
    {
        mAgent = agt;
    }


    private Vector3 TurnFromTowards;
    private Vector3 TurnToTowards;
    private float TurnTime;
    private float TurnRecord;

    public void TurnTo(Vector3 towards)
    {
        if (mAgent == null)
            return;

        TurnFromTowards = mAgent.GetTowards();
        TurnToTowards = towards;
        if (towards.Equals(Vector3.zero))
        {
            TurnTime = 0;
            TurnRecord = 0;
            return;
        }

        if (GameCommonConfig.AgentTurnSpeed == 0)
        {
            TurnTime = 0;
        }
        else
        {
            float rotation = Quaternion.FromToRotation(TurnFromTowards, TurnToTowards).eulerAngles.y; 
            if(rotation > 180)
            {
                rotation -= 180;
            }

            TurnTime = rotation / GameCommonConfig.AgentTurnSpeed;
        }
        TurnRecord = 0;
    }

    public virtual void Move(float deltaTime)
    {
        // ¿ØÖÆÒÆ¶¯
        Vector3 pos = mAgent.GetPosition() + mAgent.GetTowards().normalized * mAgent.GetSpeed() * deltaTime;
        mAgent.SetPosition(pos);
    }

    public virtual void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        if (TurnTime > 0)
        {
            TurnRecord += deltaTime;

            if (TurnRecord < TurnTime)
            {
                Vector3 towards = Vector3.Lerp(TurnFromTowards, TurnToTowards, TurnRecord / TurnTime);
                mAgent.SetTowards(towards);
            }
            else
            {
                TurnTime = 0;
                TurnRecord = 0;
                mAgent.SetTowards(TurnToTowards);
            }
        }
    }
}
