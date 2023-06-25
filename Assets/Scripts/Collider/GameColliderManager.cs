using System.Collections.Generic;
using GameEngine;
using UnityEngine;
using System;
using System.Linq;


// 1 生成无限网格，将地图切分成虚拟的大块
// 2 注册新的碰撞体时，计算这个碰撞体占据的地图大块的横纵索引并存储到字典中,包含这个碰撞体覆盖的坐标，地图坐标包含的碰撞体
// 3 判断碰撞时，计算这个碰撞体占据的地图大块横纵坐标，从字典中快速找到这几个大块中的所有碰撞体进行判断
public class GameColliderManager : ModuleManager<GameColliderManager>,IColliderSetter
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
        Log.Logic(LogLevel.Info, "GameColliderManager Initialize Completed");
    }

    private HashSet<ValueTuple<int, int>> GetMaxEnvelopeOccupyGridIndexs(GameCollider2D collider)
    {
        return GetMaxEnvelopeOccupyGridIndexs(collider.AnchorPos,collider.AnchorAngle,collider.Offset,collider.Size);
    }

    private HashSet<ValueTuple<int, int>> GetMaxEnvelopeOccupyGridIndexs(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        GameColliderHelper.GetMaxEnvelopeArea(anchorPos, anchorAngle, offset, size, out Vector2 envelopPos, out Vector2 envelopSize);

        float minEnvelopX = envelopPos.x - envelopSize.x / 2f;
        float maxEnvelopX = envelopPos.x + envelopSize.x / 2f;
        int startColIndex = Mathf.RoundToInt(minEnvelopX / _gridConfig.cellWidth);
        int endColIndex = Mathf.RoundToInt(maxEnvelopX / _gridConfig.cellWidth);

        float minEnvelopY = envelopPos.y - envelopSize.y / 2f;
        float maxEnvelopY = envelopPos.y + envelopSize.y / 2f;
        int startRowIndex = Mathf.RoundToInt(minEnvelopY / _gridConfig.cellHeight);
        int endRowIndex = Mathf.RoundToInt(maxEnvelopY / _gridConfig.cellHeight);

        var indexs = new HashSet<ValueTuple<int, int>>();
        for (int i = startColIndex; i <= endColIndex; i++)
        {
            for (int j = startRowIndex; j <= endRowIndex; j++)
            {
                indexs.Add((i, j));
            }
        }

        return indexs;
    }

    /// <summary>
    /// 向管理器注册碰撞
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public void RegisterGameCollider(GameCollider2D collider)
    {
        if(collider == null)
        {
            Log.Error(LogLevel.Normal, "RegisterGameCollider Error, collider is null!");
            return;
        }

        int colliderId = collider.GetColliderUID();

        // 首次注册
        if (_colliderToGridIndexsDict.ContainsKey(colliderId) == false)
        {
            _colliderToGridIndexsDict.Add(colliderId, new HashSet<(int, int)>());
        }

        // 更新碰撞体所在的区域信息
        UpdateColliderInfoInGrid(collider);
    }

    public void UpdateColliderPos(GameCollider2D collider, Vector3 newAnchorPos)
    {
        if (collider == null) return;

        var colliderUID = collider.GetColliderUID();
        collider.UpdateColliderPos(this,newAnchorPos);
        if(_colliderToGridIndexsDict.ContainsKey(colliderUID))
        {
            // 需要同步更新在网格中的信息
            UpdateColliderInfoInGrid(collider);
        }
    }

    public void UpdateColliderRotateAngle(GameCollider2D collider, float newAngle)
    {
        if (collider == null) return;

        var colliderUID = collider.GetColliderUID();
        collider.UpdateColliderRotateAngle(this, newAngle);
        if (_colliderToGridIndexsDict.ContainsKey(colliderUID))
        {
            // 需要同步更新在网格中的信息
            UpdateColliderInfoInGrid(collider);
        }
    }

    public void UpdateColliderScale(GameCollider2D collider, Vector3 newScale)
    {
        if (collider == null) return;

        var colliderUID = collider.GetColliderUID();
        collider.UpdateColliderScale(this, newScale);
        if (_colliderToGridIndexsDict.ContainsKey(colliderUID))
        {
            // 需要同步更新在网格中的信息
            UpdateColliderInfoInGrid(collider);
        }
    }

    public void UpdateColliderInfoInGrid(GameCollider2D collider)
    {
        if(collider == null)
        {
            Log.Error(LogLevel.Normal, "UpdateMapCollider Error, collider is null!");
            return;
        }

        int colliderID = collider.GetColliderUID();
        if (_colliderToGridIndexsDict.ContainsKey(colliderID) == false)
        {
            return;
        }

        // 1 清空 grid index to colliders 中存储的 这个碰撞体的旧信息
        foreach (var gridIndex in _colliderToGridIndexsDict[colliderID])
        {
            _gridIndexToCollidersDict[gridIndex].Remove(collider);
        }

        // 2 计算新的网格索引
        var colliderGridIndexs = _colliderToGridIndexsDict[colliderID] = GetMaxEnvelopeOccupyGridIndexs(collider);

        // 3 在网格索引中放入这个碰撞体
        foreach (var gridIndex in colliderGridIndexs)
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
                _gridIndexToCollidersDict.Add(gridIndex, new HashSet<GameCollider2D>() { collider });
            }
        }

    }

    /// <summary>
    /// 从管理器删除碰撞
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public bool UnRegisterGameCollider(GameCollider2D collider)
    {
        int colliderId = collider.GetColliderUID();

        // 不包含时
        if (_colliderToGridIndexsDict.ContainsKey(colliderId) == false) return false;

        // 清空 grid index to colliders 中存储的信息
        foreach (var gridIndex in _colliderToGridIndexsDict[colliderId])
        {
            _gridIndexToCollidersDict[gridIndex].Remove(collider);
            // 如果这个网格中已经没有碰撞了，清空这个 key value pair
            // 如果网格中没有碰撞体，不删除，因为又可能其它的碰撞体注册进来
            //if (_gridIndexToCollidersDict[gridIndex].Count == 0) _gridIndexToCollidersDict.Remove(gridIndex);
        }

        // 清空 collide to grid indexs
        _colliderToGridIndexsDict.Remove(colliderId);
        return true;
    }


    public bool CheckFirstCollideHappenWithRect(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size, out GameCollider2D firstCollider, GameCollider2D exceptCollider)
    {
        firstCollider = null;
        if (CheckCollideHappenWithRect(anchorPos,anchorAngle,offset,size,out HashSet<GameCollider2D>  allColliders,exceptCollider) == false)
        {
            return false;
        }

        // 从allColliders中查找第一个碰到的
        if(allColliders.Count == 1)
        {
            firstCollider = allColliders.First<GameCollider2D>();
            return true;
        }
        else
        {

        }

        return true;

    }

    public bool CheckCollideHappenWithRect(Vector3 anchorPos, float anchorAngle, Vector2 offset, Vector2 size, out HashSet<GameCollider2D> detectedColliders, GameCollider2D exceptCollider)
    {
        var tempIndexs = GetMaxEnvelopeOccupyGridIndexs(anchorPos,anchorAngle,offset,size);
        detectedColliders = new HashSet<GameCollider2D>();
        if (tempIndexs != null)
        {
            foreach (var mapIndex in tempIndexs)
            {
                if(_gridIndexToCollidersDict.ContainsKey(mapIndex) == false)
                {
                    continue;
                }

                var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];
                if (collidersInThisMapCell == null) continue;

                foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
                {
                    if (tgtCollider.GetColliderUID() == exceptCollider.GetColliderUID()) continue;

                    if (GameColliderHelper.CheckCollapse(anchorPos, anchorAngle, offset, size, tgtCollider))
                    {
                        detectedColliders.Add(tgtCollider);
                    }
                    
                }
            }
        }

        if (detectedColliders.Count > 0) return true;
        return false;

    }





    public bool CheckCollideHappenWithUnRegistered(GameCollider2D checkCollider, out HashSet<MyColliderType> detectedColliderTypes, CollideTriggerConfig triggerCfg = CollideTriggerConfig.TriggerBoth)
    {
        detectedColliderTypes = new HashSet<MyColliderType>();
        int checkColliderID = checkCollider.GetColliderUID();
        if (_colliderToGridIndexsDict.ContainsKey(checkColliderID) == true)
        {
            Log.Error(LogLevel.Normal, "CheckCollideHappenWithRegistered Error, checkCollider is registered!");
            return false;
        }
        //!!

        return false;
    }



    public bool CheckCollideHappenWithRegistered(GameCollider2D checkCollider, out HashSet<MyColliderType> detectedColliderTypes, CollideTriggerConfig triggerCfg = CollideTriggerConfig.TriggerBoth)
    {
        detectedColliderTypes = new HashSet<MyColliderType>();
        int checkColliderID = checkCollider.GetColliderUID();
        if (_colliderToGridIndexsDict.ContainsKey(checkColliderID) == false)
        {
            Log.Error(LogLevel.Normal, "CheckCollideHappenWithRegistered Error, checkCollider not registered!");
            return false;
        }

        foreach (var mapIndex in _colliderToGridIndexsDict[checkColliderID])
        {
            var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];
            foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
            {
                // 是自己
                if (tgtCollider.GetColliderUID() == checkColliderID) continue;

                if(GameColliderHelper.CheckCollapse(checkCollider, tgtCollider))
                {
                    detectedColliderTypes.Add(tgtCollider.GetColliderType());

                    switch (triggerCfg)
                    {
                        case CollideTriggerConfig.TriggerBoth:
                            ExcuteOnColliderHappen(checkCollider, tgtCollider);
                            ExcuteOnColliderHappen(tgtCollider, checkCollider);
                            break;
                        case CollideTriggerConfig.TriggerSrc:
                            ExcuteOnColliderHappen(checkCollider, tgtCollider);
                            break;
                        case CollideTriggerConfig.TriggerTgt:
                            ExcuteOnColliderHappen(tgtCollider, checkCollider);
                            break;
                        case CollideTriggerConfig.NotTrigger:
                            continue;
                        default:
                            continue;
                    }
                    
                }
            }
        }

        if (detectedColliderTypes.Count > 0) return true;
        return false;
    }



    /**
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
    public int CheckCollideHappen(Vector2 size, Vector2 offset, 
        Vector2 anchorPos, float anchorRotateAngle, Vector3 scale,
        IGameColliderHandler collideHandler, int exceptColliderId)
    {
        int colliderTypes = GameColliderDefine.CollliderType_None;

        RecalcRoundOccupyMapIndexsWith(
            size,
            offset, 
            anchorRotateAngle,
            anchorPos,
            scale,
            ref _tempIndexs);

        if (_tempIndexs != null)
        {
            foreach (var mapIndex in _tempIndexs)
            {
                var collidersInThisMapCell = _gridIndexToCollidersDict[mapIndex];

                if (collidersInThisMapCell == null) 
                    continue;

                foreach (GameCollider2D tgtCollider in collidersInThisMapCell)
                {
                    if (tgtCollider.GetColliderId() == exceptColliderId)
                        continue;

                    if (GameColliderHelper.CheckCollapse(
                            size,
                            offset,
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
    */

    private void ExcuteOnColliderHappen(GameCollider2D srcCollider, GameCollider2D tgtCollider)
    {
        var srcEntityID = srcCollider.GetBindEntityId();
        var tgtEntityID = tgtCollider.GetBindEntityId();
        if (srcEntityID == 0|| tgtEntityID == 0)
        {
            Log.Error(LogLevel.Normal,"两个碰撞体中存在无Entity的情况");
            return;
        }

        var srcEntity = EntityManager.Ins.GetEntity(srcEntityID);
        if(srcEntity == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteOnColliderHappen Error, can not find src collider's entity!");
            return;
        }

        var tgtEntity = EntityManager.Ins.GetEntity(tgtEntityID);
        if (tgtEntity == null)
        {
            Log.Error(LogLevel.Normal, "ExcuteOnColliderHappen Error, can not find tgt collider's entity!");
            return;
        }

        CollideHandleCenter.HandleCollideFromSrcToTgt(srcEntity,tgtEntity);
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
