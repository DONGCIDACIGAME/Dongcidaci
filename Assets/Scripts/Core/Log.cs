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
    /// <summary>
    /// ！！仅临时打印时用这个, 日志等级最最低级的Info级，出包版本使用该等级打印可能不会打印日志
    /// </summary>
    /// <param name="str"></param>
    public static void Logic(string str, params object[] args)
    {
        Logic(LogLevel.Info, str, args);
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
        Debug.LogErrorFormat(TimeMgr.Ins.FrameIndex + "|" + str);
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

    public static void Break()
    {
        Debug.Break();
    }
}
