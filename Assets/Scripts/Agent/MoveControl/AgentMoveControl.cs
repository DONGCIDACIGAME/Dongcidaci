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

        if (towards.Equals(Vector3.zero))
        {
            TurnTime = 0;
            TurnRecord = 0;
            return;
        }

        TurnFromTowards = mAgent.GetTowards();
        TurnToTowards = towards;

        Log.Logic("Turn");
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


    private Vector3 MoveToPos;
    private Vector3 MoveFromPos;
    private float MoveTime;
    private float MoveRecord;
    public virtual void Dash(float distance, float duration)
    {
        Vector3 offset = mAgent.GetTowards() * distance;
        MoveFromPos = mAgent.GetPosition();
        MoveToPos = MoveFromPos + offset;

        if (duration == 0)
        {
            mAgent.SetPosition(MoveToPos);
            return;
        }

        MoveTime = duration;
        MoveRecord = 0;
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

        if(MoveTime > 0)
        {
            MoveRecord += deltaTime;

            if (MoveRecord < MoveTime)
            {
                Vector3 pos = Vector3.Lerp(MoveFromPos, MoveToPos, MoveRecord / MoveTime);
                mAgent.SetPosition(pos);
            }
            else
            {
                MoveTime = 0;
                MoveRecord = 0;
                mAgent.SetPosition(MoveToPos);
            }
        }
    }
}
