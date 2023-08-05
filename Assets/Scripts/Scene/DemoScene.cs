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

    public override void OnSceneEnter()
    {
        //AudioManager.Ins.LoadBgm("Audio/Music/Thunder Love");
        //AudioManager.Ins.LoadBgm("Audio/Music/bass-gone-walking_preview-150bpm");
        //AudioManager.Ins.LoadBgm("Audio/Music/broke-neck_preview-146bpm");
        //AudioManager.Ins.LoadBgm("Audio/Music/downtown-drummer_preview-119bpm");
        //AudioManager.Ins.LoadBgm("Audio/Music/groove-child_preview-125bpm");
        AudioManager.Ins.LoadBgm("Audio/Music/livin'-dat-life_preview-90bpm");
        //AudioManager.Ins.LoadBgm("Audio/Music/The Rush");
        //AudioManager.Ins.LoadBgm("Audio/Music/Tobu - Higher");
        AudioManager.Ins.PlayBgm(true);
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        AgentManager.Ins.LoadHero(3);
        AgentManager.Ins.LoadMonster(1001);
        GameMapManager.Ins.LoadMap();
    }

    public override void OnSceneExit()
    {
        
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        
    }
}
