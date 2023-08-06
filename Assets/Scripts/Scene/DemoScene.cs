using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class DemoScene : GameScene
{
    public override string GetSceneName()
    {
        return SceneDefine.Demo;
    }

    public override void OnSceneEnter(Dictionary<string, object> args)
    {
        AgentManager.Ins.Initialize();

        string musicName = args["music"] as string;
        AudioManager.Ins.LoadBgm("Audio/Music/" + musicName);
        AudioManager.Ins.PlayBgm(true);
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        AgentManager.Ins.LoadHero(3);
        AgentManager.Ins.LoadMonster(1001);
        GameMapManager.Ins.LoadMap();
    }

    public override void OnSceneExit()
    {
        UIManager.Ins.ClosePanel<PanelDemo>();
        AgentManager.Ins.Dispose();
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        AgentManager.Ins.OnLateUpdate(deltaTime);
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        AgentManager.Ins.OnGameUpdate(deltaTime);
    }
}
