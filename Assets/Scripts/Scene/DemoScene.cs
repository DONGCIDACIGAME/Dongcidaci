using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;
using System.IO;

public class DemoScene : GameScene, IMeterHandler
{
    private AgentManager mAgtMgr;
    private GameMapManager mMapMgr;

    public override string GetSceneName()
    {
        return SceneDefine.Demo;
    }

    public override void OnSceneEnter(Dictionary<string, object> args)
    {
        mAgtMgr = new AgentManager();
        mAgtMgr.Initialize();

        //added by weng 
        // 目前场景的逻辑层级更高，因此关卡数据的加载应放在这个位置
        // 1 从 datacenter 获取地图数据
        string mapDataPath = Path.Combine(PathDefine.MAP_DATA_DIR_PATH, "_Level_0_2.json");
        GameMapData loadedMapData;
        if (File.Exists(mapDataPath) == false)
        {
            Debug.LogError("目标路径没有地图数据");
            return;
        }

        StreamReader sw = new StreamReader(mapDataPath);
        var jsonStr = sw.ReadToEnd();
        try
        {
            loadedMapData = JsonUtility.FromJson<GameMapData>(jsonStr);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
            return;
        }

        mMapMgr = new GameMapManager();
        // GameMapMgr should init with the loaded map data
        mMapMgr.Initialize(loadedMapData);

        GameNodeCenter.Ins.InitliazeAgentNodes();
        GameNodeCenter.Ins.InitliazeMapNodes();

        string musicName = args["music"] as string;
        AudioManager.Ins.LoadBgm("Audio/Music/" + musicName);
        AudioManager.Ins.PlayBgm(true);
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        // added by weng 
        // agtMgr should load the agents according to mapdata config
        foreach (var mapEvent in loadedMapData.mapEventDatas)
        {
            if(mapEvent.eventType == MapEventType.PlayerInitPoint) mAgtMgr.LoadHero(3,new Vector3(mapEvent.initPosX,mapEvent.initPosY,mapEvent.initPosZ));
            if(mapEvent.eventType == MapEventType.MonsterInitPoint)
            {
                mAgtMgr.LoadMonster(1002, new Vector3(mapEvent.initPosX, mapEvent.initPosY, mapEvent.initPosZ));
            }
        }
        
        mMapMgr.LoadMap();

        MeterManager.Ins.RegisterMeterHandler(this);
    }

    public override void OnSceneExit()
    {
        UIManager.Ins.ClosePanel<PanelDemo>();

        mAgtMgr.Dispose();
        mAgtMgr = null;

        mMapMgr.Dispose();
        mMapMgr = null;

        GameNodeCenter.Ins.DisposeAgentNodes();
        GameNodeCenter.Ins.DisposeMapNodes();

        MeterManager.Ins.UnregiseterMeterHandler(this);
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        mAgtMgr.OnLateUpdate(deltaTime);
        mMapMgr.OnLateUpdate(deltaTime);
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        mAgtMgr.OnGameUpdate(deltaTime);
        mMapMgr.OnGameUpdate(deltaTime);
    }

    public void OnDisplayPointBeforeMeterEnter(int meterIndex)
    {
        mAgtMgr.OnDisplayPointBeforeMeterEnter(meterIndex);
        mMapMgr.OnDisplayPointBeforeMeterEnter(meterIndex);
    }

    public void OnMeterEnd(int meterIndex)
    {
        mAgtMgr.OnMeterEnd(meterIndex);
        mMapMgr.OnMeterEnd(meterIndex);
    }

    public void OnMeterEnter(int meterIndex)
    {
        mAgtMgr.OnMeterEnter(meterIndex);
        mMapMgr.OnMeterEnter(meterIndex);
    }
}
