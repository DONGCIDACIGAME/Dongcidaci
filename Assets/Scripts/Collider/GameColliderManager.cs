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

    /// <summary>
    /// 无限网格的配置，通过调整网格的尺寸优化性能
    /// </summary>
    private UnLimitedGrid _gridConfig;

    /// <summary>
    /// 碰撞体在无限网格中的坐标映射
    /// </summary>
    private Dictionary<int, HashSet<ValueTuple<int, int>>> _colliderToGridIndexsDict;

    public List<ConvexCollider2D> tempTestColliders;

    /// <summary>
    /// 处于某个地图坐标中的碰撞体
    /// </summary>
    private Dictionary<ValueTuple<int,int>,HashSet<ConvexCollider2D>> _gridIndexToCollidersDict;

    public override void Initialize()
    {
        _gridConfig = new UnLimitedGrid(6f,6f);
        _colliderToGridIndexsDict = new Dictionary<int, HashSet<(int, int)>>();
        _gridIndexToCollidersDict = new Dictionary<(int, int), HashSet<ConvexCollider2D>>();

        tempTestColliders = new List<ConvexCollider2D>();
        Log.Logic(LogLevel.Info, "GameColliderManager Initialize Completed");
    }

    /// <summary>
    /// 获取某个碰撞体的最大正投影矩形包络
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    private HashSet<ValueTuple<int, int>> GetMaxEnvelopeOccupyGridIndexs(ConvexCollider2D collider)
    {
        return GetMaxEnvelopeOccupyGridIndexs(collider.Convex2DShape);
    }

    /// <summary>
    /// 获取某个凸多边形的最大正投影矩形包络
    /// </summary>
    /// <param name="convexShape"></param>
    /// <returns></returns>
    private HashSet<ValueTuple<int, int>> GetMaxEnvelopeOccupyGridIndexs(IConvex2DShape convexShape)
    {
        // 获取最大的包络，通过最大矩形包络快速索引可能产生交差的地图块
        GameColliderHelper.GetRectMaxEnvelope(convexShape, out Vector2 envelopPos, out Vector2 envelopSize);

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
    public void RegisterGameCollider(ConvexCollider2D collider)
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

            tempTestColliders.Add(collider);
        }
        // 更新碰撞体所在的区域信息
        UpdateColliderInfoInGrid(collider);
    }

    /// <summary>
    /// 更新一个碰撞体的锚点信息
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="newAnchorPos"></param>
    public void UpdateColliderPos(ConvexCollider2D collider, Vector3 newAnchorPos)
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

    /// <summary>
    /// 更新一个碰撞体的锚点旋转信息
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="newAngle"></param>
    public void UpdateColliderRotateAngle(ConvexCollider2D collider, float newAngle)
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

    /// <summary>
    /// 更新一个碰撞体的缩放比例
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="newScale"></param>
    public void UpdateColliderScale(ConvexCollider2D collider, Vector3 newScale)
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

    /// <summary>
    /// 更新碰撞体在网格中的信息
    /// </summary>
    /// <param name="collider"></param>
    public void UpdateColliderInfoInGrid(ConvexCollider2D collider)
    {
        if(collider == null)
        {
            Log.Error(LogLevel.Normal, "UpdateColliderInfoInGrid Error, collider is null!");
            return;
        }

        int colliderID = collider.GetColliderUID();
        if (_colliderToGridIndexsDict.ContainsKey(colliderID) == false)
        {
            Log.Error(LogLevel.Normal, "UpdateColliderInfoInGrid Error, collider not registered!");
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
                    _gridIndexToCollidersDict[gridIndex] = new HashSet<ConvexCollider2D>();
                }
                _gridIndexToCollidersDict[gridIndex].Add(collider);
            }
            else
            {
                // 新的地图索引
                _gridIndexToCollidersDict.Add(gridIndex, new HashSet<ConvexCollider2D>() { collider });
            }
        }

    }

    /// <summary>
    /// 从管理器删除碰撞
    /// </summary>
    /// <param name="collider"></param>
    /// <returns></returns>
    public bool UnRegisterGameCollider(ConvexCollider2D collider)
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

        tempTestColliders.Remove(collider);
        return true;
    }

    /// <summary>
    /// 检测某个形状是否会产生碰撞;
    /// 返回碰撞的对象和它的分离向量无序
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="tgtsWithLeaveV2Dict"></param>
    /// <param name="exceptCollider"></param>
    /// <returns></returns>
    public bool CheckCollideHappenWithShape(IConvex2DShape shape, out Dictionary<ConvexCollider2D,Vector2> tgtsWithLeaveV2Dict, ConvexCollider2D exceptCollider)
    {
        var tempIndexs = GetMaxEnvelopeOccupyGridIndexs(shape);
        tgtsWithLeaveV2Dict = new Dictionary<ConvexCollider2D, Vector2>();
        if (tempIndexs != null)
        {
            foreach (var mapIndex in tempIndexs)
            {
                if (_gridIndexToCollidersDict.ContainsKey(mapIndex) == false)
                {
                    continue;
                }

                var collidersInThisGrid = _gridIndexToCollidersDict[mapIndex];
                if (collidersInThisGrid == null) continue;

                foreach (ConvexCollider2D tgtCollider in collidersInThisGrid)
                {
                    if (exceptCollider != null)
                    {
                        if (tgtCollider.GetColliderUID() == exceptCollider.GetColliderUID()) continue;
                    }
                    
                    if (GameColliderHelper.CheckCollideSATWithLeaveVector(shape,tgtCollider.Convex2DShape,out Vector2 leaveV2))
                    {
                        if(tgtsWithLeaveV2Dict.ContainsKey(tgtCollider) == false)
                        {
                            tgtsWithLeaveV2Dict.Add(tgtCollider, leaveV2);
                        }
                        
                    }

                }
            }
        }

        if (tgtsWithLeaveV2Dict.Count > 0) return true;
        return false;

    }


    /// <summary>
    /// 检测某个形状是否会产生碰撞;
    /// 返回碰撞的对象按照距离 shape 的 anchor pos 进行排序
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="detectedColliders"></param>
    /// <param name="exceptCollider"></param>
    /// <returns></returns>
    public bool CheckCollideHappenWithShape(IConvex2DShape shape, out List<ConvexCollider2D> detectedColliders, ConvexCollider2D exceptCollider)
    {
        var tempIndexs = GetMaxEnvelopeOccupyGridIndexs(shape);
        detectedColliders = new List<ConvexCollider2D>();
        if (tempIndexs != null)
        {
            foreach (var mapIndex in tempIndexs)
            {
                if (_gridIndexToCollidersDict.ContainsKey(mapIndex) == false)
                {
                    continue;
                }

                var collidersInThisGrid = _gridIndexToCollidersDict[mapIndex];
                if (collidersInThisGrid == null) continue;

                foreach (ConvexCollider2D tgtCollider in collidersInThisGrid)
                {
                    if (exceptCollider != null)
                    {
                        if (tgtCollider.GetColliderUID() == exceptCollider.GetColliderUID()) continue;
                    }

                    if (GameColliderHelper.CheckCollideSAT(shape, tgtCollider.Convex2DShape))
                    {
                        if (detectedColliders.Contains(tgtCollider) == false)
                        {
                            if (detectedColliders.Count == 0)
                            {
                                detectedColliders.Add(tgtCollider);
                            }
                            else if(detectedColliders.Count>0)
                            {
                                int insertIndex = -1;

                                for (int i=0;i< detectedColliders.Count;i++)
                                {
                                    var crtDis = (detectedColliders[i].Convex2DShape.AnchorPos - shape.AnchorPos).magnitude;
                                    var tgtDis = (tgtCollider.Convex2DShape.AnchorPos - shape.AnchorPos).magnitude;
                                    if(tgtDis<= crtDis)
                                    {
                                        insertIndex = i;
                                        break;
                                    }
                                }

                                if (insertIndex >=0)
                                {
                                    detectedColliders.Insert(insertIndex, tgtCollider);
                                }else if (insertIndex == -1)
                                {
                                    detectedColliders.Add(tgtCollider);
                                }
                                
                            }
                            
                        }

                    }

                }
            }
        }

        if (detectedColliders.Count > 0) return true;
        return false;

    }



    /// <summary>
    /// 判断某个碰撞体是否会产生碰撞，并处理碰撞
    /// </summary>
    /// <param name="srcCollider"></param>
    /// <param name="handleCfg"></param>
    /// <returns></returns>
    public bool CheckCollideHappen(ConvexCollider2D srcCollider, CollideHandleConfig handleCfg = CollideHandleConfig.HandleBoth)
    {
        if (srcCollider == null) return false;

        int srcUID = srcCollider.GetColliderUID();
        if (_colliderToGridIndexsDict.ContainsKey(srcUID))
        {
            Log.Logic(LogLevel.Normal, "CheckCollideHappen, srcCollider is registered!");
        }

        int tgtCounter = 0;
        foreach (var mapIndex in _colliderToGridIndexsDict[srcUID])
        {
            var collidersInGrid = _gridIndexToCollidersDict[mapIndex];
            foreach (ConvexCollider2D tgtCollider in collidersInGrid)
            {
                // 是自己
                if (tgtCollider.GetColliderUID() == srcUID) continue;

                if (GameColliderHelper.CheckCollideSAT(srcCollider.Convex2DShape, tgtCollider.Convex2DShape))
                {
                    switch (handleCfg)
                    {
                        case CollideHandleConfig.HandleBoth:
                            ExcuteOnColliderHappen(srcCollider, tgtCollider);
                            ExcuteOnColliderHappen(tgtCollider, srcCollider);
                            break;
                        case CollideHandleConfig.HandleSrc:
                            ExcuteOnColliderHappen(srcCollider, tgtCollider);
                            break;
                        case CollideHandleConfig.HandleTgt:
                            ExcuteOnColliderHappen(tgtCollider, srcCollider);
                            break;
                        case CollideHandleConfig.NoHandle:
                            continue;
                        default:
                            continue;
                    }

                    tgtCounter += 1;
                }
            }
        }

        if (tgtCounter > 0) return true;
        return false;
    }


    private void ExcuteOnColliderHappen(ConvexCollider2D srcCollider, ConvexCollider2D tgtCollider)
    {
        var srcEntityID = srcCollider.GetBindEntityId();
        var tgtEntityID = tgtCollider.GetBindEntityId();
        if (srcEntityID == 0 || tgtEntityID == 0)
        {
            Log.Error(LogLevel.Normal, "两个碰撞体中存在无Entity的情况");
            return;
        }

        var srcEntity = EntityManager.Ins.GetEntity(srcEntityID);
        if (srcEntity == null)
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

        CollideHandleCenter.HandleCollideFromSrcToTgt(srcEntity, tgtEntity);
    }




    /**
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
    */



    public override void Dispose()
    {
        _colliderToGridIndexsDict = null;
        _gridIndexToCollidersDict = null;
        tempTestColliders = null;
    }


}
