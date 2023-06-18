using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapCenter : ModuleManager<GameMapCenter>
{
    public GameObject MapRootNode { get { return GameObject.Find(GameDefine._MAP_ROOT); } }
    public GameObject GroundLayerNode { get { return GameObject.Find("_SCENE/_MAP/_GROUND_LAYER"); } }
    public GameObject BlockLayerNode { get { return GameObject.Find("_SCENE/_MAP/_BLOCK_LAYER"); } }
    public GameObject EventLayerNode { get { return GameObject.Find("_SCENE/_MAP/_EVENT_LAYER"); } }
    public GameObject DecoLayerNode { get { return GameObject.Find("_SCENE/_MAP/_DECO_LAYER"); } }

    private MapGridInfo _mapGridConfig;

    /// <summary>
    /// 获取某个点的地图格子索引
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetMapGridIndexWithPoint(Vector2 pos)
    {
        return this._mapGridConfig.GetMapIndex(pos);
    }


    public override void Initialize()
    {
        
        // 1 从 datacenter 获取地图数据


        // 2 生成 map grid


        // 3 根据地图数据生成地图


    }


    

    public override void OnUpdate(float deltaTime)
    { 

    
    }

    public override void Dispose()
    {
        
    }

    

}
