using System;

[Serializable]
public class ChargeAttackData
{
    /// <summary>
    /// 蓄力攻击的id
    /// </summary>
    public uint chargeAttackUID;

    /// <summary>
    /// 蓄力行为数据
    /// </summary>
    public AgentActionData chargeActionData;

    /// <summary>
    /// 所有蓄力攻击招式
    /// </summary>
    public ChargeAttackStep[] ChargeAttackSteps;
}
