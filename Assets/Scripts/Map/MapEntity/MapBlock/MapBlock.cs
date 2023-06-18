using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MapEntityWithCollider
{
    public MapBlock(MapBlockView mapBlockView)
    {
        BindMapEntityView(mapBlockView);
        SyncAllTansformInfoFromView();
    }


    public override int GetEntityType()
    {
        return EntityTypeDefine.Block;
    }

    public override void HandleCollideTo(ICollideProcessor tgtColliderProcessor)
    {
        return;
    }
}
