using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSkillEffect
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
        public float defenseRate = 0;

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

        /// <summary>
        /// 指令禁用状态记录
        /// 目前指令数量不足8个，数组大小只做到8
        /// 每一个int代表这种类型的指令被禁用的次数
        /// </summary>
        public int[] cmdDisableRecord = new int[32];


        public AgentAttribute(int crtHp,int maxHp,int bsAtk,float defenseRate,float criticalRate,float criticalDmgRate,float dodgeRate,float moveSpeed)
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

        public void EnableCmdType(int cmdType)
        {
            for(int i = 0; i < 32; i++)
            {
                int ret = cmdType & (1 << i);
                if(ret > 0)
                {
                    int record = cmdDisableRecord[i];
                    cmdDisableRecord[i] = record == 0 ? 0 : --record;
                }
            }
        }

        public void DisableCmdType(int cmdType)
        {
            for (int i = 0; i < 32; i++)
            {
                int ret = cmdType & (1 << i);
                if (ret > 0)
                {
                    ++cmdDisableRecord[i];
                }
            }
        }


        /**
        public int CalculateRealDamage(int tgtDmgValue)
        {
            return tgtDmgValue + baseAtk;
        }

        public void GetDamage(int dmgValue)
        {
            //var realDmg = (100- defenseRate)*0.01f *dmgValue
        }
        */




    }


}

