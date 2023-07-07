using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

namespace GameSkillEffect
{
    public class AgentSkEftHandler : IRecycle
    {
        private Agent _bindAgent;
        private List<PortableEffect> _carrySkEfts;


        public void InitAgentSkEftHandler(Agent agt)
        {
            _bindAgent = agt;
            _carrySkEfts = new List<PortableEffect>();
        }



        /// <summary>
        /// 在每个击打点需要执行的效果合集
        /// </summary>
        /// <param name="effectDatas"></param>
        public void OnExcuteComboEffect(SkEftDataCollection effectDatas)
        {
            // 需要修改这里的逻辑
            if (effectDatas == null || effectDatas.effects == null || effectDatas.effects.Length == 0) return;

            // 1 创建效果
            var tgtRlsEffects = new List<SkillEffect>();
            foreach (var effectData in effectDatas.effects)
            {
                var rlsSkEft = GameSkEftPool.Ins.PopWithInit(_bindAgent,effectData);
                if (rlsSkEft != null) tgtRlsEffects.Add(rlsSkEft);
            }

            // 2 优先查找身上的状态对这个操作存在影响的

            // 3 触发剩下的效果
            foreach (var tgtRlsEft in tgtRlsEffects)
            {
                tgtRlsEft.TriggerSkEft();
            }

            /**
            Log.Logic(LogLevel.Info, "{0} excute effect {1}", _bindAgent.GetAgentId(), effectData.effectType);

            // 查找carrySkEft中的效果是 ITriggerOnExcuteComboEft
            // 按照TrigOnExComboEftPriority 优先级进行排序，优先级高的在前面
            var iEftInSort = new List<ITriggerOnExcuteComboEft>();
            foreach (var portableEft in _carrySkEfts)
            {
                if (!(portableEft is ITriggerOnExcuteComboEft)) continue;
                
                var iEft = portableEft as ITriggerOnExcuteComboEft;
                if (iEftInSort.Count == 0)
                {
                    iEftInSort.Add(iEft);
                    continue;
                }

                for (int i = 0; i < iEftInSort.Count; i++)
                {
                    if (iEft.TrigOnExComboEftPriority >= iEftInSort[i].TrigOnExComboEftPriority)
                    {
                        iEftInSort.Insert(i, iEft);

                    }
                    else if (i == iEftInSort.Count - 1)
                    {
                        // 优先级最小
                        iEftInSort.Add(iEft);
                    }
                }
            }

            foreach (var iEft in iEftInSort)
            {
                if (iEft.OnExcuteComboEft(_bindAgent, effectData) == false)
                {
                    // 这个初始化效果的行为被打断了
                    return;
                }
            }

            var rlsSkEft = SkEftDefine.GetSkEftBy(_bindAgent, effectData);
            if (rlsSkEft == null) return;
            rlsSkEft.TriggerSkEft();

            */
        }


        public void OnApplyDamage(Agent tgt, Damage rlsDmg)
        {
            Log.Logic(LogLevel.Info, "{0} OnApplyDamage {1}", _bindAgent.GetAgentId(), rlsDmg.DmgValue);
        }

        public void OnGetDamage(Agent src, Damage gotDmg)
        {

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

    }

}

