using GameEngine;
using UnityEngine;

public interface IGameCollider : IGameDisposable
{
    public int GetBindEntityId();
    public MyColliderType GetColliderType();

}

public interface IConvex2DCollider:IGameCollider
{
    public int GetColliderUID();

    /// <summary>
    /// 获取凸多边形的所有顶点坐标
    /// 从左上顺时针旋转
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetSortedVertexs();

    /// <summary>
    /// 获取凸多边形的每一条边
    /// </summary>
    /// <returns></returns>
    public Vector2[,] GetEdges();

    /// <summary>
    /// 获取凸多边形每条边朝外的法向
    /// </summary>
    /// <returns></returns>
    public Vector2[] GetEdgeOutsideNormals();

}


public interface IColliderSetter
{
    public void UpdateColliderPos(GameCollider2D collider, Vector3 newAnchorPos);
    public void UpdateColliderRotateAngle(GameCollider2D collider, float newAngle);
    public void UpdateColliderScale(GameCollider2D collider, Vector3 newScale);

}


public interface IGameColliderHandler : IGameDisposable
{
    void HandleColliderToHero(Hero hero);
    void HandleColliderToMonster(Monster monster);
    void HandleColliderToBlock(MapBlock block);

    // handle other all 
}



