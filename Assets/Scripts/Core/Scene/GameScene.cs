using GameEngine;
using System.Collections.Generic;

public abstract class GameScene : IScene
{
    protected GameScope mScope;
    protected GameEventListener mEventListener;

    public abstract string GetSceneName();

    public virtual void BeforeSceneEnter()
    {
        string sceneName = GetSceneName();
        mScope = GameScope.SceneScope.CreateChildScope(sceneName);
        mEventListener = new GameEventListener();
    }

    public abstract void OnSceneEnter();

    public abstract void OnSceneExit();

    public virtual void AfterSceneExit()
    {
        if (mEventListener != null)
        {
            mEventListener.ClearEvents();
        }
        mScope.Dispose();
    }

    public abstract void OnSceneLateUpdate(float deltaTime);

    public abstract void OnSceneUpdate(float deltaTime);
}
