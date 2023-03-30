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
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        InputManager.Ins.RegisterInputControl(InputControlCenter.AttackInputCtl);

        InputManager.Ins.AddState(InputStateDefine.GAMEDEMO_STATE);

        AudioManager.Ins.LoadBgm("Audio/Music/Tobu - Higher");

        AgentManager.Ins.LoadHero(3);
    }

    public override void OnSceneExit()
    {
        InputManager.Ins.RemoveState(InputStateDefine.GAMEDEMO_STATE);
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        
    }
}
