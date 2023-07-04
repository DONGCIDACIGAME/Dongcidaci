using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongciDaci
{

    [System.Serializable]
    public class AgentAttribute
    {
        /// <summary>
        /// 当前生命
        /// </summary>
        public int crtHp = 10;

        /// <summary>
        /// 最大生命
        /// </summary>
        public int maxHp = 10;

        /// <summary>
        /// 基础攻击值
        /// </summary>
        public int baseAtk = 1;

        /// <summary>
        /// 防御百分比
        /// </summary>
        public int defenseRate = 0;

        /// <summary>
        /// 暴击率
        /// </summary>
        public float criticalRate = 0;

        /// <summary>
        /// 暴击伤害的倍率
        /// </summary>
        public float criticalDmgRate = 1.5f;

        /// <summary>
        /// 闪避率
        /// </summary>
        public float dodgeRate = 0;

        /// <summary>
        /// 移动速度
        /// </summary>
        public float moveSpeed = 2f;

        public AgentAttribute(int crtHp,int maxHp,int bsAtk,int defenseRate,float criticalRate,float criticalDmgRate,float dodgeRate,float moveSpeed)
        {
            this.crtHp = crtHp;
            this.maxHp = maxHp;
            this.baseAtk = bsAtk;
            this.defenseRate = defenseRate;
            this.criticalRate = criticalRate;
            this.criticalDmgRate = criticalDmgRate;
            this.dodgeRate = dodgeRate;
            this.moveSpeed = moveSpeed;
            
        }


    }


}

