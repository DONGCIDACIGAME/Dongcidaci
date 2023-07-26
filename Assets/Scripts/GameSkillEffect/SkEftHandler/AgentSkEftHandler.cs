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

        /// <summary>
        /// 获取当前携带的所有buff
        /// </summary>
        /// <returns></returns>
        public List<Buff> GetAllCarryBuffs()
        {
            return null;
        }

        /// <summary>
        /// 获取当前携带的所有被动效果
        /// </summary>
        /// <returns></returns>
        public List<PassiveEffect> GetAllPasvEfts()
        {
            return null;
        } 


        public void InitAgentSkEftHandler(Agent agt)
        {
            _bindAgent = agt;
            _carrySkEfts = new List<PortableEffect>();
        }


        public void AddAPortableEft(PortableEffect newEft)
        {

        }

        public void RemoveAPortableEft(PortableEffect eftToRemove)
        {

        }



        /// <summary>
        /// 当执行触发一个combo的打击点效果时
        /// </summary>
        /// <param name="hitEffects"></param>
        public void OnExcHitPointComboEffects(ComboHitEffectsData hitEffects)
        {
            // 需要修改这里的逻辑
            if (hitEffects == null || hitEffects.effectsForRls == null || hitEffects.effectsForRls.Count == 0) return;

            // 1 优先查找身上的状态对这个操作存在影响的
            var iTrigEfts = SkEftHelper.GetSortedTriggerableEfts<ITrigOnExcuteComboEft>(_carrySkEfts);
            foreach (var iTrigEft in iTrigEfts)
            {
                var iTrigOnExcComboEft = iTrigEft as ITrigOnExcuteComboEft;
                iTrigOnExcComboEft.OnExcuteComboEfts(_bindAgent, hitEffects);
            }

            // 显示这个hit的特效
            GameFXManager.Ins.ShowAFX(hitEffects.rlsFxCfg, _bindAgent);

            // 2 触发剩下的效果
            foreach (var tgtRlsEft in hitEffects.effectsForRls)
            {
                // 仅针对当前击打点触发的效果中有攻击伤害时才会产生被击打的特效
                if(tgtRlsEft is Damage)
                {
                    var rlsDmg = tgtRlsEft as Damage;
                    rlsDmg.DmgHitFXCfg = hitEffects.hitFxCfg;
                }

                tgtRlsEft.TriggerSkEft();
            }
            
        }

        #region DAMAGE HANDLE REGION

        public void OnApplyDamage(Agent tgt, Damage rlsDmg)
        {
            if (rlsDmg == null) return;

            // 查找在这个时机触发的技能效果
            var iTrigEfts = SkEftHelper.GetSortedTriggerableEfts<ITrigOnApplyDmg>(_carrySkEfts);
            foreach (var iTrigEft in iTrigEfts)
            {
                var iTrigOnApplyDmg = iTrigEft as ITrigOnApplyDmg;
                if (iTrigOnApplyDmg.OnApplyDmg(_bindAgent, tgt, rlsDmg) == false)
                {
                    return;
                }
            }

            // 通过属性handler判断是否产生暴击
            if (_bindAgent.AttrHandler.GetCriticalDmg(ref rlsDmg))
            {
                OnApplyCriticalDamage(tgt,rlsDmg);
            }
            else
            {
                tgt.SkillEftHandler.OnGetDamage(_bindAgent,rlsDmg);
            }

        }

        public void OnGetDamage(Agent src, Damage gotDmg)
        {
            if (gotDmg == null) return;

            // 查找在这个时机触发的技能效果
            var iTrigEfts = SkEftHelper.GetSortedTriggerableEfts<ITrigOnGetDmg>(_carrySkEfts);
            foreach (var iTrigEft in iTrigEfts)
            {
                var iTrigOnGetDmg = iTrigEft as ITrigOnGetDmg;
                if (iTrigOnGetDmg.OnGetDmg(src, _bindAgent, gotDmg) == false)
                {
                    return;
                }
            }

            // 通过属性handler判断是否闪避
            if (_bindAgent.AttrHandler.CheckWillDodgeDmg())
            {
                // 成功闪避了这次伤害
                // 调用当我闪避了伤害的效果
                OnDodgeADamage(src,gotDmg);
                // 调用释放者的伤害被闪避
                src.SkillEftHandler.OnDamageBeDodged(_bindAgent,gotDmg);
            }
            else
            {
                OnFinalGetADmg(src, gotDmg);
            }

        }

        public void OnApplyCriticalDamage(Agent tgt, Damage criticalDmg)
        {
            if (criticalDmg == null) return;
            // 检查身上的效果是否会在此刻触发




            // 目标受到暴击伤害
            tgt.SkillEftHandler.OnGetCriticalDamage(_bindAgent,criticalDmg);
        }

        public bool OnGetCriticalDamage(Agent src,Damage criticalDmg)
        {
            return true;
        }

        public void OnDodgeADamage(Agent src, Damage dmgDodged)
        {

        }

        public void OnDamageBeDodged(Agent tgt, Damage dmgDodged)
        {

        }

       
        public void OnFinalGetADmg(Agent srcAgt,Damage finalGotDmg)
        {
            // 显示被打中的特效
            GameFXManager.Ins.ShowAFX(finalGotDmg.DmgHitFXCfg,_bindAgent);

            //调用自己的attrHandler处理受到伤害的逻辑
            _bindAgent.AttrHandler.GetFinalDamage(srcAgt,finalGotDmg);
        }

        #endregion

        public void OnApplyHeal()
        {

        }

        public void OnGetHeal()
        {

        }




        public bool OnApplyRemoteEft(IRemoteEffect rlsEft)
        {

            return true;
        }


        public bool OnApplyBuff(Agent tgt, Buff rlsBuff)
        {
            return true;
        }

        public bool OnGetBuff(Agent src, Buff buffGot)
        {

            return true;
        }


        public bool OnApplyMoveEffect(MobilityEffect moveEftToRls)
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
        public void OnGameUpdate(float deltaTime)
        {
            
        }

        public void OnMeterEnter(int meterIndex)
        {
            
        }

        public void OnMeterEnd(int meterIndex)
        {
            
        }

        public void OnDisplayPointBeforeMeterEnter(int meterIndex)
        {

        }

    }

}

