using UnityEngine;

public enum LogLevel
{    
    Fatal = 0,  //致命错误
    Critical = 1, // 严重错误
    Normal = 2, // 一般错误
    Info = 3, // 提示性错误
    Undefine = 4,
}

public static class Log
{
    public static void Logic(string str)
    {
        Debug.Log(str);
    }    
    
    public static void Logic(LogLevel level,string str, params object[] args)
    {
        Debug.LogFormat(TimeMgr.Ins.FrameIndex +  "|" + str,args);
    }

    /// <summary>
    /// log error
    /// </summary>
    /// <param name="str"></param>
    /// <param name="level">0级:致命错误  1:严重错误  2:一般错误  3: 提示性错误</param>
    public static void Error(LogLevel level, string str)
    {
        Debug.LogError(str);
    }

    public static void Error(LogLevel level, string str, params object[] args)
    {
        Debug.LogErrorFormat(TimeMgr.Ins.FrameIndex + "|" + str, args);
    }

    public static void Warning(string str)
    {
        Debug.LogWarning(str);
    }

    public static void Warning(string str, params object[] args)
    {
        Debug.LogWarningFormat(TimeMgr.Ins.FrameIndex + "|" + str, args);
    }

    public static void Assert(bool condition,string msg)
    {
        Debug.Assert(condition, msg);
    }
}
