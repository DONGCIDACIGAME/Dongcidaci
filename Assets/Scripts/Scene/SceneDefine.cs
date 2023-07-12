using System.Collections.Generic;

public static class SceneDefine
{
    public const string Launch = "Launch";
    public const string SceneSelect = "SceneSelect";
    public const string MusicEditor = "MusicEditor";
    public const string Demo = "DemoScene";

    public static Dictionary<string, IScene> RegisteredScenes = new Dictionary<string, IScene>
    {
        {Launch, new LaunchScene()},
        {SceneSelect, new SceneSelectScene()},
        {Demo, new DemoScene()},
        {MusicEditor, new MusicEditorScene()},
    };

    public static IScene GetScene(string sceneName)
    {
        if(RegisteredScenes.TryGetValue(sceneName, out IScene scene))
        {
            return scene;
        }
        return null;
    }
}