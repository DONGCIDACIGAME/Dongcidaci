using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapBlockType
{
    // 不可破坏障碍物
    UndamageBlock,
    // 可破坏障碍物
    DamagedBlock,

}


public abstract class MapBlock : MapEntity,ICollideProcessor
{
    private GameObject _bindObj;

    private int[] _occupyMapIndexs;

    private Vector3 _initPosV3;

    public void Initliazlie(string prefabStr, Vector3 initPos, int[] mapIndexs)
    {
        this._bindObj = PrefabUtil.LoadPrefab(prefabStr, GameMapCenter.Ins.BlockLayerNode, "Map Init");
        this._bindObj.transform.position = initPos;
        this._initPosV3 = initPos;
        this._occupyMapIndexs = mapIndexs;
    }


    public Entity GetProcessorEntity()
    {
        return this;
    }

    public abstract void HandleCollideTo(ICollideProcessor tgtColliderProcessor);

}
