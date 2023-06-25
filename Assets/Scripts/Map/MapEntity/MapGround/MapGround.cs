using UnityEngine;
using System.Linq;

public class MapGround: MapEntity
{
    public MapGround(MapGroundView groundEntityView)
    {
        BindMapEntityView(groundEntityView);
        SyncAllTansformInfoFromView();
    }

    public override int GetEntityType()
    {
        return EntityTypeDefine.Ground;
    }

    
    public bool IsMapIndexInGround(int mapIndex)
    {
        if (mapIndex < 0) return false;
        var groundView = this._mMapEntiyView as MapGroundView;
        return groundView.OccupyMapIndexs.Contains(mapIndex);
    }

}
