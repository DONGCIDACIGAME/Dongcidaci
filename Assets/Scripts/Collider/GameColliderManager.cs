using System.Collections.Generic;
using GameEngine;
using UnityEngine;
using System;

// 1 生成无限网格，将地图切分成虚拟的大块
// 2 注册新的碰撞体时，计算这个碰撞体占据的地图大块的横纵索引并存储到字典中,包含这个碰撞体覆盖的坐标，地图坐标包含的碰撞体
// 3 判断碰撞时，计算这个碰撞体占据的地图大块横纵坐标，从字典中快速找到这几个大块中的所有碰撞体进行判断

public class GameColliderManager : ModuleManager<GameColliderManager>
{
    private struct UnLimitedGrid
    {
        public float cellWidth;
        public float cellHeight;
        public UnLimitedGrid(float width,float height)
        {
            this.cellHeight = height;
            this.cellWidth = width;
        }
    }

    private UnLimitedGrid _gridConfig;

    private Dictionary<GameCollider2D, HashSet<ValueTuple<int, int>>> _colliderToGridIndexsDict;

    private Dictionary<ValueTuple<int,int>,HashSet<GameCollider2D>> _gridIndexToCollidersDict;

    public override void Initialize()
    {
        _gridConfig = new UnLimitedGrid(6f,6f);
        _colliderToGridIndexsDict = new Dictionary<GameCollider2D, HashSet<(int, int)>>();
        _gridIndexToCollidersDict = new Dictionary<(int, int), HashSet<GameCollider2D>>();
    }

    private HashSet<ValueTuple<int,int>> GetRoundOccupyMapIndexsWith(GameCollider2D collider)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        return GetRoundOccupyMapIndexsWith(collider.RectanglePosv3);
    }

    private HashSet<ValueTuple<int, int>> GetRoundOccupyMapIndexsWith(RectangleColliderVector3 rectPosV3)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        rectPosV3.GetMaxEnvelopeArea(out Vector2 envelopPos, out Vector2 envelopSize);
        float minEnvelopX = envelopPos.x - envelopSize.x / 2f;
        float maxEnvelopX = envelopPos.x + envelopSize.x / 2f;
        int startColIndex = Mathf.RoundToInt(minEnvelopX / _gridConfig.cellWidth);
        int endColIndex = Mathf.RoundToInt(maxEnvelopX / _gridConfig.cellWidth);

        float minEnvelopY = envelopPos.y - envelopSize.y / 2f;
        float maxEnvelopY = envelopPos.y + envelopSize.y / 2f;
        int startRowIndex = Mathf.RoundToInt(minEnvelopY / _gridConfig.cellHeight);
        int endRowIndex = Mathf.RoundToInt(maxEnvelopY / _gridConfig.cellHeight);

        var results = new HashSet<ValueTuple<int, int>>();
        for (int i = startColIndex;i<=endColIndex;i++)
        {
            for (int j = startRowIndex;j<=endRowIndex;j++)
            {
                results.Add((i,j));
            }
        }

        return results;
    }


    public bool RegisterGameCollider(GameCollider2D collider)
    {
        // 判断是否重复注册
        if (_colliderToGridIndexsDict.ContainsKey(collider)) return false;
        
        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider);
        if (estimatedMapIndexs.Count!=0)
        {
            _colliderToGridIndexsDict.Add(collider, estimatedMapIndexs);

            foreach (var gridIndex in estimatedMapIndexs)
            {
                if (_gridIndexToCollidersDict.ContainsKey(gridIndex))
                {
                    if (_gridIndexToCollidersDict[gridIndex] == null)
                    {
                        _gridIndexToCollidersDict[gridIndex] = new HashSet<GameCollider2D>();
                    }
                    _gridIndexToCollidersDict[gridIndex].Add(collider);
                }
                else
                {
                    // 新的地图索引
                    _gridIndexToCollidersDict.Add(gridIndex, new HashSet<GameCollider2D>() { collider});
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    
    public bool UnRegisterGameCollider(GameCollider2D collider)
    {
        // 不包含时
        if (_colliderToGridIndexsDict.ContainsKey(collider) == false) return false;

        // 清空 grid index to colliders 中存储的信息
        foreach (var gridIndex in _colliderToGridIndexsDict[collider])
        {
            _gridIndexToCollidersDict[gridIndex].Remove(collider);
            //如果这个地图块中已经没有碰撞了，清空这个 key value pair
            if (_gridIndexToCollidersDict[gridIndex].Count == 0) _gridIndexToCollidersDict.Remove(gridIndex);
        }

        // 清空 collide to grid indexs
        _colliderToGridIndexsDict.Remove(collider);

        return true;
    }

    
    public void UpdateGameCollidersInMap(GameCollider2D updateCollider, Vector2 newAnchorPos, float newAnchorRotateAngle = 0, float newScaleX = 1, float newScaleY = 1, bool checkCollideAfterUpdate = true)
    {
        // 1 unregister the collider
        if (UnRegisterGameCollider(updateCollider) == false) return;
        // 2 update collider new pos
        updateCollider.SetCollideRectPos(newAnchorPos,newAnchorRotateAngle,newScaleX,newScaleY);
        // 3 register the collider into map
        if (RegisterGameCollider(updateCollider) == false) return;
        // 4 check collider
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
        Log.Logic(LogLevel.Info, "CheckCollideHappen---{0}", checkColliderInMap.GetHashCode());

        if (_colliderToGridIndexsDict.ContainsKey(checkColliderInMap) == false)
        {
            Debug.LogError("检查的碰撞体在地图中没有注册");
            return;
        }

        foreach (var mapIndex in _colliderToGridIndexsDict[checkColliderInMap])
        {
            var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];
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
                var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];
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
        _colliderToGridIndexsDict = null;
        _gridIndexToCollidersDict = null;
    }


}
