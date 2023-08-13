using System;

[Serializable]
public class ChargeAttackData
{
    /// <summary>
    /// 蓄力攻击的id
    /// </summary>
    public uint chargeAttackUID;

    /// <summary>
    /// 蓄力攻击的名称
    /// </summary>
    public string chargeAttackName;

    /// <summary>
    /// 蓄力动画（一定是蓄力状态里的一个动画）
    /// 计划设计为不同的蓄力段，都在一个动画中
    /// </summary>
    public string chargeStateName;

    // 这里要不要考虑设计为，A拍对应A效果，B拍对应B效果？

    /// <summary>
    /// 蓄力完成所需要的节拍数
    /// </summary>
    public int ChargeFinishMeterLen;

    /// <summary>
    /// 蓄力攻击的动画（一定是蓄力攻击状态里的一个动画）
    /// </summary>
    public string attackStateName;
}
