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
    /// �̰�����
    /// </summary>
    public const byte ATTACK_SHORT = 1 << 4;

    /// <summary>
    /// ��������
    /// </summary>
    public const byte ATTACK_LONG = 1 << 5;

    /// <summary>
    /// �ܻ�
    /// </summary>
    public const byte BE_HIT = 1 << 6;
}
