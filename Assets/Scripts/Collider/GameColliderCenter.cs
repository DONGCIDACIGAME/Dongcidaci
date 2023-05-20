using System.Collections;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;
using System;
using System.Linq;

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

    /// <summary>
    /// 根据行数和列数获取当前的地图块索引
    /// </summary>
    /// <param name="colIndex"></param>
    /// <param name="rowIndex"></param>
    /// <returns>-1 数组越界</returns>
    public int GetIndexByColAndRow(int colIndex, int rowIndex)
    {
        if (colIndex > colNum - 1 || rowIndex > rowNum - 1) return -1;
        return rowIndex * colNum + colIndex;
    }

    /// <summary>
    /// 根据行与列范围，转换成所有的索引
    /// </summary>
    /// <param name="colsRange"></param>
    /// <param name="rowsRange"></param>
    /// <returns></returns>
    public int[] GetIndexWithColsAndRows(ValueTuple<int,int> colsRange,ValueTuple<int,int> rowsRange)
    {
        List<int> ret = new List<int>();
        if (colsRange.Item1 > colsRange.Item2 || colsRange.Item2 > this.colNum-1) return null;
        if (rowsRange.Item1 > rowsRange.Item2 || rowsRange.Item2 > this.rowNum-1) return null;
        for (int i = colsRange.Item1;i<colsRange.Item2;i++)
        {
            for (int k = rowsRange.Item1;k<rowsRange.Item2; k++)
            {
                ret.Add(k*colNum + i);
            }
        }

        return ret.ToArray();
    }

    /// <summary>
    /// 根据地图块索引获取当前的区域信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Rect GetRectByMapIndex(int mapIndex)
    {
        var crtRowIndex = mapIndex / this.colNum;
        var crtColIndex = mapIndex % this.colNum;

        var tgtRect = new Rect(new Vector2(crtColIndex*cellWidth,crtRowIndex*cellHeight),new Vector2(this.cellWidth,this.cellHeight));
        return tgtRect;
    }


}


public class GameColliderCenter : ModuleManager<GameColliderCenter>
{
    private MapGridInfo _mapGridConfig;

    /// <summary>
    /// 所有区块的碰撞体
    /// 数组的索引对应地图上某一块,通过索引获取地图上某一块中包含的所有2d碰撞体
    /// </summary>
    private HashSet<GameCollider2D>[] _gameCollidersInMap;

    /// <summary>
    /// 记录每个碰撞体占用的地图块索引
    /// 使用对象直接作为key，在生成查询地址时慢于值类型，查询时影响较小
    /// </summary>
    private Dictionary<GameCollider2D, int[]> _mAllCollidersRecord;

    public override void Initialize()
    {
        _mAllCollidersRecord = new Dictionary<GameCollider2D, int[]>();
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

        _gameCollidersInMap = new HashSet<GameCollider2D>[colNum*rowNum];
        _mAllCollidersRecord.Clear();

        this._mapGridConfig = new MapGridInfo(colNum,rowNum,cellWidth,cellHeight);

    }

