using System.Collections;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;
using System;

public struct MapGridInfo
{
    public int colNum;
    public int rowNum;
    public float cellWidth;
    public float cellHeight;

    public MapGridInfo(int colNumber, int rowNumber, float cellWidth, float cellHeight)
    {
        this.colNum = colNumber;
        this.rowNum = rowNumber;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
    }

    public int GetIndexByColAndRow(int colIndex, int rowIndex)
    {


        return 0;
    }

    public int[] GetIndexsWithColsAndRows(ValueTuple<int,int> colsRange,ValueTuple<int,int> rowsRange)
    {
        if (colsRange.Item1 > colsRange.Item2 || colsRange.Item2 > this.colNum) return null;
        if (rowsRange.Item1 > rowsRange.Item2) return null;

        return new int[3];
    }

}


public class GameColliderCenter : ModuleManager<GameColliderCenter>
{
    private MapGridInfo _mapGridConfig;


    /// <summary>
    /// 所有区块的碰撞体
    /// 数组的索引对应地图上某一块,通过索引获取地图上某一块中包含的所有2d碰撞体
    /// </summary>
    private HashSet<GameCollider2D>[] mAllGameColliders;

    /// <summary>
    /// 每个碰撞体所在的区块index
    /// key: GameCollider2D
    /// value: index of map area
    /// 每个碰撞体位置变化时，需要把原来所在的区块里，这个碰撞体删除，然后根据位置重新算一下所在区块
    /// </summary>
    private Dictionary<GameCollider2D, List<int>> mAllCollidersRecord;

    public override void Initialize()
    {
        mAllCollidersRecord = new Dictionary<GameCollider2D, List<int>>();
    }


    /// <summary>
    /// 根据地图的基本信息初始化
    /// 原点从左下角开始
    /// 5 6 7 8 9...
    /// 0 1 2 3 4...
    /// 第一个cell的基准坐标 0，0
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    /// <param name="cellWidth"></param>
    /// <param name="cellHeight"></param>
    public void InitWithMapInfo(float mapWidth, float mapHeight, float cellWidth, float cellHeight)
    {
        int colNum = Mathf.CeilToInt(mapWidth/cellWidth);
        int rowNum = Mathf.CeilToInt(mapHeight/cellHeight);

        mAllGameColliders = new HashSet<GameCollider2D>[colNum*rowNum];
        mAllCollidersRecord.Clear();

        this._mapGridConfig = new MapGridInfo(colNum,rowNum,cellWidth,cellHeight);

    }


    public void RegisterGameCollider(GameCollider2D collider)
    {
        // 1. 添加到所有对应的区块里 -- mAllGameColliders
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        collider.GetMaxEnvelopeArea(out Vector2 envelopPos, out Vector2 envelopSize);
        float minEnvelopX = envelopPos.x - envelopSize.x / 2f;
        float maxEnvelopX = envelopPos.x + envelopSize.x / 2f;
        int startColIndex = (minEnvelopX > 0) ? Mathf.RoundToInt(minEnvelopX / _mapGridConfig.cellWidth):0;
        int endColIndex = (maxEnvelopX > 0) ? Mathf.RoundToInt(maxEnvelopX / _mapGridConfig.cellWidth) : 0;



        // 2. 更新这个碰撞体所在的区块信息--mAllCollidersRecord
    }

    public void UnRegisterGameCollider(GameCollider2D collider)
    {
        // 删除这个碰撞体所在的区块信息--mAllGameColliders，mAllCollidersRecord
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        //check collider occur


    }

    public override void Dispose()
    {
        mAllGameColliders = null;
        mAllCollidersRecord = null;
    }


}
