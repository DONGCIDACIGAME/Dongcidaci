public static class AgentCommandDefine
{
    /// <summary>
    /// 空指令 0
    /// </summary>
    public const int EMPTY = 0;

    /// <summary>
    /// 空闲 1
    /// </summary>
    public const int IDLE                                   = 1 << 0;

    /// <summary>
    /// 跑 2
    /// </summary>
    public const int RUN                                    = 1 << 1;


    /// <summary>
    /// 冲刺  4
    /// </summary>
    public const int DASH                                   = 1 << 2;

    /// <summary>
    /// 即时攻击 8
    /// </summary>
    public const int INSTANT_ATTACK                         = 1 << 3;

    /// <summary>
    /// 卡节拍的攻击 16
    /// </summary>
    public const int METER_ATTACK                           = 1 << 4;

    /// <summary>
    /// 蓄力攻击-蓄力 32
    /// </summary>
    public const int CHARGING                                = 1 << 5;

    /// <summary>
    /// 蓄力攻击-攻击 64
    /// </summary>
    public const int CHARGING_ATTACK                        = 1 << 6;

    /// <summary>
    /// 蓄力攻击触发失败
    /// </summary>
    public const int CHARGING_ATTACKFAILED                  = 1 << 7;

    /// <summary>
    /// 受击128 不打断当前行为
    /// </summary>
    public const int BE_HIT                                 = 1 << 8;

    /// <summary>
    /// 受击 256 打断当前行为
    /// </summary>
    public const int BE_HIT_BREAK                           = 1 << 9;

    /// <summary>
    /// 死亡 512
    /// </summary>
    public const int DEAD                                   = 1 << 10;

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
    private static int ComboTriggerCommands = DASH | INSTANT_ATTACK ;

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
