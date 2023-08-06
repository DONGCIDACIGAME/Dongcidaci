using System.Collections.Generic;

public interface IScene
{
    string GetSceneName();

    void BeforeSceneEnter(Dictionary<string, object> args);

    void OnSceneEnter(Dictionary<string, object> args);

    void OnSceneUpdate(float deltaTime);

    void OnSceneLateUpdate(float deltaTime);

    void OnSceneExit();

    void AfterSceneExit();
}
