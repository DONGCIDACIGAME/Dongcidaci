using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DongciDaci
{
    public interface IEntityWithAgentAttr
    {
        public int GetCrtHp();
        public void SetCrtHp(int value);

        public int GetMaxHp();
        public void SetMaxHp(int value);

        public int GetBsAtk();
        public void SetBsAtk(int value);

        public int GetDefenseRate();
        public void SetDefenseRate(int value);

        public float GetCriticalRate();
        public void SetCriticalRate(float rate);

        public float GetCriticalDmgRate();
        public void SetCriticalDmgRate(float rate);

        public float GetDodgeRate();
        public void SetDodgeRate(float value);

        public float GetMoveSpeed();
        public void SetMoveSpeed(float value);

    }

    public interface IEntityWithHeroAttr: IEntityWithAgentAttr
    {
        public int GetExtraEnergyGain();
        public void SetExtraEnergyGain(int value);

        public float GetBeatTolerance();
        public void SetBeatTolerance(float value);

        public int GetLuckyRate();
        public void SetLuckyRate(int value);
    }




}

