using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkillEffect
{
    [System.Serializable]
    public class HeroAttribute : AgentAttribute
    {
        /// <summary>
        /// 角色当前的大招能量
        /// </summary>
        public int crtEnergy = 0;

        /// <summary>
        /// 角色的大招能量上限
        /// </summary>
        public const int maxEnergy = 100;

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


        public HeroAttribute(int crtHp, int maxHp, int bsAtk, float defenseRate, float criticalRate, float criticalDmgRate, float dodgeRate, float moveSpeed, int crtEnergy,int extraEnergy, float beatTolerance, int luckyRate) : base(crtHp,maxHp,bsAtk,defenseRate,criticalRate,criticalDmgRate,dodgeRate,moveSpeed)
        {
            this.crtEnergy = crtEnergy;
            this.extraEnergyGain = extraEnergy;
            this.beatTolerance = beatTolerance;
            this.luckyRate = luckyRate;
        }



    }
}
