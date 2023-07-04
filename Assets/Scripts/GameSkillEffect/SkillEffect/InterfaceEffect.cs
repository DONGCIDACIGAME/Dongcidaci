using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可被角色携带的技能效果
/// </summary>
public interface IPortableEffect
{

}



public interface ITriggerOnExcuteComboEft
{
    /// <summary>
    /// 角色初始化combo效果时触发；
    /// 该行为可以被阻止
    /// </summary>
    /// <param name="user"></param>
    /// <param name="effectData"></param>
    /// <returns></returns>
    public bool OnExcuteComboEft(Agent user, SkillEffectData effectData);

    /// <summary>
    /// 这个效果的执行优先级
    /// </summary>
    public int TrigOnExComboEftPriority { get; }
}


public interface ITriggerOnApplyDmg
{
    /// <summary>
    /// 释放伤害时触发；
    /// 释放伤害的动作可被阻止
    /// </summary>
    /// <param name="user"></param>
    /// <param name="tgt"></param>
    /// <param name="rlsDmg"></param>
    /// <returns></returns>
    public bool OnApplyDmg(Agent user, Agent tgt, DmgEft rlsDmg); 
}

public interface ITriggerOnGetDmg
{
    /// <summary>
    /// 受到伤害时触发；
    /// 受到伤害的动作可被阻止
    /// </summary>
    /// <param name="user"></param>
    /// <param name="receiver"></param>
    /// <param name="gotDmg"></param>
    /// <returns></returns>
    public bool OnGetDmg(Agent user, Agent receiver, DmgEft gotDmg);
}









