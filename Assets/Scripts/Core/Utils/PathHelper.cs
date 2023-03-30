using System.IO;
using UnityEngine;

public static class PathHelper
{
    public static string GetMapObsFilePath(string mapId)
    {
        return Path.Combine(PathDefine.MAP_OBS_DIR_PATH, string.Format("{0}_obs.bin", mapId));
    }

    public static string GetMapDataFilePath(string mapId)
    {
        return Path.Combine(PathDefine.MAP_DATA_DIR_PATH, string.Format("{0}_map.bin", mapId));
    }


    public static string GetMacroPath(string path)
    {
        var macroPath = path;

#if UNITY_IPHONE
        macroPath = System.IO.Path.Combine (path, PlatformDef.Ios);

#elif UNITY_ANDROID
        macroPath = System.IO.Path.Combine (path, PlatformDef.Android);
#else
        macroPath = System.IO.Path.Combine(path, PlatformDef.Windows);
#endif
        return macroPath;
    }
}
