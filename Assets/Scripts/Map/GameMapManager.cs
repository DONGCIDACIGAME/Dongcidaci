using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMapManager : IMeterHandler
{
    //private MapGridInfo _mapGridConfig;
    private GameMapData _mapData;

    private List<MapGround> _mapGrounds;
    private List<MapBlock> _mapBlocks;
    
    /// <summary>
    /// 地图上需要响应节拍的物件
    /// </summary>
    private List<IMeterHandler> _mapMeterHandlers;

    private string mapBasePrefabPath = "Prefabs/Maps/Base/";


    public void Initialize(GameMapData loadedMapData)
    {
        if (loadedMapData == null)
        {
            Log.Error(LogLevel.Critical,"the map data is null");
        }
        this._mapData = loadedMapData;
        
        _mapGrounds = new List<MapGround>();
        _mapBlocks = new List<MapBlock>();
        _mapMeterHandlers = new List<IMeterHandler>();

    }

    public void LoadMap()
    {
        LoadMapBase();
        LoadMapEvents();
        
    }

    
    public bool GetNavRouteToPos(Vector3 tgtPos,out Vector3[] routes)
    {




        routes = new Vector3[0];
        return false;
    }





    private void LoadMapBase()
    {
        var basePrefabFullPath = mapBasePrefabPath + _mapData.mapBasePrefabName;
        var go = PrefabUtil.LoadPrefab(basePrefabFullPath, "Load MapBase Prefab");
        go.transform.SetParent(GameNodeCenter.Ins.BaseLayerNode.transform, false);

        //生成地面
        var groundNodeT = go.transform.Find("_GROUND_LAYER");
        if (groundNodeT != null)
        {
            if (groundNodeT.childCount > 0)
            {
                for (int i = 0; i < groundNodeT.childCount; i++)
                {
                    groundNodeT.GetChild(i).TryGetComponent<MapGroundView>(out MapGroundView groundView);
                    if (groundView != null)
                    {
                        this._mapGrounds.Add(new MapGround(groundView));
                    }
                }
            }
        }

        //生成block
        var blockNodeT = go.transform.Find("_BLOCK_LAYER");
        if (blockNodeT != null)
        {
            if (blockNodeT.childCount > 0)
            {
                for (int i = 0; i < blockNodeT.childCount; i++)
                {
                    blockNodeT.GetChild(i).TryGetComponent<MapBlockView>(out MapBlockView blockView);
                    if (blockView != null)
                    {
                        this._mapBlocks.Add(new MapBlock(blockView));
                        // 判断这个障碍物是否节拍事件
                        if(blockView is IMeterHandler)
                        {
                            _mapMeterHandlers.Add(blockView as IMeterHandler);
                        }
                    }
                }
            }
        }

        // 从dec 层获取需要更新的 IMeterHandler
        var decNodeT = go.transform.Find("_DECO_LAYER");
        
        if (decNodeT != null)
        {
            if (decNodeT.childCount > 0)
            {
                for (int i = 0; i < decNodeT.childCount; i++)
                {
                    decNodeT.GetChild(i).TryGetComponent<MapDecView>(out MapDecView decView);
                    if (decView != null)
                    {
                        // 判断这个障碍物是否节拍事件
                        if (decView is IMeterHandler)
                        {
                            _mapMeterHandlers.Add(decView as IMeterHandler);
                        }
                    }
                }
            }
        }

        
    }



    private void LoadMapEvents()
    {

    }

    public void OnGameUpdate(float deltaTime)
    {

    }

    public void OnLateUpdate(float deltaTime)
    {

    }

    public void OnMeterEnter(int meterIndex)
    {
        
    }

    public void OnMeterEnd(int meterIndex)
    {
        
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        if (_mapMeterHandlers.Count == 0) return;
        for (int i=0;i<_mapMeterHandlers.Count;i++)
        {
            _mapMeterHandlers[i].OnDisplayPointBeforeMeterEnter(meterIndex);
        }
    }

    public void Dispose()
    {
        _mapData = null;
        _mapGrounds = null;
        _mapBlocks = null;
    }
}
