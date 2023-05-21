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
        return GetRoundOccupyMapIndexsWith(collider.RectanglePosv3);
    }

    private int[] GetRoundOccupyMapIndexsWith(RectangleColliderVector3 rectPosV3)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        rectPosV3.GetMaxEnvelopeArea(out Vector2 envelopPos, out Vector2 envelopSize);
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
        // 判断地图信息是否初始化了
        if (this._mapGridConfig.colNum ==0) return false;
        // 判断是否重复注册
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

    /// <summary>
    /// 卸载一个已经注册的碰撞体
    /// </summary>
    /// <param name="collider"></param>
    public void UnRegisterGameCollider(GameCollider2D collider)
    {
        // 不包含时
        if (_mAllCollidersRecord.ContainsKey(collider) == false) return;

        var lastMapIndexs = _mAllCollidersRecord[collider];
        this._mAllCollidersRecord.Remove(collider);
        // clear collider info in map
        foreach (var mapIndex in lastMapIndexs)
        {
            _gameCollidersInMap[mapIndex].Remove(collider);
        }

    }

    /// <summary>
    /// 更新已经注册在地图中的碰撞体的位置信息
    /// </summary>
    /// <param name="updateCollider"></param>
    /// <param name="newAnchorPos"></param>
    /// <param name="newAnchorRotateAngle"></param>
    /// <param name="newScaleX"></param>
    /// <param name="newScaleY"></param>
    /// <param name="checkCollideAfterUpdate"></param>
    public void UpdateGameCollidersInMap(GameCollider2D updateCollider, Vector2 newAnchorPos, float newAnchorRotateAngle = 0, float newScaleX = 1, float newScaleY = 1, bool checkCollideAfterUpdate = true)
    {
        if (_mAllCollidersRecord.ContainsKey(updateCollider) == false) return;

        // 1 Remove last info in map
        var lastMapIndexs = _mAllCollidersRecord[updateCollider];
        foreach (int lastIndex in lastMapIndexs)
        {
            _gameCollidersInMap[lastIndex].Remove(updateCollider);
        }

        // 2 update collider new pos
        updateCollider.SetCollideRectPos(newAnchorPos,newAnchorRotateAngle,newScaleX,newScaleY);
        // 3 update info in map
        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(updateCollider);
        _mAllCollidersRecord[updateCollider] = estimatedMapIndexs;
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

                _gameCollidersInMap[mapIndex].Add(updateCollider);
            }
        }

        if (checkCollideAfterUpdate)
        {
            CheckCollideHappen(updateCollider);
        }
    }

    

    /// <summary>
    /// 检查已经在地图注册的碰撞体 是否触发碰撞
    /// </summary>
    /// <param name="checkColliderInMap"></param>
    public void CheckCollideHappen(GameCollider2D checkColliderInMap)
    {
        if (_mAllCollidersRecord.ContainsKey(checkColliderInMap) == false)
        {
            Debug.LogError("检查的碰撞体在地图中没有注册");
            return;
        }

        foreach (var mapIndex in _mAllCollidersRecord[checkColliderInMap])
        {
            var collidersInThisMapCell = _gameCollidersInMap[mapIndex];
            foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
            {
                // 是自己
                if (tgtCollider == checkColliderInMap) continue;
                if (checkColliderInMap.CheckCollapse(tgtCollider.RectanglePosv3))
                {
                    // 通知这个检查碰撞体
                    checkColliderInMap.OnColliderEnter(tgtCollider);
                    // 通知被检测者
                    tgtCollider.OnColliderEnter(checkColliderInMap);
                }
            }
        }
    }

    /// <summary>
    /// 快速检查某个不需要在地图中注册的碰撞体体矩形是否会触发碰撞
    /// </summary>
    /// <param name="initColliderData"></param>
    /// <param name="collideProcessor"></param>
    /// <param name="anchorPos"></param>
    /// <param name="anchorRotateAngle"></param>
    /// <param name="scaleX"></param>
    /// <param name="scaleY"></param>
    public void CheckCollideHappen(GameColliderData2D initColliderData, ICollideProcessor collideProcessor, Vector2 anchorPos, float anchorRotateAngle = 0, float scaleX = 1, float scaleY = 1)
    {
        var cheRectanglePos = new RectangleColliderVector3(initColliderData.offset,initColliderData.size,anchorPos,anchorRotateAngle,scaleX,scaleY);

        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(cheRectanglePos);
        if (estimatedMapIndexs!=null)
        {
            foreach (var mapIndex in estimatedMapIndexs)
            {
                var collidersInThisMapCell = _gameCollidersInMap[mapIndex];
                if (collidersInThisMapCell == null) continue;
                foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
                {
                    if (tgtCollider.CheckCollapse(cheRectanglePos))
                    {
                        // 通知这个检查碰撞体
                        collideProcessor.HandleCollideTo(tgtCollider.GetCollideProcessor());
                        // 通知被检测者
                        tgtCollider.GetCollideProcessor().HandleCollideTo(collideProcessor);
                    }
                }
            }
        }

    }



    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);

    }

    public override void Dispose()
    {
        _mAllCollidersRecord = null;
        _gameCollidersInMap = null;
    }


}
