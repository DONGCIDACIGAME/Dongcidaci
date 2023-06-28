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
    public IConvex2DShape Convex2DShape { get; }


}


public interface IColliderSetter
{
    public void UpdateColliderPos(ConvexCollider2D collider, Vector3 newAnchorPos);
    public void UpdateColliderRotateAngle(ConvexCollider2D collider, float newAngle);
    public void UpdateColliderScale(ConvexCollider2D collider, Vector3 newScale);

}


public interface IGameColliderHandler : IGameDisposable
{
    void HandleColliderToHero(Hero hero);
    void HandleColliderToMonster(Monster monster);
    void HandleColliderToBlock(MapBlock block);

    // handle other all 
}



