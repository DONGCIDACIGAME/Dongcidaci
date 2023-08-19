[System.Serializable]
public class ChargeAttackStep
{
    /// <summary>
    /// 所需的最低蓄力拍数
    /// </summary>
    public int chargeMeterLen;

    /// <summary>
    /// 对应的攻击行为
    /// </summary>
    public AgentActionData attackAction;
}
