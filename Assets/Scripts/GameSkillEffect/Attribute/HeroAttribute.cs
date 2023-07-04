using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongciDaci
{
    [System.Serializable]
    public class HeroAttribute : AgentAttribute
    {
        /// <summary>
        /// 额外能量获取
        /// </summary>
        public int extraEnergyGain = 0;

        /// <summary>
        /// 节拍的容差
        /// </summary>
        public float beatTolerance = 0.2f;

        /// <summary>
        /// 幸运值
        /// </summary>
        public int luckyRate = 0;


        public HeroAttribute(int crtHp, int maxHp, int bsAtk, int defenseRate, float criticalRate, float criticalDmgRate, float dodgeRate, float moveSpeed, int extraEnergy, float beatTolerance, int luckyRate) : base(crtHp,maxHp,bsAtk,defenseRate,criticalRate,criticalDmgRate,dodgeRate,moveSpeed)
        {
            this.extraEnergyGain = extraEnergy;
            this.beatTolerance = beatTolerance;
            this.luckyRate = luckyRate;
        }



    }
}
