using UnityEngine;


public enum GameColliderType
{
    //角色的碰撞体
    AgentCollider = 0,
    //攻击范围的碰撞体
    AtkAreaCollider,
    //地图事件的碰撞体
    MapEventCollider,
    //地图障碍物的碰撞体
    MapBlockCollider,

    Undefined,
}

public class GameColliderData2D
{
    /// <summary>
    /// 该碰撞体的类别
    /// </summary>
    public GameColliderType colliderType = GameColliderType.Undefined;

    /// <summary>
    /// 该碰撞体是否静态的
    /// </summary>
    public bool isStatic = true;

    /// <summary>
    /// 碰撞体的尺寸，中心为锚点
    /// </summary>
    public Vector2 size { get; private set; }

    /// <summary>
    /// 碰撞体与挂在游戏体Position的偏移关系
    /// </summary>
    public Vector2 offset { get; private set; }

    public GameColliderData2D(GameColliderType colliderType, Vector2 size, Vector2 offset, bool isStatic = true)
    {
        this.colliderType = colliderType;
        this.size = size;
        this.offset = offset;
        this.isStatic = isStatic;
    }
}
