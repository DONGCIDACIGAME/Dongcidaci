using System.Collections;
using System.Collections.Generic;
using GameEngine;

public sealed class MonsterCollideProcessor : Singleton<MonsterCollideProcessor>, ICollideHandle<Monster>
{
    public void HandleColliderToBlock(Monster handler, MapBlock tgtBlock)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCollideToHero(Monster handler, Hero tgtHero)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCollideToMonster(Monster handler, Hero tgtMonster)
    {
        throw new System.NotImplementedException();
    }



}
