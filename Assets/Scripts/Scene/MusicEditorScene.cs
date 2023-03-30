using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEngine;

public class MusicEditorScene : GameScene
{
    public override string GetSceneName()
    {
        return SceneDefine.MusicEditor;
    }

    public override void OnSceneEnter()
    {
        UIManager.Ins.OpenPanel<PanelMusicEditor>("Prefabs/UI/MusicEditor/Panel_MusicEditor");
    }

    public override void OnSceneExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnSceneLateUpdate(float deltaTime)
    {
        
    }

    public override void OnSceneUpdate(float deltaTime)
    {
        
    }
}
