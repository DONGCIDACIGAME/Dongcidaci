using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class SceneSelectScene : GameScene
{
    public override string GetSceneName()
    {
        return SceneDefine.SceneSelect;
    }

    public override void OnSceneEnter()
    {
        Log.Logic(LogLevel.Info, "select scene....open select panel");
        UIManager.Ins.OpenPanel<PanelSceneSelect>("Prefabs/UI/Panel_SceneSelect");
    }

    public override void OnSceneExit()
    {
        UIManager.Ins.ClosePanel<PanelSceneSelect>();
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        
    }
}
