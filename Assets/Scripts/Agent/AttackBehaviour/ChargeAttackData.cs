using System;

[Serializable]
public class ChargeAttackData
{
    /// <summary>
    /// 蓄力攻击的id
    /// </summary>
    public uint chargeAttackUID;

    /// <summary>
    /// 蓄力动画
    /// </summary>
    public string chargeStateName;

    /// <summary>
    /// 所有蓄力攻击招式
    /// </summary>
    public ChargeAttackStep[] ChargeAttackSteps;
}
