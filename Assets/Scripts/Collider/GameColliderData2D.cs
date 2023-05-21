using UnityEngine;


public class GameColliderData2D
{
    
    /// <summary>
    /// 碰撞体的尺寸，中心为锚点
    /// </summary>
    public Vector2 size { get; private set; }

    /// <summary>
    /// 碰撞体与挂在游戏体Position的偏移关系
    /// </summary>
    public Vector2 offset { get; private set; }

    public GameColliderData2D(Vector2 size, Vector2 offset)
    {
        this.size = size;
        this.offset = offset;
    }



}
