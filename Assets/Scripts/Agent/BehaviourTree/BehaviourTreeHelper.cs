using LitJson;
using UnityEngine;

public static class BehaviourTreeHelper
{
    public static BTNodeData LoadBTNodeData(string filePath)
    {
        if(string.IsNullOrEmpty(filePath))
        {
            Log.Error(LogLevel.Normal, "LoadTree Failed, filePath is null or empty!");
            return null;
        }

        string jsonStr = FileHelper.ReadText(filePath, System.Text.Encoding.UTF8);
        BTNodeData treeData = JsonMapper.ToObject<BTNodeData>(jsonStr);
        return treeData;
    }

    public static void SaveBTNodeData(string filePath, BTNodeData data)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Log.Error(LogLevel.Normal, "SaveBTTreeData Failed, filePath is null or empty!");
            return;
        }

        string jsonStr= JsonMapper.ToJson(data);

        FileHelper.WriteStr(filePath, jsonStr, System.Text.Encoding.UTF8);
    }

    public static int ParseInt(BTNodeArg arg, out int value)
    {
        value = 0;
        if(arg == null)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseInt Failed, arg is null!");
            return BTDefine.BT_LoadNodeResult_Failed_NullArg;
        }

        if(!arg.ArgType.Equals(BTDefine.BT_ArgType_int))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseInt Failed, arg type is not int, arg name:{0}, arg type:{1}", arg.ArgName, arg.ArgType);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgType;
        }


        bool result = int.TryParse(arg.ArgContent, out value);
        if(!result)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseInt Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public static int ParseFloat(BTNodeArg arg, out float value)
    {
        value = 0;
        if (arg == null)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseFloat Failed, arg is null!");
            return BTDefine.BT_LoadNodeResult_Failed_NullArg;
        }

        if (!arg.ArgType.Equals(BTDefine.BT_ArgType_float))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseFloat Failed, arg type is not int, arg name:{0}, arg type:{1}", arg.ArgName, arg.ArgType);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgType;
        }


        bool result = float.TryParse(arg.ArgContent, out value);
        if (!result)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseFloat Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public static int ParseString(BTNodeArg arg, out string value)
    {
        value = string.Empty;
        if (arg == null)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseString Failed, arg is null!");
            return BTDefine.BT_LoadNodeResult_Failed_NullArg;
        }

        if (!arg.ArgType.Equals(BTDefine.BT_ArgType_string))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseString Failed, arg type is not int, arg name:{0}, arg type:{1}", arg.ArgName, arg.ArgType);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgType;
        }

        value = arg.ArgContent;
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public static int ParseVector2(BTNodeArg arg, out Vector2 value)
    {
        value = Vector2.zero;
        if (arg == null)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector2 Failed, arg is null!");
            return BTDefine.BT_LoadNodeResult_Failed_NullArg;
        }

        if (!arg.ArgType.Equals(BTDefine.BT_ArgType_Vector2))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector2 Failed, arg type is not int, arg name:{0}, arg type:{1}", arg.ArgName, arg.ArgType);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgType;
        }

        string[] strArr = arg.ArgContent.Split('_');
        if (strArr.Length != 2)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector2 Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        if (!float.TryParse(strArr[0], out float v1) || !float.TryParse(strArr[1], out float v2))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector3 Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        value = new Vector2(v1, v2);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public static int ParseVector3(BTNodeArg arg, out Vector3 value)
    {
        value = Vector3.zero;
        if (arg == null)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector3 Failed, arg is null!");
            return BTDefine.BT_LoadNodeResult_Failed_NullArg;
        }

        if (!arg.ArgType.Equals(BTDefine.BT_ArgType_Vector3))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector3 Failed, arg type is not int, arg name:{0}, arg type:{1}", arg.ArgName, arg.ArgType);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgType;
        }

        string[] strArr = arg.ArgContent.Split('_');
        if(strArr.Length != 3)
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector3 Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        if(!float.TryParse(strArr[0], out float v1) || !float.TryParse(strArr[1], out float v2) || !float.TryParse(strArr[2], out float v3))
        {
            Log.Error(LogLevel.Normal, "BTTreeHelper ParseVector3 Failed, decode value failed, arg name:{0}, arg content:{1}", arg.ArgName, arg.ArgContent);
            return BTDefine.BT_LoadNodeResult_Failed_ParseArgFailed;
        }

        value = new Vector3(v1, v2, v3);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }
}
