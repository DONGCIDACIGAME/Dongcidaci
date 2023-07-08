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

        /// <summary>
        /// 检测指令类型是否可以响应
        /// </summary>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public bool CheckCmdReact(byte cmdType)
        {
            for (int i = 0; i < 8; i++)
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

