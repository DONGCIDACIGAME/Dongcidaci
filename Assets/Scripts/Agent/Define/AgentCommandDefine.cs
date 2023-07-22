using System.Collections.Generic;

public static class AgentCommandDefine
{
    /// <summary>
    /// 空指令 0
    /// </summary>
    public const byte EMPTY = 0;

    /// <summary>
    /// 空闲 1
    /// </summary>
    public const byte IDLE = 1 << 0;

    /// <summary>
    /// 跑 2
    /// </summary>
    public const byte RUN = 1 << 1;

    /// <summary>
    /// 冲刺  4
    /// </summary>
    public const byte DASH = 1 << 2;

    /// <summary>
    /// 短按攻击 8
    /// </summary>
    public const byte ATTACK_SHORT = 1 << 3;

    /// <summary>
    /// 长按攻击 16
    /// </summary>
    public const byte ATTACK_LONG = 1 << 4;


    /// <summary>
    /// 受击32 不打断当前行为
    /// </summary>
    public const byte BE_HIT = 1 << 5;

    /// <summary>
    /// 受击 64 打断当前行为
    /// </summary>
    public const byte BE_HIT_BREAK = 1 << 6;

    /// <summary>
    /// 死亡 128
    /// </summary>
    public const byte DEAD = 1 << 7;






    /// <summary>
    /// 可以进行优化的指令集
    /// </summary>
    private static byte OptimizableCommands = IDLE | RUN;

    /// <summary>
    /// 是否是可优化的指令
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public static bool IsOptimizable(byte cmdType)
    {
        return (OptimizableCommands & cmdType) > 0;
    }

    private static byte ComboTriggerCommands = DASH | ATTACK_SHORT | ATTACK_LONG;

    /// <summary>
    /// 是否是可以触发combo的指令
    /// </summary>
    /// <param name="cmdType"></param>
    /// <returns></returns>
    public static bool IsComboTrigger(byte cmdType)
    {
        return (ComboTriggerCommands & cmdType) > 0;
    }
}
