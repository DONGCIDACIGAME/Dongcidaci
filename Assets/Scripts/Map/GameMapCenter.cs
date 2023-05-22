using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapCenter : ModuleManager<GameMapCenter>
{
    private Transform _mapRootNodeT;
    public Transform MapRootNodeT => _mapRootNodeT;


    public override void Initialize()
    {
        // 0 set map root node
        this._mapRootNodeT = GameObject.Find(GameDefine._MAP_ROOT).transform;

        // 1 从 datacenter 获取地图数据

        // 2 根据地图数据生成地图


    }


    

    public override void OnUpdate(float deltaTime)
    { 

    
    }

    public override void Dispose()
    {
        
    }



}
