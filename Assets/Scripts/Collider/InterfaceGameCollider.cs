using GameEngine;
using UnityEngine;

public interface IGameCollider : IGameDisposable
{
    public int GetBindEntityId();
}

public interface IGameColliderHandler : IGameDisposable
{
    void HandleColliderToHero(Hero hero);
    void HandleColliderToMonster(Monster monster);
    void HandleColliderToBlock(MapBlock block);

    // handle other all 
}


public interface ICollideHandle<T>
{
    public void HandleCollideToHero(T handler, Hero tgtHero);
    public void HandleCollideToMonster(T handler, Hero tgtMonster);
    public void HandleColliderToBlock(T handler, MapBlock tgtBlock);


}
