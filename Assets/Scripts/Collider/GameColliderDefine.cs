using System.Collections.Generic;

public enum MyColliderType
{
    Collider_None = 0,
    Collider_Hero,
    Collider_Monster,
    Collider_NPC,
    Collider_Block,
    Collider_Trap,

    Collider_SkBullet,
    
}

public enum CollideHandleConfig
{
    HandleBoth = 0,
    HandleSrc,
    HandleTgt,

    NoHandle
}



public static class GameColliderDefine
{
    #region Old By Hou
    public static EmptyColliderHandler EMPTY_COLLIDER_HANDLER = new EmptyColliderHandler();

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

    #endregion

    /// <summary>
    /// 不可被移动穿过的碰撞体的类型
    /// </summary>
    private static readonly HashSet<MyColliderType> UnMoveColliderTypes = new HashSet<MyColliderType>() {
        MyColliderType.Collider_Hero,
        MyColliderType.Collider_Monster,
        MyColliderType.Collider_NPC,
        MyColliderType.Collider_Block
    };

    /// <summary>
    /// 不可被冲刺穿过的碰撞体类型
    /// </summary>
    private static readonly HashSet<MyColliderType> UnDashColliderTypes = new HashSet<MyColliderType>() {
        MyColliderType.Collider_Hero,
        MyColliderType.Collider_Monster,
        MyColliderType.Collider_NPC,
        MyColliderType.Collider_Block
    };



    public static bool CheckCanMoveThrough(MyColliderType colliderType)
    {
        return !UnMoveColliderTypes.Contains(colliderType);
    }

    public static List<ConvexCollider2D> GetUnMoveableColliders(ConvexCollider2D[] checkColliders)
    {
        var retColliders = new List<ConvexCollider2D>();
        if (checkColliders == null || checkColliders.Length ==0) return retColliders;
        
        for (int i=0;i<checkColliders.Length;i++)
        {
            if (UnMoveColliderTypes.Contains(checkColliders[i].GetColliderType()))
            {
                //This collider is unmoveable
                retColliders.Add(checkColliders[i]);
            }
        }

        return retColliders;
    }


    public static bool CheckCanDashThrough(MyColliderType colliderType)
    {
        return !UnDashColliderTypes.Contains(colliderType);
    }

    public static List<ConvexCollider2D> GetAgentColliders(ConvexCollider2D[] checkColliders)
    {
        var retColliders = new List<ConvexCollider2D>();
        if (checkColliders == null || checkColliders.Length == 0) return retColliders;

        for (int i = 0; i < checkColliders.Length; i++)
        {
            if (checkColliders[i].GetColliderType() == MyColliderType.Collider_Hero || checkColliders[i].GetColliderType() == MyColliderType.Collider_Monster)
            {
                //This collider is agent
                retColliders.Add(checkColliders[i]);
            }
        }

        return retColliders;
    }

}
