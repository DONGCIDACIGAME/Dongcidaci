using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

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

        mMapMgr = new GameMapManager();
        mMapMgr.Initialize();

        GameNodeCenter.Ins.InitliazeAgentNodes();
        GameNodeCenter.Ins.InitliazeMapNodes();

        string musicName = args["music"] as string;
        AudioManager.Ins.LoadBgm("Audio/Music/" + musicName);
        AudioManager.Ins.PlayBgm(true);
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        mAgtMgr.LoadHero(3);
        //mAgtMgr.LoadMonster(1001);
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
