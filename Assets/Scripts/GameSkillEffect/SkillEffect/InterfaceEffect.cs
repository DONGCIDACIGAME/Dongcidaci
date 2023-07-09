using System.Collections;
using System.Collections.Generic;

namespace GameSkillEffect
{
    #region Skill effect supplement
    //技能效果的补充接口

    /// <summary>
    /// 如果是远程类的效果继承这个接口
    /// </summary>
    public interface IRemoteEffect
    {
        /// <summary>
        /// 远程类效果的射程距离
        /// </summary>
        public float FlightDis { get; }
    }

    /// <summary>
    /// 如果是控制类的效果继承这个接口
    /// </summary>
    public interface IControlEffect
    {


    }

    #endregion





    #region Triggerable Eft should assigned from these interface

    public struct TrigEftPriorityCfg
    {
        public int PriorityOnExcComboEft;

        public int PriorityOnApplyDmg;
        public int PriorityOnGetDmg;
        public int PriorityOnApplyCriticalDmg;
        public int PriorityOnGetCriticalDmg;
        public int PriorityOnDodgeDmg;
        public int PriorityOnDmgBeDodged;

        public int PriorityOnApplyRemoteEft;

    }


    /// <summary>
    /// 触发型的效果
    /// </summary>
    public interface ITriggerableEft
    {
        public TrigEftPriorityCfg PriorityCfg { get; }
    }

    public interface ITrigOnExcuteComboEft : ITriggerableEft
    {
        /// <summary>
        /// 角色初始化combo效果时触发；
        /// 该行为可以被阻止
        /// </summary>
        /// <param name="user"></param>
        /// <param name="eftWaitForTrig"></param>
        /// <returns></returns>
        public void OnExcuteComboEfts(Agent user, ComboHitEffectsData comboHitData);

    }


    public interface ITrigOnApplyDmg : ITriggerableEft
    {
        /// <summary>
        /// 释放伤害时触发；
        /// 释放伤害的动作可被阻止
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tgt"></param>
        /// <param name="rlsDmg"></param>
        /// <returns></returns>
        public bool OnApplyDmg(Agent user, Agent tgt, Damage rlsDmg);

    }


    public interface ITrigOnGetDmg : ITriggerableEft
    {
        /// <summary>
        /// 受到伤害时触发；
        /// 受到伤害的动作可被阻止
        /// </summary>
        /// <param name="user"></param>
        /// <param name="receiver"></param>
        /// <param name="gotDmg"></param>
        /// <returns></returns>
        public bool OnGetDmg(Agent user, Agent receiver, Damage gotDmg);

    }


    public interface ITrigOnApplyRemoteEft : ITriggerableEft
    {
        public bool OnApplyRemoteEft(Agent user, IRemoteEffect rlsEft);

    }








    #endregion







}






