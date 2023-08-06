using GameEngine;
using System.Collections.Generic;

public abstract class GameScene : IScene
{
    protected GameScope mScope;
    protected GameEventListener mEventListener;

    public abstract string GetSceneName();

    public virtual void BeforeSceneEnter(Dictionary<string, object> args)
    {
        string sceneName = GetSceneName();
        mScope = GameScope.SceneScope.CreateChildScope(sceneName);
        mEventListener =  GamePoolCenter.Ins.GameEventLIstenerPool.Pop();
    }

    public abstract void OnSceneEnter(Dictionary<string, object> args);

    public abstract void OnSceneExit();

    public virtual void AfterSceneExit()
    {
        if (mEventListener != null)
        {
            mEventListener.ClearAllEventListen();
        }
        mScope.Dispose();
    }

    public abstract void OnSceneLateUpdate(float deltaTime);

    public abstract void OnSceneUpdate(float deltaTime);
}
