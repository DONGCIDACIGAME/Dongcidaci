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
    public GameColliderType colliderType = GameColliderType.Undefined;

    public Vector2 size { get; private set; }
    public Vector2 offset { get; private set; }

    public GameColliderData2D(GameColliderType colliderType, Vector2 size, Vector2 offset)
    {
        this.colliderType = colliderType;
        this.size = size;
        this.offset = offset;
    }
}
