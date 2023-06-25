using System.Collections;
using System.Collections.Generic;
using GameEngine;

public sealed class HeroCollideProcessor : CollideProcessor<HeroCollideProcessor, Hero>
{
    public override void HandleColliderToBlock(Hero handler, MapBlock tgtBlock)
    {
        
    }

    public override void HandleColliderToHero(Hero handler, Hero tgtHero)
    {
        
    }

    public override void HandleColliderToMonster(Hero handler, Monster tgtMonster)
    {
        
    }
}
