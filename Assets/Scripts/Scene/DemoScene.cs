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
        AudioManager.Ins.LoadBgm("Audio/Music/Tobu - Higher");
        AudioManager.Ins.PlayBgm(true);
        UIManager.Ins.OpenPanel<PanelDemo>("Prefabs/UI/Panel_Demo");

        InputManager.Ins.RegisterInputControl(InputControlCenter.PlayerKeyboardInput);

        InputManager.Ins.AddState(InputStateDefine.GAMEDEMO_STATE);

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
