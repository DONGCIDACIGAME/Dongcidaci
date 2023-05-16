using System.Collections;
using System.Collections.Generic;
using GameEngine;

/// <summary>
/// TODO: 这里要优化一下，根据地图划分块，做检测的时候只检测某个区域内的
/// 区块粒度要想一下
/// 划分依据应该是根据位置和包围盒大小
/// </summary>
public class GameColliderCenter : ModuleManager<GameColliderCenter>
{
    /// <summary>
    /// 所有区块的碰撞体
    /// </summary>
    private HashSet<GameCollider2D>[,] mAllGameColliders;

    /// <summary>
    /// 每个碰撞体所在的区块index
    /// key: unique hash value of collider
    /// value: index of map area
    /// 每个碰撞体位置变化时，需要把原来所在的区块里，这个碰撞体删除，然后根据位置重新算一下所在区块
    /// </summary>
    private Dictionary<int, List<int>> mAllCollidersRecord;

    public override void Initialize()
    {
        mAllCollidersRecord = new Dictionary<int, List<int>>();
    }

    /// <summary>
    /// 根据地图的信息重置所有的碰撞
    /// </summary>
    /// <param name="mapWidth"></param>
    /// <param name="mapHeight"></param>
    /// <param name="cellSize"></param>
    public void ResetWithMapInfo(int mapWidth, int mapHeight, int cellSize)
    {
        int colNum = (mapWidth % cellSize) == 0 ? (mapWidth / cellSize) : (mapWidth / cellSize) + 1;
        int rowNum = (mapHeight % cellSize) == 0 ? (mapHeight / cellSize) : (mapHeight / cellSize) + 1;

        mAllGameColliders = new HashSet<GameCollider2D>[colNum, rowNum];
        mAllCollidersRecord.Clear();
    }

    public void RegisterGameCollider(GameCollider2D collider)
    {
        // 1. 添加到所有对应的区块里 -- mAllGameColliders

        // 2. 更新这个碰撞体所在的区块信息--mAllCollidersRecord
    }

    public void UnRegisterGameCollider(GameCollider2D collider)
    {
        // 删除这个碰撞体所在的区块信息--mAllGameColliders，mAllCollidersRecord
    }

    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);


    }

    public override void Dispose()
    {
        mAllGameColliders = null;
        mAllCollidersRecord = null;
    }


}
