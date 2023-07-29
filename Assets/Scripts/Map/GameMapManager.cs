using GameEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameMapManager : ModuleManager<GameMapManager>
{
    public GameObject MapRootNode { get { return GameObject.Find(GameDefine._MAP_ROOT); } }
    public GameObject BaseLayerNode { get { return GameObject.Find("_SCENE/_MAP/_BASE_LAYER"); } }
    public GameObject EventLayerNode { get { return GameObject.Find("_SCENE/_MAP/_EVENT_LAYER"); } }

    private MapGridInfo _mapGridConfig;
    private GameMapData _mapData;
    private List<MapGround> _mapGrounds;
    private List<MapBlock> _mapBlocks;

    private MavMeshOnLoad _navMeshLoad;


    private string mapBasePrefabPath = "Prefabs/Maps/Base/";

    /// <summary>
    /// 获取某个点的地图格子索引
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetMapGridIndexWithPoint(Vector3 WorldPos)
    {
        return this._mapGridConfig.GetMapIndex(WorldPos);
    }


    public override void Initialize()
    {
        // 1 从 datacenter 获取地图数据
        string mapDataPath = Path.Combine(PathDefine.MAP_DATA_DIR_PATH, "_Level_0_1.json");
        if (File.Exists(mapDataPath) == false)
        {
            Debug.LogError("目标路径没有地图数据");
            return;
        }

        StreamReader sw = new StreamReader(mapDataPath);
        var jsonStr = sw.ReadToEnd();
        try
        {
            this._mapData = JsonUtility.FromJson<GameMapData>(jsonStr);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return;
        }

        // 2 生成 map grid
        this._mapGridConfig = new MapGridInfo(_mapData.mapColCount, _mapData.mapRowCount, _mapData.mapCellWidth, _mapData.mapCellHeight, 30);

        _mapGrounds = new List<MapGround>();
        _mapBlocks = new List<MapBlock>();

    }

    public void LoadMap()
    {
        LoadMapBase();
        LoadMapEvents();
        //GameObject gameObject = new GameObject("MavMeshOnLoad");
        //_navMeshLoad = gameObject.AddComponent<MavMeshOnLoad>();
        //_navMeshLoad.GetSurfaceComponent();
        //_navMeshLoad.Bake();
    }

    public bool IsPosOnGround(Vector3 worldPos)
    {
        var mapIndex = GetMapGridIndexWithPoint(worldPos);
        if (mapIndex < 0) return false;
        foreach (var ground in _mapGrounds)
        {
            if (ground.IsMapIndexInGround(mapIndex)) return true;
        }
        return false;
    }



    private void LoadMapBase()
    {
        var basePrefabFullPath = mapBasePrefabPath + _mapData.mapBasePrefabName;
        var go = PrefabUtil.LoadPrefab(basePrefabFullPath, "Load MapBase Prefab");
        go.transform.SetParent(BaseLayerNode.transform, false);

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
                    }
                }
            }
        }


    }

    private void LoadMapEvents()
    {

    }



    public override void OnGameUpdate(float deltaTime)
    {

    }

    public override void Dispose()
    {
        _mapData = null;
        _mapGrounds = null;
        _mapBlocks = null;
    }



}
