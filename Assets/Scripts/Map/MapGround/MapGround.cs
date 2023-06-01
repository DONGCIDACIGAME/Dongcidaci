using UnityEngine;
using System.Linq;
using GameEngine;

public class MapGround: MapEntity
{
    private GameObject _bindObj;

    private int[] _occupyMapIndexs;

    public MapGround(string prefabStr,Vector3 initPos,int[] mapIndexs)
    {
        this._bindObj = PrefabUtil.LoadPrefab(prefabStr,GameMapCenter.Ins.GroundLayerNode,"Map Init");
        this._bindObj.transform.position = initPos;
        this._occupyMapIndexs = mapIndexs;
    }

    public bool IsPosOnGround(Vector2 checkPos)
    {
        if (_occupyMapIndexs == null) return false;
        return _occupyMapIndexs.Contains(GameMapCenter.Ins.GetMapGridIndexWithPoint(checkPos));
    }
    


}
