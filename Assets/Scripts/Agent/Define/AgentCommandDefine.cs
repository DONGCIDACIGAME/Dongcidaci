public static class AgentCommandDefine
{
    /// <summary>
    /// 空指令 0
    /// </summary>
    public const int EMPTY = 0;

    /// <summary>
    /// 空闲 1
    /// </summary>
    public const int IDLE = 1 << 0;

    /// <summary>
    /// 跑 2
    /// </summary>
    public const int RUN = 1 << 1;

    /// <summary>
    /// 冲刺  4
    /// </summary>
    public const int DASH = 1 << 2;

    /// <summary>
    /// 短按攻击 8
    /// </summary>
    public const int ATTACK_SHORT = 1 << 3;

    /// <summary>
    /// 长按攻击 16
    /// </summary>
    public const int ATTACK_LONG = 1 << 4;


    /// <summary>
    /// 受击32 不打断当前行为
    /// </summary>
    public const int BE_HIT = 1 << 5;

    /// <summary>
    /// 受击 64 打断当前行为
    /// </summary>
    public const int BE_HIT_BREAK = 1 << 6;

    /// <summary>
    /// 死亡 128
    /// </summary>
    public const int DEAD = 1 << 7;

    /// <summary>
    /// 即时攻击-short
    /// </summary>
    public const int ATTACK_SHORT_INSTANT = 1 << 8;

    /// <summary>
    /// 即时攻击-long
    /// </summary>
    public const int ATTACK_LONG_INSTANT = 1 << 9;


    /// <summary>
    /// 可以进行优化的指令集(1拍里多个相同指令时可以优化为1个)
    /// </summary>
    private static int OptimizableCommands = IDLE;

    /// <summary>
    /// 是否是可优化的指令
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public static bool IsOptimizable(int cmdType)
    {
        return (OptimizableCommands & cmdType) > 0;
    }

    /// <summary>
    /// 所有可以触发combo的指令类型
    /// </summary>
    private static int ComboTriggerCommands = DASH | ATTACK_SHORT | ATTACK_LONG | ATTACK_SHORT_INSTANT | ATTACK_LONG_INSTANT;

    /// <summary>
    /// 是否是可以触发combo的指令
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public static bool IsComboTrigger(int cmdType)
    {
        return (ComboTriggerCommands & cmdType) > 0;
    }
}
