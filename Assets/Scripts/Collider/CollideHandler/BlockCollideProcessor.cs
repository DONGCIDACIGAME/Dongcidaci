using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BlockCollideProcessor : CollideProcessor<BlockCollideProcessor, MapBlock>
{
    public override void HandleColliderToBlock(MapBlock handler, MapBlock tgtBlock)
    {
        return;
    }

    public override void HandleColliderToHero(MapBlock handler, Hero tgtHero)
    {
        return;
    }

    public override void HandleColliderToMonster(MapBlock handler, Monster tgtMonster)
    {
        return;
    }


}
