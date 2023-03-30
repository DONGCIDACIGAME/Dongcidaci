using System.Collections.Generic;

/// <summary>
/// 全局控制变量
/// </summary>
public class GlobeVar
{
    /// <summary>
    /// 包名，包名用于标别包体，用于配置热更新入口
    /// </summary>
    public static string binaryPackageName = string.Empty;
    /// <summary>
    /// 内置版本号 ( Version.Xml ) 出包时候的版本号 无法修改
    /// </summary>
    public static VersionData binaryVersion = new VersionData();
    /// <summary>
    /// 二进制版本构建日期
    /// </summary>
    public static string binaryBuildTime = string.Empty;
    /// <summary>
    /// 服务器补丁构建日期
    /// </summary>
    public static string serverPatchTime = string.Empty;
    /// <summary>
    /// 当前本地版本号，会被热更新修改掉
    /// </summary>
    public static VersionData localVersion = new VersionData();
    /// <summary>
    /// 当前拉取的服务器的版本号
    /// </summary>
    public static VersionData serverVersion = new VersionData();

    /// <summary>
    /// 版本更新地址
    /// </summary>
    public static UwUrlData updateUrl = new UwUrlData();

    /// <summary>
    /// 屏幕原始分辨率--宽
    /// </summary>
    public static int ScreenWidth = 0;
    /// <summary>
    /// 屏幕原始分辨率--高
    /// </summary>
    public static int ScreenHeight = 0;

    // BugReport配置
    public static int BugMaxLen = 400 *1024; // 400KB
    public static string BugReportUrl = string.Empty;

    public static string NoticeSvrUrl = string.Empty;
    public static string IPAddr= string.Empty;

    public static bool IsLogDevice = false;

    public static void initVar(string url, int maxLen, string noticeSvrUrl,string ipaddr)
    {
        BugReportUrl = url;
        BugMaxLen = maxLen;
        NoticeSvrUrl = noticeSvrUrl;
        IPAddr = ipaddr;
    }

    public static void EditorFree()
    {
        updateUrl = new UwUrlData();
    }
    public static void ReSetData()
    {
        binaryVersion = new VersionData();
        localVersion = new VersionData();
        serverVersion = new VersionData();
    }
}