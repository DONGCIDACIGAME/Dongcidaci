using System.Collections.Generic;

namespace GameSkillEffect
{
    public static class SkEftHelper
    {
        /// <summary>
        /// 获取技能效果可以击中的目标Agent
        /// </summary>
        /// <param name="eftUser"></param>
        /// <param name="eftCollideShape"></param>
        /// <param name="rlsTgt"></param>
        /// <returns>根据释放目标生成的命中角色</returns>
        public static List<Agent> GetHitAgents(Agent eftUser, IConvex2DShape eftCollideShape, EffectTgt rlsTgt)
        {
            if (eftUser == null || eftCollideShape == null) return null;
            var rets = new List<Agent>();

            eftCollideShape.AnchorPos = eftUser.GetPosition();
            eftCollideShape.AnchorAngle = eftUser.GetRotation().y;

            var hitRet = GameColliderManager.Ins.CheckCollideHappenWithShape(eftCollideShape, ColliderHanlderDefine.EmptyHandler, out List<ConvexCollider2D> hitColliders);

            if (hitRet)
            {
                Log.Logic(LogLevel.Info, "GetHitAgents Damage -- with hit tgts");

                // 1 根据当前的使用者类型筛选 实际受击的对象
                ConvexCollider2D userCollider = eftUser.GetCollider();
                MyColliderType userColliderType = userCollider.GetColliderType();
                List<ConvexCollider2D> tgtColloders = new List<ConvexCollider2D>();
                switch (rlsTgt)
                {
                    // 仅对敌人释放，此处NPC的类别，默认也是敌人
                    case EffectTgt.Enemy:

                        foreach (var collider in hitColliders)
                        {
                            if (collider.GetColliderType() != userColliderType)
                            {
                                tgtColloders.Add(collider);
                            }
                        }

                        break;

                    // 仅对自己释放
                    case EffectTgt.Self:
                        rets.Add(eftUser);
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
                    if (entity != null && entity is Agent)
                    {
                        rets.Add(entity as Agent);
                    }
                }


            }

            return rets;
        }


        public static List<ITrigOnExcuteComboEft> GetSortedEftsTrigOnExcComboEft(List<PortableEffect> tgtEffects)
        {
            var iEftInSort = new List<ITrigOnExcuteComboEft>();
            foreach (var portableEft in tgtEffects)
            {
                if (!(portableEft is ITrigOnExcuteComboEft)) continue;

                var iEft = portableEft as ITrigOnExcuteComboEft;
                if (iEftInSort.Count == 0)
                {
                    iEftInSort.Add(iEft);
                    continue;
                }

                int insertIndex = -1;
                for (int i = 0; i < iEftInSort.Count; i++)
                {
                    if (iEft.PriorityOnExcComboEft >= iEftInSort[i].PriorityOnExcComboEft)
                    {
                        insertIndex = i;
                    }
                }

                if (insertIndex >= 0) iEftInSort.Insert(insertIndex,iEft);
                if (insertIndex == -1) iEftInSort.Add(iEft);

            }

            return iEftInSort;

        }

        public static List<ITrigOnApplyDmg> GetSortedEftsTrigOnApplyDmg(List<PortableEffect> tgtEffects)
        {
            var iEftInSort = new List<ITrigOnApplyDmg>();
            foreach (var portableEft in tgtEffects)
            {
                if (!(portableEft is ITrigOnApplyDmg)) continue;

                var iEft = portableEft as ITrigOnApplyDmg;
                if (iEftInSort.Count == 0)
                {
                    iEftInSort.Add(iEft);
                    continue;
                }

                int insertIndex = -1;
                for (int i = 0; i < iEftInSort.Count; i++)
                {
                    if (iEft.PriorityOnApplyDmg >= iEftInSort[i].PriorityOnApplyDmg)
                    {
                        insertIndex = i;
                    }
                }

                if (insertIndex >= 0) iEftInSort.Insert(insertIndex, iEft);
                if (insertIndex == -1) iEftInSort.Add(iEft);

            }

            return iEftInSort;

        }


        public static List<ITrigOnGetDmg> GetSortedEftsTrigOnGetDmg(List<PortableEffect> tgtEffects)
        {
            var iEftInSort = new List<ITrigOnGetDmg>();
            foreach (var portableEft in tgtEffects)
            {
                if (!(portableEft is ITrigOnGetDmg)) continue;

                var iEft = portableEft as ITrigOnGetDmg;
                if (iEftInSort.Count == 0)
                {
                    iEftInSort.Add(iEft);
                    continue;
                }

                int insertIndex = -1;
                for (int i = 0; i < iEftInSort.Count; i++)
                {
                    if (iEft.PriorityOnGetDmg >= iEftInSort[i].PriorityOnGetDmg)
                    {
                        insertIndex = i;
                    }
                }

                if (insertIndex >= 0) iEftInSort.Insert(insertIndex, iEft);
                if (insertIndex == -1) iEftInSort.Add(iEft);

            }

            return iEftInSort;

        }


































    }
}



