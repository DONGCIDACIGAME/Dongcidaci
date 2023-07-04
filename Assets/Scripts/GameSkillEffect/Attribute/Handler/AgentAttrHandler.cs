using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkillEffect
{
    public class AgentAttrHandler
    {
        protected AgentAttribute _bindAttr;

        public void InitAgentAttr(AgentAttribute agtAttr)
        {
            _bindAttr = agtAttr;

        }

        /// <summary>
        /// 检查是否会产生暴击
        /// </summary>
        /// <returns></returns>
        public bool CheckWillCauseCriticalDmg()
        {
            // do somthing here

            return false;
        }

        /// <summary>
        /// 检查是否会闪避伤害
        /// </summary>
        /// <returns></returns>
        public bool CheckWillDodgeDmg()
        {

            return false;
        }






    }
}

