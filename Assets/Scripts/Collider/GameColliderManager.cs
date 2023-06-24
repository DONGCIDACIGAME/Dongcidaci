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

    private Dictionary<int, HashSet<ValueTuple<int, int>>> _colliderToGridIndexsDict;

    private Dictionary<ValueTuple<int,int>,HashSet<GameCollider2D>> _gridIndexToCollidersDict;

    public override void Initialize()
    {
        _gridConfig = new UnLimitedGrid(6f,6f);
        _colliderToGridIndexsDict = new Dictionary<int, HashSet<(int, int)>>();
        _gridIndexToCollidersDict = new Dictionary<(int, int), HashSet<GameCollider2D>>();
        Debug.Log("碰撞管理器初始化完成--------");
    }

    private HashSet<ValueTuple<int, int>> GetRoundOccupyMapIndexsWith(Vector2 size, Vector2 offset, float anchorAngle, Vector3 anchorPos, Vector3 scale)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        GameColliderHelper.GetMaxEnvelopeArea(size, offset, anchorAngle, anchorPos, scale, out Vector2 envelopPos, out Vector2 envelopSize);

        float minEnvelopX = envelopPos.x - envelopSize.x / 2f;
        float maxEnvelopX = envelopPos.x + envelopSize.x / 2f;
        int startColIndex = Mathf.RoundToInt(minEnvelopX / _gridConfig.cellWidth);
        int endColIndex = Mathf.RoundToInt(maxEnvelopX / _gridConfig.cellWidth);

        float minEnvelopY = envelopPos.y - envelopSize.y / 2f;
        float maxEnvelopY = envelopPos.y + envelopSize.y / 2f;
        int startRowIndex = Mathf.RoundToInt(minEnvelopY / _gridConfig.cellHeight);
        int endRowIndex = Mathf.RoundToInt(maxEnvelopY / _gridConfig.cellHeight);

        var results = new HashSet<ValueTuple<int, int>>();
        for (int i = startColIndex; i <= endColIndex; i++)
        {
            for (int j = startRowIndex; j <= endRowIndex; j++)
            {
                results.Add((i, j));
            }
        }

        return results;
    }


    public bool RegisterGameCollider(GameCollider2D collider)
    {
        if(collider == null)
        {
            Log.Error(LogLevel.Normal, "RegisterGameCollider Error, collider is null!");
            return false;
        }

        // 判断是否重复注册
        int colliderId = collider.GetColliderId();
        if (_colliderToGridIndexsDict.ContainsKey(colliderId)) return false;

       
        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(collider.size, collider.offset, collider.anchorAngle, collider.anchorPos, collider.scale);
        if (estimatedMapIndexs.Count!=0)
        {
            _colliderToGridIndexsDict.Add(colliderId, estimatedMapIndexs);

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

        Log.Logic(LogLevel.Info,"注册了新的碰撞体" + collider.GetHashCode());
        return true;
    }

    
    public bool UnRegisterGameCollider(GameCollider2D collider)
    {
        int colliderId = collider.GetColliderId();

        // 不包含时
        if (!_colliderToGridIndexsDict.ContainsKey(colliderId))
            return false;

        // 清空 grid index to colliders 中存储的信息
        foreach (var gridIndex in _colliderToGridIndexsDict[colliderId])
        {
            _gridIndexToCollidersDict[gridIndex].Remove(collider);
            //如果这个地图块中已经没有碰撞了，清空这个 key value pair
            if (_gridIndexToCollidersDict[gridIndex].Count == 0) _gridIndexToCollidersDict.Remove(gridIndex);
        }

        // 清空 collide to grid indexs
        _colliderToGridIndexsDict.Remove(colliderId);

        return true;
    }

    public void UpdateGameCollidersInMap(GameCollider2D updateCollider)
    {
        Debug.Log("更新碰撞体的信息");
        // 1 unregister the collider
        if (UnRegisterGameCollider(updateCollider) == false) return;
        // 2 register the collider into map
        if (RegisterGameCollider(updateCollider) == false) return;
    }


    public void UpdateGameCollidersInMap(GameCollider2D updateCollider, Vector2 newAnchorPos, float newAnchorRotateAngle = 0, float newScaleX = 1, float newScaleY = 1, bool checkCollideAfterUpdate = true)
    {
        Debug.Log("更新碰撞体的信息");
        // 1 unregister the collider
        if (UnRegisterGameCollider(updateCollider) == false) return;
        // 2 update collider new pos
        //updateCollider.UpdateCollider2DInfo(newAnchorPos, newAnchorRotateAngle, newScaleX,newScaleY);
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
    /// <param name="checkCollider2D"></param>
    public int CheckCollideHappen(GameCollider2D checkCollider2D)
    {
        //Log.Logic(LogLevel.Info, "CheckCollideHappen---{0}", checkColliderInMap.GetHashCode());

        if (_colliderToGridIndexsDict.ContainsKey(checkCollider2D.GetColliderId()) == false)
        {
            Debug.LogError("检查的碰撞体在地图中没有注册");
            return GameColliderDefine.CollliderType_None;
        }

        int colliderTypes = GameColliderDefine.CollliderType_None;
        int checkColliderEntityId = checkCollider2D.GetBindEntityId();

        foreach (var mapIndex in _colliderToGridIndexsDict[checkCollider2D.GetBindEntityId()])
        {
            var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];
            foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
            {
                // 是自己
                if (tgtCollider.GetBindEntityId() == checkColliderEntityId) 
                    continue;

                if(GameColliderHelper.CheckCollapse(checkCollider2D, tgtCollider))
                //if (checkCollider2D.CheckCollapse(tgtCollider))
                {

                    //Log.Logic(LogLevel.Info, "OnCollapse--{0}", (EntityManager.Ins.GetEntity(tgtCollider.GetBindEntityId()) as MapEntityWithCollider).GetPosition());
                    colliderTypes |= tgtCollider.GetColliderType();

                    // 检测方对被检测方的碰撞逻辑
                    ExcuteOnColliderHappen(checkCollider2D.GetColliderHandler(), tgtCollider.GetBindEntityId());

                    // 被检测方对检测方的碰撞逻辑
                    ExcuteOnColliderHappen(tgtCollider.GetColliderHandler(), checkCollider2D.GetBindEntityId());
                }
            }
        }

        return colliderTypes;
    }


    /// <summary>
    /// 快速检查某个不需要在地图中注册的碰撞体体矩形是否会触发碰撞
    /// </summary>
    /// <param name="colliderType">碰撞体类型</param>
    /// <param name="entityId">碰撞体绑定的entityId</param>
    /// <param name="initColliderData">碰撞范围数据</param>
    /// <param name="collideHandler">碰撞处理</param>
    /// <param name="anchorPos">碰撞体位置</param>
    /// <param name="anchorRotateAngle">碰撞体角度</param>
    /// <param name="scaleX">碰撞体缩放-x</param>
    /// <param name="scaleY">碰撞体缩放-y</param>
    public int CheckCollideHappen(GameColliderData2D initColliderData,IGameColliderHandler collideHandler,
        Vector2 anchorPos, float anchorRotateAngle, Vector3 scale)
    {
        int colliderTypes = GameColliderDefine.CollliderType_None;

        var estimatedMapIndexs = GetRoundOccupyMapIndexsWith(
            initColliderData.size,
            initColliderData.offset, 
            anchorRotateAngle,
            anchorPos,
            scale);

        if (estimatedMapIndexs!=null)
        {
            foreach (var mapIndex in estimatedMapIndexs)
            {
                var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];

                if (collidersInThisMapCell == null) 
                    continue;

                foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
                {
                    if (GameColliderHelper.CheckCollapse(initColliderData.size,
                            initColliderData.offset,
                            anchorRotateAngle,
                            anchorPos,
                            scale, tgtCollider))
                    {
                        colliderTypes |= tgtCollider.GetColliderType();

                        // 检测方对被检测方的碰撞逻辑
                        ExcuteOnColliderHappen(collideHandler, tgtCollider.GetBindEntityId());
                    }
                }
            }
        }

        return colliderTypes;
    }

    private void ExcuteOnColliderHappen(IGameColliderHandler handler, int colliderToEntityId)
    {
        // 如果目标碰撞不是一个实体，就不需要使用handler对这次碰撞做处理
        if (colliderToEntityId == 0)
            return;

        if (handler == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteOnColliderHappen Error, handler is null!");
            return;
        }

        Entity entity = EntityManager.Ins.GetEntity(colliderToEntityId);
        if (entity == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteOnColliderHappen Error, can not find collider to entity!");
            return;
        }

        if (entity is Hero)
        {
            handler.HandleColliderToHero(entity as Hero);
        }
        else if (entity is Monster)
        {
            handler.HandleColliderToMonster(entity as Monster);
        }
    }


    public override void Dispose()
    {
        _colliderToGridIndexsDict = null;
        _gridIndexToCollidersDict = null;
    }
}
