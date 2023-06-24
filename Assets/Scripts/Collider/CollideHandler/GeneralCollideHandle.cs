using System.Collections;
using System.Collections.Generic;
using GameEngine;

public sealed class HeroCollideProcessor : Singleton<HeroCollideProcessor>,ICollideHandle<Hero>
{
    public void HandleColliderToBlock(Hero handler, MapBlock tgtBlock)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCollideToHero(Hero handler, Hero tgtHero)
    {
        throw new System.NotImplementedException();
    }

    public void HandleCollideToMonster(Hero handler, Hero tgtMonster)
    {
        throw new System.NotImplementedException();
    }





}
