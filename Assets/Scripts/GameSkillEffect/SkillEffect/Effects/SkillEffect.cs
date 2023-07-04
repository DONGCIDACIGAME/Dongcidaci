using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public abstract class SkillEffect
{
    protected Agent _eftUser;
    protected SkillEffectData _initSkEftData;
    protected IConvex2DShape _eftCollideShape;

    public abstract bool InitSkEft(Agent user, SkillEffectData initData);
    public abstract void TriggerSkEft();

    public List<Agent> GetHitAgents()
    {
        if (_eftUser == null || _eftCollideShape == null) return null;
        var rets = new List<Agent>();

        _eftCollideShape.AnchorPos = _eftUser.GetPosition();
        _eftCollideShape.AnchorAngle = _eftUser.GetRotation().y;

        var hitRet = GameColliderManager.Ins.CheckCollideHappenWithShape(_eftCollideShape, out List<ConvexCollider2D> hitColliders,null);

        
        if (hitRet)
        {
            // 1 根据当前的使用者类型筛选 实际受击的对象
            ConvexCollider2D userCollider = _eftUser.GetCollider();
            MyColliderType userColliderType = userCollider.GetColliderType();
            List<ConvexCollider2D> tgtColloders = new List<ConvexCollider2D>();
            switch (_initSkEftData.rlsTgt)
            {
                // 仅对敌人释放，此处NPC的类别，默认也是敌人
                case EffectTgt.Enemy:

                    foreach (var collider in hitColliders)
                    {
                        if (collider.GetColliderType()!=userColliderType)
                        {
                            tgtColloders.Add(collider);
                        }
                    }

                    break;

                // 仅对自己释放
                case EffectTgt.Self:
                    rets.Add(_eftUser);
                    return rets;

                // 仅对友军释放不包含自己
                case EffectTgt.TeamWithoutSelf:
                    foreach (var collider in hitColliders)
                    {
                        if (collider.GetColliderType() == userColliderType && collider.GetColliderUID() != userCollider.GetColliderUID())
                        {
                            tgtColloders.Add(collider);
                        }
                    }
                    break;

                // 对友军释放同时包含自己
                case EffectTgt.TeamIncludeSelf:
                    foreach (var collider in hitColliders)
                    {
                        if (collider.GetColliderType() == userColliderType)
                        {
                            tgtColloders.Add(collider);
                        }
                    }

                    if (tgtColloders.Contains(userCollider) == false)
                    {
                        tgtColloders.Add(userCollider);
                    }
                    break;

                // 除了自己以外的所有单位
                case EffectTgt.AnyOthers:

                    foreach (var collider in hitColliders)
                    {
                        if (collider.GetColliderUID() != userCollider.GetColliderUID())
                        {
                            tgtColloders.Add(collider);
                        }
                    }

                    break;

                // 所有单位
                case EffectTgt.All:

                    tgtColloders = hitColliders;
                    if (tgtColloders.Contains(userCollider) == false)
                    {
                        tgtColloders.Add(userCollider);
                    }

                    break;

            }

            // 2 通过碰撞体生成agents
            foreach (var tgtCollider in tgtColloders)
            {
                if (tgtCollider.GetBindEntityId() == 0) continue;
                var entity = EntityManager.Ins.GetEntity(tgtCollider.GetBindEntityId());
                if (entity!=null && entity is Agent)
                {
                    rets.Add(entity as Agent);
                }
            }


        }

        return rets;
    }

}
