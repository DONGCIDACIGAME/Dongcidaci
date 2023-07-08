using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect
{
    public class AgentSkEftHandler : IRecycle,IGameUpdate,IMeterHandler
    {
        private Agent _bindAgent;
        private List<PortableEffect> _carrySkEfts;

        public void InitAgentSkEftHandler(Agent agt)
        {
            _bindAgent = agt;
            _carrySkEfts = new List<PortableEffect>();
        }


        public void OnExcHitPointComboEffects(ComboHitEffectsData hitEffects)
        {
            // 需要修改这里的逻辑
            if (hitEffects == null || hitEffects.effectsForRls == null || hitEffects.effectsForRls.Count == 0) return;

            // 1 优先查找身上的状态对这个操作存在影响的
            var iTrigOnExcComboEfts = SkEftHelper.GetSortedEftsTrigOnExcComboEft(_carrySkEfts);
            foreach (var iTrigOnExcComboEft in iTrigOnExcComboEfts)
            {
                iTrigOnExcComboEft.OnExcuteComboEfts(_bindAgent, hitEffects);
            }

            // 2 触发剩下的效果
            foreach (var tgtRlsEft in hitEffects.effectsForRls)
            {
                tgtRlsEft.TriggerSkEft();
            }
            
        }


        public bool OnApplyDamage(Agent tgt, Damage rlsDmg)
        {
            if (rlsDmg == null) return false;

            // 查找在这个时机触发的技能效果
            var iTrigOnApplyDmgEfts = SkEftHelper.GetSortedEftsTrigOnApplyDmg(_carrySkEfts);
            foreach (var iTrigOnApplyDmg in iTrigOnApplyDmgEfts)
            {
                if(iTrigOnApplyDmg.OnApplyDmg(_bindAgent, tgt, rlsDmg) == false)
                {
                    return false;
                }
            }

            // 通过属性handler判断是否产生暴击


            return true;
        }

        public bool OnGetDamage(Agent src, Damage gotDmg)
        {
            if (gotDmg == null) return false;

            // 查找在这个时机触发的技能效果
            var iTrigOnGetDmgEfts = SkEftHelper.GetSortedEftsTrigOnGetDmg(_carrySkEfts);
            foreach (var iTrigOnGetDmg in iTrigOnGetDmgEfts)
            {
                if (iTrigOnGetDmg.OnGetDmg(src, _bindAgent, gotDmg) == false)
                {
                    return false;
                }
            }

            // 通过属性handler判断是否闪避


            return true;
        }

        public bool OnApplyCriticalDamage(Agent tgt, Damage criticalDmg)
        {

            return true;
        }

        public bool OnGetCriticalDamage(Agent src,Damage criticalDmg)
        {
            return true;
        }




        public bool OnApplyRemoteEft(IRemoteEffect rlsEft)
        {

            return true;
        }




















        public void Dispose()
        {
            _bindAgent = null;

            // 回收携带的每个技能效果
            foreach (var skEft in _carrySkEfts)
            {
                skEft.Recycle();
            }
            _carrySkEfts = null;

        }

        public void Recycle()
        {
            Dispose();
            GamePoolCenter.Ins.AgtSkEftHandlerPool.Push(this);
        }

        public void RecycleReset()
        {
            Dispose();
        }

        /// <summary>
        /// 每帧调用，此处的调用者是Agent中的OnUpdate
        /// </summary>
        /// <param name="deltaTime"></param>
        public void OnUpdate(float deltaTime)
        {
            
        }

        public void OnMeterEnter(int meterIndex)
        {
            
        }

        public void OnMeterEnd(int meterIndex)
        {
            
        }



    }

}

