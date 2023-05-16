public static class AgentCommandDefine
{
    /// <summary>
    /// 空指令
    /// </summary>
    public const byte EMPTY = 0;

    /// <summary>
    /// 空闲
    /// </summary>
    public const byte IDLE = 1 << 0;

    /// <summary>
    /// 跑
    /// </summary>
    public const byte RUN = 1 << 1;

    /// <summary>
    /// 冲刺
    /// </summary>
    public const byte DASH = 1 << 2;

    /// <summary>
    /// 短按攻击
    /// </summary>
    public const byte ATTACK_SHORT = 1 << 3;

    /// <summary>
    /// 长按攻击
    /// </summary>
    public const byte ATTACK_LONG = 1 << 4;

    /// <summary>
    /// 受击
    /// </summary>
    public const byte BE_HIT = 1 << 5;
}
