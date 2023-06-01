using UnityEngine;

public class TurnControl
{
    private Agent mAgent;
    private Vector3 TurnFromTowards;
    private Vector3 TurnToTowards;
    private float TurnTime;
    private float TurnRecord;

    public TurnControl(Agent agt)
    {
        mAgent = agt;
    }

    public Vector3 GetTowards()
    {
        return TurnToTowards;
    }

    public void TurnTo(Vector3 towards)
    {
        // 和上次的目标转向一致时不做重复处理
        if (towards.Equals(TurnToTowards))
            return;


        //Log.Error(LogLevel.Info, "TurnTo-{0}", towards);
        if (mAgent == null)
            return;

        if (towards.Equals(GamePlayDefine.InputDirection_NONE))
        {
            TurnTime = 0;
            TurnRecord = 0;
            TurnToTowards = GamePlayDefine.InputDirection_NONE;
            return;
        }

        TurnFromTowards = mAgent.GetRotation();
        TurnToTowards = towards;

        if (GameCommonConfig.AgentTurnSpeed == 0)
        {
            TurnTime = 0;
        }
        else
        {
            float rotation = Quaternion.FromToRotation(TurnFromTowards, TurnToTowards).eulerAngles.y;
            if (rotation > 180)
            {
                rotation -= 180;
            }

            TurnTime = rotation / GameCommonConfig.AgentTurnSpeed;
        }
        TurnRecord = 0;
    }

    public void OnUpdate(float deltaTime)
    {
        if (mAgent == null)
            return;

        if (TurnTime == 0)
            return;

        TurnRecord += deltaTime;

        if (TurnRecord < TurnTime)
        {
            Vector3 towards = Vector3.Lerp(TurnFromTowards, TurnToTowards, TurnRecord / TurnTime);
            mAgent.SetRotation(towards);
        }
        else
        {
            TurnTime = 0;
            TurnRecord = 0;
            mAgent.SetRotation(TurnToTowards);
        }
    }
}
