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

    /// <summary>
    /// 根据行数和列数获取当前的地图块索引
    /// </summary>
    /// <param name="colIndex"></param>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    public int GetIndexByColAndRow(int colIndex, int rowIndex)
    {
        if (colIndex > colNum - 1 || rowIndex > rowIndex - 1) return 0;
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
    /// 存放的所有碰撞体
    /// </summary>
    private List<GameCollider2D> _allGameColliders;

    /// <summary>
    /// 所有区块的碰撞体
    /// 数组的索引对应地图上某一块,通过索引获取地图上某一块中包含的所有2d碰撞体
    /// </summary>
    private HashSet<GameCollider2D>[] _gameCollidersInMap;

    /// <summary>
    /// 每个碰撞体所在的区块index
    /// key: GameCollider2D
    /// value: index of map area
    /// 每个碰撞体位置变化时，需要把原来所在的区块里，这个碰撞体删除，然后根据位置重新算一下所在区块
    /// 这个字典可以不需要，因为，占据的格子可以直接动态算出
    /// </summary>
    //private Dictionary<GameCollider2D, List<int>> mAllCollidersRecord;

    public override void Initialize()
    {
        //mAllCollidersRecord = new Dictionary<GameCollider2D, List<int>>();
        _allGameColliders = new List<GameCollider2D>();
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
        _allGameColliders.Clear();

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
        if (_allGameColliders.Contains(collider)) return false;
        // it is a new collider
        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider);
        if (estimatedMapIndexs!=null)
        {
            collider.lastMapIndexs = estimatedMapIndexs;
            //添加到所有碰撞体中
            _allGameColliders.Add(collider);
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
        if (collider.lastMapIndexs !=null)
        {
            foreach (int lastIndex in collider.lastMapIndexs)
            {
                _gameCollidersInMap[lastIndex].Remove(collider);
            }
        }

        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider);
        collider.lastMapIndexs = estimatedMapIndexs;
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
        this._allGameColliders.Remove(collider);
        // clear collider info in map
        foreach (var mapIndex in collider.lastMapIndexs)
        {
            _gameCollidersInMap[mapIndex].Remove(collider);
        }
        collider.lastMapIndexs = null;

    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

        if (_allGameColliders.Count == 0) return;
        for (int i= _allGameColliders.Count-1;i>=0;i--)
        {
            // update collider map info
            if (_allGameColliders[i].ColliderData.isStatic == false)
            {
                UpdateGameCollidersInMap(_allGameColliders[i]);
            }
            //check collider occur
            foreach (var mapIndex in _allGameColliders[i].lastMapIndexs)
            {
                var collidersInThisMapCell = _gameCollidersInMap[mapIndex];
                foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
                {
                    // 是自己
                    if (tgtCollider == _allGameColliders[i]) continue;
                    if (_allGameColliders[i].CheckCollapse(tgtCollider.PosVector3,tgtCollider.ColliderData.size))
                    {
                        // 此处仅调用 检测方碰撞被检测方，避免重复调用
                        // 因为被检测方在遍历中会成为检测方
                        _allGameColliders[i].OnColliderEnter(tgtCollider);
                        //tgtCollider.OnColliderEnter(_allGameColliders[i]);
                    }
                }
            }
        }

    }

    public override void Dispose()
    {
        _allGameColliders = null;
        _gameCollidersInMap = null;
    }


}
