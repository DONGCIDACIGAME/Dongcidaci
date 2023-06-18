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

    public bool IsPosOnGround(Vector2 checkPos)
    {
        var groundView = this.mMapEntiyView as MapGroundView;
        if (groundView.OccupyMapIndexs == null) return false;
        return groundView.OccupyMapIndexs.Contains(GameMapCenter.Ins.GetMapGridIndexWithPoint(checkPos));
    }
    


}
