using UnityEngine;

public enum Convex2DShapeType
{
    //矩形
    Rect = 0,
    Ellipse,
    Circle,
}

public class GameColliderData2D
{
    /// <summary>
    /// 2D 碰撞体的类型
    /// </summary>
    public Convex2DShapeType shapeType = Convex2DShapeType.Rect;

    /// <summary>
    /// 碰撞体的尺寸，中心为锚点
    /// </summary>
    public Vector2 size { get; private set; }

    /// <summary>
    /// 碰撞体与挂在游戏体Position的偏移关系
    /// </summary>
    public Vector2 offset { get; private set; }

    public GameColliderData2D(Convex2DShapeType shapeType, Vector2 size, Vector2 offset)
    {
        this.shapeType = shapeType;
        this.size = size;
        this.offset = offset;
    }



}
