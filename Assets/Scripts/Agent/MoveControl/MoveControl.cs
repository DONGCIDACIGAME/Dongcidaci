using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    private void Reset()
    {
        MoveTime = 0;
        MoveRecord = 0;
    }

    public virtual void MoveTowards(Vector3 towards, float distance, float duration)
    {
        if (towards.Equals(DirectionDef.none))
            return;

        Reset();

        Vector3 offset = towards.normalized * distance;
        MoveFromPos = mAgent.GetPosition();
        MoveToPos = MoveFromPos + offset;

        if (duration == 0)
        {
            MoveToPosition(MoveToPos);
            return;
        }

        MoveTime = duration;
        MoveRecord = 0;
    }

    public void MoveToPosition(Vector3 tgtPosition)
    {
        Reset();

        ConvexCollider2D collider = mAgent.GetCollider();

        var checkShape = collider.Convex2DShape;
        checkShape.AnchorPos = tgtPosition;
        bool ret = GameColliderManager.Ins.CheckCollideHappenWithShape(checkShape, mAgent.ColliderHandler, mAgent.GetCollider(), out Dictionary<ConvexCollider2D, Vector2> tgtsWithLeaveV2);
        if (ret)
        {
            // 发生了碰撞
            var unMoveColliders = GameColliderDefine.GetUnMoveableColliders(tgtsWithLeaveV2.Keys.ToArray());
            if (unMoveColliders.Count > 0)
            {
                // 这里存在问题，需要再考虑下多个碰撞体的
                // 目前策略同时碰多block,视为卡墙角，不能动
                if (unMoveColliders.Count > 1) return;

                // 单个墙障碍
                var leaveV2 = tgtsWithLeaveV2[unMoveColliders[0]];
                tgtPosition = new Vector3(tgtPosition.x + leaveV2.x, tgtPosition.y, tgtPosition.z + leaveV2.y);
            }

        }

        mAgent.SetPosition(tgtPosition);
    }

    public void OnUpdate(float deltaTime)
    {
        if (MoveTime == 0)
            return;

        MoveRecord += deltaTime;

        if (MoveRecord < MoveTime)
        {
            Vector3 pos = Vector3.Lerp(MoveFromPos, MoveToPos, MoveRecord / MoveTime);
            MoveToPosition(pos);
        }
        else
        {
            MoveTime = 0;
            MoveRecord = 0;
            MoveToPosition(MoveToPos);
        }
    }
}
