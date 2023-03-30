public interface IScene
{
    string GetSceneName();

    void BeforeSceneEnter();

    void OnSceneEnter();

    void OnSceneUpdate(float deltaTime);

    void OnSceneLateUpdate(float deltaTime);

    void OnSceneExit();

    void AfterSceneExit();
}
