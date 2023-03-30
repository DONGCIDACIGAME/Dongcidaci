
using UnityEngine;

// 热更新模式
public enum UwPatchMode
{
    SkipPatch = 1, // 跳过热更新
    Patch = 2,     // 要热更新
}

// 热更新：检查本地文件模式
public enum UwPatchCheckFileMode
{
    CheckFileSize = 1, // 要检查文件大小
    SkipCheckFileSize = 2, // 不检查文件大小
}

public static class BuildSetting
{
    public const string EditorBugReportKey = "_EditorBugReport";

    /// 热更新模式 （应使用Patch模式，开发环境下可以用SkipPatch跳过热更新）
    public static UwPatchMode PatchMode
    {
        get
        {
#if DEBUG_SKIP_PATCH
            return UwPatchMode.SkipPatch;
#else
            return UwPatchMode.Patch;
#endif
        }
    }

    /// 热更新：检查本地文件模式（玩家正式版本，应使用CheckFileSize模式）
    public static UwPatchCheckFileMode PatchCheckFileMode
    {
        get
        {
            return UwPatchCheckFileMode.SkipCheckFileSize;
        }
    }

    /// 是否上传BugReport
    public static bool UploadBugReport
    {
        get
        {          
            // 在编辑模式下，看本地设置
            return PlayerPrefs.GetInt(EditorBugReportKey, 0) > 0;
        }
    }

}