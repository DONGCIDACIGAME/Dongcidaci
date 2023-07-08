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
    /// <summary>
    /// 触发型的效果
    /// </summary>
    public interface ITriggerableEft
    {

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

        /// <summary>
        /// 这个效果的执行优先级
        /// </summary>
        public int PriorityOnExcComboEft { get; }

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

        /// <summary>
        /// 这个效果在造成伤害时触发的优先级
        /// 优先级越高，在这个触发时刻会越早被执行
        /// </summary>
        public int PriorityOnApplyDmg { get; }
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

        /// <summary>
        /// 这个效果在受到伤害时触发的优先级
        /// </summary>
        public int PriorityOnGetDmg { get; }
    }


    public interface ITrigOnApplyRemoteEft : ITriggerableEft
    {
        public bool OnApplyRemoteEft(Agent user, IRemoteEffect rlsEft);

        public int PriorityOnApplyRemoteEft { get; }
    }


    #endregion







}






