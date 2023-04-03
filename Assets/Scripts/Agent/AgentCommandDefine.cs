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
    /// 走
    /// </summary>
    public const byte WALK = 1 << 1;

    /// <summary>
    /// 跑
    /// </summary>
    public const byte RUN = 1 << 2;

    /// <summary>
    /// 冲刺
    /// </summary>
    public const byte DASH = 1 << 3;

    /// <summary>
    /// 轻击
    /// </summary>
    public const byte ATTACK_LIGHT = 1 << 4;

    /// <summary>
    /// 重击
    /// </summary>
    public const byte ATTACK_HARD = 1 << 5;

    /// <summary>
    /// 受击
    /// </summary>
    public const byte BE_HIT = 1 << 6;


    /// <summary>
    /// 按照指令优先级排序后的指令组
    /// </summary>
    public static byte[] COMMANDS = new byte[]
    {
        EMPTY,
        BE_HIT,
        ATTACK_HARD,
        ATTACK_LIGHT,
        DASH,
        RUN,
        WALK,
        IDLE
    };
}
