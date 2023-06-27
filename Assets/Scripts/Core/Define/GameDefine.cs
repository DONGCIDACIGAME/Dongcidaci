using GameEngine;

public static class GameDefine
{
    public static string _UI_ROOT = "_UI_ROOT";

    //added by weng 0522 1038
    public static string _MAP_ROOT = "_MAP";

    public const int MaxPlayerNum = 2; //  同一台机器最多支持的玩家数量

    
}

public static class CoreDefine
{
    public const int bufferSizeOfInt = sizeof(int) * 8;

    public const int BitTypeModuleBufferSize = 32; //最多支持32个模块定义
    public const int BitTypeEventBufferSize     = 128; //最多支持128种事件类型定义
    public const int BitTypeAgentStateBufferSize = 32;// 最多支持32中角色状态

}

public static class ModuleDef
{
    // 生成index的种子数
    // 如果有重登录过程，这个种子数需要重置一下
    private static int indexSeed = 0;

    public static BitType GameMgrModule = BitTypeCreator.CreateModuleBitType(indexSeed++, "GameMgrModule");
    public static BitType SceneModule = BitTypeCreator.CreateModuleBitType(indexSeed++, "SceneModule");
    //public static BitType InputModule = BitTypeCreator.CreateModuleBitType(indexSeed++, "InputModule");
    public static BitType UIModule = BitTypeCreator.CreateModuleBitType(indexSeed++, "UIModule");
    public static BitType MapEditor = BitTypeCreator.CreateEventModuleBitType(indexSeed++, "MapEditor");
}

public static class CameraDef
{
    public const string MapEditorCamCtl = "MapEditorCamCtl";
}

public static class AgentTypeDef
{
    public const int AGENT_TYPE_UNDEFINED = 0;
    public const int AGENT_TYPE_HERO = 1;
    public const int AGENT_TYPE_MONSTER = 2;
    public const int AGENT_TYPE_NPC = 3;
}

public class PlatformDef
{
    public const string Android = "android";
    public const string Ios = "ios";
    public const string Windows = "windows";
    public const string Osx = "Osx";

#if UNITY_ANDROID && (!UNITY_EDITOR)
    public const string Current = Android;    
#elif UNITY_IOS && (!UNITY_EDITOR)
    public const string Current = Ios;
#else
    public const string Current = Windows;
#endif

}

public static class MapDef
{
    public static bool GlobalDrawMapDizmos = true;

    public static float SceneRotation = 45f;
}

public static class EventDef
{
    /// <summary>
    /// 对于post的事件，每帧最多处理多少个
    /// </summary>
    public const int PostEventExcuteSingleFrame = 20;
}



