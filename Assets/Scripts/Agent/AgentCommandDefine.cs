public static class AgentCommandDefine
{
    /// <summary>
    /// ��ָ��
    /// </summary>
    public const byte EMPTY = 0;

    /// <summary>
    /// ����
    /// </summary>
    public const byte IDLE = 1 << 0;

    /// <summary>
    /// ��
    /// </summary>
    public const byte WALK = 1 << 1;

    /// <summary>
    /// ��
    /// </summary>
    public const byte RUN = 1 << 2;

    /// <summary>
    /// ���
    /// </summary>
    public const byte DASH = 1 << 3;

    /// <summary>
    /// ���
    /// </summary>
    public const byte ATTACK_LIGHT = 1 << 4;

    /// <summary>
    /// �ػ�
    /// </summary>
    public const byte ATTACK_HARD = 1 << 5;

    /// <summary>
    /// �ܻ�
    /// </summary>
    public const byte BE_HIT = 1 << 6;


    /// <summary>
    /// ����ָ�����ȼ�������ָ����
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
