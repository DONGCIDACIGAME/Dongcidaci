using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class PathDefine
{
    public static string ROOT_PATH = Application.dataPath.Substring(0,Application.dataPath.Length - 7);

    public static string EDITOR_DATA_DIR_PATH = Path.Combine(ROOT_PATH,"Data");
    
    public static string RELEASE_DATA_DIR_PATH = Application.dataPath + "/StreamingAssets/Data";

    public static string LUA_DIR_PATH = Path.Combine(EDITOR_DATA_DIR_PATH, "LuaScripts");

    public static string MAP_OBS_DIR_PATH = Path.Combine(EDITOR_DATA_DIR_PATH, "ObsData");
    public static string TABLE_PB_DATA_PATH = Path.Combine(EDITOR_DATA_DIR_PATH, "TableData");

    public static string MAP_DATA_DIR_PATH = Path.Combine(EDITOR_DATA_DIR_PATH, "MapData");

    public static string ASSETBUNDLES_DIR = Path.Combine(Application.dataPath,  "AssetBundles");

    public const string ASSETBUNDLES_ROOT_DIR = "Assets/AssetBundles/";
    public static string INTERNAL_ASSETBUNDLES_DIR = "AssetBundles";

    //public static string ASSET_DIR = PathHelper.GetMacroPath(Path.Combine(Application.streamingAssetsPath, INTERNAL_ASSETBUNDLES_DIR));
    public static string STREAMINGASSET_DIR = Application.streamingAssetsPath;
}