    /// <summary>
    /// 获取某个碰撞体最大包络占据的所有地图索引
    /// 粗略的计算方法，性能更好
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private int[] GetRoundOccupyMapIndexsWith(GameCollider2D collider)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        collider.GetMaxEnvelopeArea(out Vector2 envelopPos, out Vector2 envelopSize);
        float minEnvelopX = envelopPos.x - envelopSize.x / 2f;
        float maxEnvelopX = envelopPos.x + envelopSize.x / 2f;
        int startColIndex = (minEnvelopX > 0) ? Mathf.RoundToInt(minEnvelopX / _mapGridConfig.cellWidth) : 0;
        int endColIndex = (maxEnvelopX > 0) ? Mathf.RoundToInt(maxEnvelopX / _mapGridConfig.cellWidth) : 0;
        float minEnvelopY = envelopPos.y - envelopSize.y / 2f;
        float maxEnvelopY = envelopPos.y + envelopSize.y / 2f;
        int startRowIndex = (minEnvelopY > 0) ? Mathf.RoundToInt(minEnvelopY / _mapGridConfig.cellHeight) : 0;
        int endRowIndex = (maxEnvelopY > 0) ? Mathf.RoundToInt(maxEnvelopY / _mapGridConfig.cellHeight) : 0;
        var estimateMapIndexs = _mapGridConfig.GetIndexWithColsAndRows((startColIndex, endColIndex), (startRowIndex, endRowIndex));
        return estimateMapIndexs;
    }

    /// <summary>
    /// 向这个碰撞管理中心注册一个新的碰撞体
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public bool RegisterGameCollider(GameCollider2D collider)
    {
        if (_mAllCollidersRecord.ContainsKey(collider)) return false;
        // it is a new collider
        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider);
        if (estimatedMapIndexs!=null)
        {
            //添加到所有碰撞体中
            _mAllCollidersRecord.Add(collider,estimatedMapIndexs);
            //添加到地图碰撞体中
            foreach (int mapIndex in estimatedMapIndexs)
            {
                if(_gameCollidersInMap[mapIndex] == null)
                {
                    // hash set not created
                    _gameCollidersInMap[mapIndex] = new HashSet<GameCollider2D>();
                }

                _gameCollidersInMap[mapIndex].Add(collider);
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    private void UpdateGameCollidersInMap(GameCollider2D collider)
    {
        // remove last info in map
        var lastMapIndexs = _mAllCollidersRecord[collider];
        foreach (int lastIndex in lastMapIndexs)
        {
            _gameCollidersInMap[lastIndex].Remove(collider);
        }


        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider);
        _mAllCollidersRecord[collider] = estimatedMapIndexs;
        if (estimatedMapIndexs!=null)
        {
            //添加到地图碰撞体中
            foreach (int mapIndex in estimatedMapIndexs)
            {
                if (_gameCollidersInMap[mapIndex] == null)
                {
                    // hash set not created
                    _gameCollidersInMap[mapIndex] = new HashSet<GameCollider2D>();
                }

                _gameCollidersInMap[mapIndex].Add(collider);
            }
        }
    }

    public void UnRegisterGameCollider(GameCollider2D collider)
    {
        var lastMapIndexs = _mAllCollidersRecord[collider];
        this._mAllCollidersRecord.Remove(collider);
        // clear collider info in map
        foreach (var mapIndex in lastMapIndexs)
        {
            _gameCollidersInMap[mapIndex].Remove(collider);
        }

    }

    /// <summary>
    /// 检查某个碰撞体是否在地图上产生了碰撞
    /// </summary>
    /// <param name="checkCollider"></param>
    private void CheckColliderHappen(GameCollider2D checkCollider)
    {
        foreach (var mapIndex in _mAllCollidersRecord[checkCollider])
        {
            var collidersInThisMapCell = _gameCollidersInMap[mapIndex];
            foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
            {
                // 是自己
                if (tgtCollider == checkCollider) continue;
                if (checkCollider.CheckCollapse(tgtCollider.PosVector3, tgtCollider.ColliderData.size))
                {
                    // 此处仅调用 检测方碰撞被检测方，避免重复调用
                    // 因为被检测方在遍历中会成为检测方
                    checkCollider.OnColliderEnter(tgtCollider);
                }
            }
        }
    }



    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (_mAllCollidersRecord.Count == 0) return;
        var allCollidersList = _mAllCollidersRecord.Keys.ToList<GameCollider2D>();

        for (int i = allCollidersList.Count - 1; i >= 0; i--)
        {
            if (allCollidersList[i].ColliderData.isStatic == false && allCollidersList[i].CheckColliderMove())
            {
                // 碰撞体产生了移动
                UpdateGameCollidersInMap(allCollidersList[i]);
                CheckColliderHappen(allCollidersList[i]);
            }
        }

    }

    public override void Dispose()
    {
        _mAllCollidersRecord = null;
        _gameCollidersInMap = null;
    }


}
