using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSkillEffect;


public interface IRemoteEffect
{
    /// <summary>
    /// 远程类效果的飞行距离
    /// </summary>
    public float FlightDis { get; }
}



public interface ITrigOnExcuteComboEft
{
    /// <summary>
    /// 角色初始化combo效果时触发；
    /// 该行为可以被阻止
    /// </summary>
    /// <param name="user"></param>
    /// <param name="eftWaitForTrig"></param>
    /// <returns></returns>
    public bool OnExcuteComboEft(Agent user, SkillEffect eftWaitForTrig);

    /// <summary>
    /// 这个效果的执行优先级
    /// </summary>
    public int TrigOnExComboEftPriority { get; }
}


public interface ITrigOnApplyDmg
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
    /// </summary>
    public int TrigOnApplyDmgPriority { get; }
}

public interface ITrigOnGetDmg
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
    public int TrigOnGetDmgPriority { get; }
}

public interface ITrigOnApplyAtkDmg
{
    public bool OnApplyAtkDmg(Agent user, Agent tgt, AttackDamage rlsDmg);

    public int TrigOnApplyAtkDmgPriority { get; }
}

public interface ITrigOnGetAtkDmg
{
    public bool OnGetAtkDmg(Agent user, Agent tgt, AttackDamage rlsDmg);

    public int TrigOnGetAtkDmgPriority { get; }
}

public interface ITrigOnApplyEftDmg
{
    public bool OnApplyEftDmg(Agent user, Agent tgt, EffectDamage rlsDmg);

    public int TrigOnApplyEftDmgPriority { get; }
}

public interface ITrigOnGetEftDmg
{
    public bool OnGetEftDmg(Agent user, Agent tgt, EffectDamage rlsDmg);

    public int TrigOnGetEftDmgPriority { get; }
}


public interface ITrigOnApplyRemoteEft
{
    public bool OnApplyRemoteEft(Agent user, IRemoteEffect rlsEft);

    public int TrigOnApplyRemoteEftPriority { get; }
}



