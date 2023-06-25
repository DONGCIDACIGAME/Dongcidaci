using GameEngine;
using UnityEngine;

public interface IGameCollider : IGameDisposable
{
    public int GetBindEntityId();
    public MyColliderType GetColliderType();

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



