using System.Collections;
using System.Collections.Generic;
using GameEngine;

public sealed class MonsterCollideProcessor : CollideProcessor<MonsterCollideProcessor, Monster>
{
    public override void HandleColliderToBlock(Monster handler, MapBlock tgtBlock)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleColliderToHero(Monster handler, Hero tgtHero)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleColliderToMonster(Monster handler, Monster tgtMonster)
    {
        throw new System.NotImplementedException();
    }
}
