using UnityEngine;

public class MoveControl
{
    private Agent mAgent;
    private Vector3 MoveToPos;
    private Vector3 MoveFromPos;
    private float MoveTime;
    private float MoveRecord;

    public MoveControl(Agent agt)
    {
        mAgent = agt;
    }

    public virtual void MoveTo(Vector3 towards, float distance, float duration)
    {
        if (towards.Equals(GamePlayDefine.InputDirection_NONE))
            return;

        Vector3 offset = towards.normalized * distance;
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

    public void OnUpdate(float deltaTime)
    {
        if (MoveTime == 0)
            return;

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
