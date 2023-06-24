using GameEngine;
using UnityEngine;

public interface IGameCollider : IGameDisposable
{
    public int GetBindEntityId();
}


public interface IGameCollider2D : IGameCollider
{
    public bool CheckPosInCollider(Vector2 pos);
}

public interface IGameColliderHandler
{
    void HandleColliderToHero(Hero hero);
    void HandleColliderToMonster(Monster monster);
    void HandleColliderToBlock(MapBlock block);
    //void HandleColliderToTrap()
}

