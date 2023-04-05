public static class AgentCommandDefine
{
    /// <summary>
    /// ¿ÕÖ¸Áî
    /// </summary>
    public const byte EMPTY = 0;

    /// <summary>
    /// ¿ÕÏÐ
    /// </summary>
    public const byte IDLE = 1 << 0;

    /// <summary>
    /// ×ß
    /// </summary>
    public const byte WALK = 1 << 1;

    /// <summary>
    /// ÅÜ
    /// </summary>
    public const byte RUN = 1 << 2;

    /// <summary>
    /// ³å´Ì
    /// </summary>
    public const byte DASH = 1 << 3;

    /// <summary>
    /// Çá»÷
    /// </summary>
    public const byte ATTACK_LIGHT = 1 << 4;

    /// <summary>
    /// ÖØ»÷
    /// </summary>
    public const byte ATTACK_HARD = 1 << 5;

    /// <summary>
    /// ÊÜ»÷
    /// </summary>
    public const byte BE_HIT = 1 << 6;
}
