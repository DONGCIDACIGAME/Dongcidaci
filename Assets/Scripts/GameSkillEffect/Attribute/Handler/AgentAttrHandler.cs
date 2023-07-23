using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkillEffect
{
    public class AgentAttrHandler
    {
        protected AgentAttribute _bindAttr;
        protected Agent _bindAgt;

        public void InitAgentAttr(Agent agt, AgentAttribute agtAttr)
        {
            _bindAgt = agt;
            _bindAttr = agtAttr;
        }


        public void GetFinalDamage(Agent srcAgt, Damage finalDmg)
        {
            // 只有 attack damage 会触发受击的状态
            var realDmgValue = Mathf.RoundToInt((1f - _bindAttr.defenseRate) * (float)finalDmg.DmgValue);
            if (realDmgValue < 0) realDmgValue = 0;

            _bindAttr.crtHp -= realDmgValue;
            if (_bindAttr.crtHp < 0) _bindAttr.crtHp = 0;

            if(finalDmg is AttackDamage)
            {
                // trigger hit status
                Log.Logic(LogLevel.Info, "Got Final Damage Success -- Value is {0}", realDmgValue);
                AgentCommand beHitCmd = GamePoolCenter.Ins.AgentInputCommandPool.Pop();
                beHitCmd.Initialize(AgentCommandDefine.BE_HIT, MeterManager.Ins.MeterIndex, TimeMgr.Ins.FrameIndex, srcAgt.GetTowards());
                _bindAgt.OnCommand(beHitCmd);
            }
        }




        /// <summary>
        /// 检查是否会产生暴击
        /// </summary>
        /// <returns></returns>
        private bool CheckWillCauseCriticalDmg()
        {
            // do somthing here
            var rateValue = Mathf.RoundToInt(_bindAttr.criticalRate * 100f);
            var rdValue = Random.Range(0,100);
            return rdValue <= rateValue;
        }

        /// <summary>
        /// 获取暴击伤害
        /// </summary>
        /// <param name="criticalDmg"></param>
        /// <returns>未触发暴击则返回原有的伤害</returns>
        public bool GetCriticalDmg(ref Damage criticalDmg)
        {
            if (CheckWillCauseCriticalDmg())
            {
                criticalDmg.DmgValue = Mathf.RoundToInt((float)criticalDmg.DmgValue * _bindAttr.criticalDmgRate);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查是否会闪避伤害
        /// </summary>
        /// <returns></returns>
        public bool CheckWillDodgeDmg()
        {
            var rateValue = Mathf.RoundToInt(_bindAttr.dodgeRate * 100f);
            var rdValue = Random.Range(0, 100);
            return rdValue <= rateValue;
        }

        /// <summary>
        /// 检测指令类型是否可以响应
        /// </summary>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public bool CheckCmdReact(int cmdType)
        {
            for (int i = 0; i < 32; i++)
            {
                int ret = cmdType & (1 << i);
                if (ret > 0)
                {
                    return _bindAttr.cmdDisableRecord[i] == 0;
                }
            }

            return false;
        }
    }
}

