public static class GameColliderDefine
{
    /// <summary>
    /// 所有碰撞类型
    /// 对应所有的事件类型，目前最多支持32种
    /// 如果不够用，扩展为long类型
    /// </summary>
    public const int CollliderType_None             = 0;
    public const int ColliderType_Hero               = 1 << 0;
    public const int ColliderType_Monster         = 1 << 1;
    public const int ColliderType_NPC                = 1 << 2;
    public const int ColliderType_Block              = 1 << 3;
    public const int ColliderType_Trap                = 1 << 4;

    /// <summary>
    /// 可以移动过去的碰撞类型
    /// </summary>
    private static int CanNotMoveThroughColliderTypes =
        ColliderType_Hero
        | ColliderType_Monster
        | ColliderType_NPC
        | ColliderType_Block;

    /// <summary>
    /// 可以冲刺过去的碰撞类型
    /// </summary>
    private static int CanNotDashThroughColliderTypes = 0;

    /// <summary>
    /// 检测碰到某个碰撞时是否能移动过去
    /// </summary>
    /// <param name="colliderType"></param>
    /// <returns></returns>
    public static bool CheckCanMoveThrough(int colliderType)
    {
        return (colliderType & CanNotMoveThroughColliderTypes) == 0;
    }

    /// <summary>
    /// 检测碰到某个碰撞时是否能冲刺过去
    /// </summary>
    /// <param name="colliderType"></param>
    /// <returns></returns>
    public static bool CheckCanDashThrough(int colliderType)
    {
        return (colliderType & CanNotDashThroughColliderTypes) == 0;
    }
}
