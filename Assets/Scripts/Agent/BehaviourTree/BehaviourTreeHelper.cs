using LitJson;
using UnityEngine;

public static class BehaviourTreeHelper
{
    public static string GetNodeTypeName(int BTNodeType)
    {
        switch (BTNodeType)
        {
            case BTDefine.BT_Node_Type_Tree:
                return "树节点";
            case BTDefine.BT_Node_Type_Composite:
                return "组合节点";
            case BTDefine.BT_Node_Type_Decor:
                return "装饰节点";
            case BTDefine.BT_Node_Type_Leaf:
                return "叶子节点";
            default:
                return "未知节点类型";
        }
    }

    /// <summary>
    /// ---------------------------增加新节点时需要更新
    /// </summary>
    /// <param name="BTNodeDetailType"></param>
    /// <returns></returns>
    public static string GetNodeDetailTypeName(int BTNodeDetailType)
    {
        switch (BTNodeDetailType)
        {
            case BTDefine.BT_Node_Type_Tree_Entry:
                return "行为树入口";
            case BTDefine.BT_Node_Type_Tree_ChildTree:
                return "子树";
            case BTDefine.BT_Node_Type_Composite_Sequence:
                return "顺序执行";
            case BTDefine.BT_Node_Type_Composite_Selector:
                return "选择执行";
            case BTDefine.BT_Node_Type_Composite_Parallel:
                return "并行执行";
            case BTDefine.BT_Node_Type_Composite_IfElse:
                return "IfElse逻辑";
            case BTDefine.BT_Node_Type_Composite_WithStateSequence:
                return "带状态的顺序执行";
            case BTDefine.BT_Node_Type_Composite_WithStateSelector:
                return "带状态的选择执行";
            case BTDefine.BT_Node_Type_Decor_Invert:
                return "反转";
            case BTDefine.BT_Node_Type_Decor_Repeat:
                return "重复";
            case BTDefine.BT_Node_Type_Decor_Once:
                return "仅一次";
            case BTDefine.BT_Node_Type_Decor_Reset:
                return "重置";
            case BTDefine.BT_Node_Type_Decor_UntilTrue:
                return "直到成功";
            case BTDefine.BT_Node_Type_Leaf_WaitTime:
                return "等待时间";
            case BTDefine.BT_Node_Type_Leaf_WaitFrame:
                return "等待帧数";
            case BTDefine.BT_Node_Type_Leaf_WaitMeter:
                return "等待节拍数";
            case BTDefine.BT_Node_Type_Leaf_ChangeTowards:
                return "改变朝向";
            case BTDefine.BT_Node_Type_Leaf_MoveTime:
                return "移动一定时间";
            case BTDefine.BT_Node_Type_Leaf_MoveMeter:
                return "移动一定节拍";
            case BTDefine.BT_Node_Type_Leaf_MoveDistance:
                return "移动一定距离";
            case BTDefine.BT_Node_Type_Leaf_MoveOneFrame:
                return "移动一帧";
            case BTDefine.BT_Node_Type_Leaf_MoveToPosition:
                return "移动到指定位置";
            case BTDefine.BT_Node_Type_Leaf_DetectAgentInArea:
                return "检测指定范围内的指定角色";
            case BTDefine.BT_Node_Type_Leaf_CheckDistanceToEntity:
                return "到目标Entity距离";
            case BTDefine.BT_Node_Type_Leaf_CheckDistanceToPosition:
                return "到目标位置距离";
            case BTDefine.BT_Node_Type_Leaf_CheckTargetEntityInLogicArea:
                return "检测在逻辑区域内";
            case BTDefine.BT_Node_Type_Leaf_CheckInStatus:
                return "检测在目标状态下";
            case BTDefine.BT_Node_Type_Leaf_CheckHasTarget:
                return "有目标对象";
            case BTDefine.BT_Node_Type_Leaf_ClearTarget:
                return "清除目标";
            case BTDefine.BT_Node_Type_Leaf_Idle:
                return "进入idle状态";
            case BTDefine.BT_Node_Type_Leaf_Attack:
                return "攻击";
            default:
                return "未知节点类型";
        }
    }

    public static string FileFullPathToTreeName(string fileFullPath)
    {
        if (string.IsNullOrEmpty(fileFullPath))
        {
            Log.Error(LogLevel.Normal, "FileFullPathToTreeName Error, fileFullPath is null or empty!");
            return null;
        }
        string[] ret = fileFullPath.Replace(".tree", "").Replace('\\','/').Split('/', System.StringSplitOptions.None);
        if (ret.Length > 0)
        {
            return ret[ret.Length - 1];
        }

        return "Invalid_Tree_Name";
    }

    public static string TreeNameToFileFullPath(string treeName)
    {
        if(string.IsNullOrEmpty(treeName))
        {
            Log.Error(LogLevel.Normal, "TreeNameToFileFullPath Error, tree name is null or empty!");
            return null;
        }
        return PathDefine.AI_TREE_DATA_DIR_PATH + "/" + treeName + ".tree";
    }

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

    public static BTNode FindFirstInvalidNode(BTNode root, ref string info)
    {
        if(!root.BTNodeDataCheck(ref info))
        {
            return root;
        }

        if (root is BTDecorNode)
        {
            BTDecorNode decor = root as BTDecorNode;
            return FindFirstInvalidNode(decor.GetChildNode(),ref info);
        }
        else if (root is BTTree)
        {
            BTTree tree = root as BTTree;
            return FindFirstInvalidNode(tree.GetChildNode(),ref info);
        }
        else if (root is BTCompositeNode)
        {
            BTCompositeNode composite = root as BTCompositeNode;
            foreach(BTNode node in composite.GetChildNodes())
            {
                BTNode _invalidNode = FindFirstInvalidNode(node, ref info);
                if (_invalidNode != null)
                    return _invalidNode;
            }
        }

        return null;
    }


    #region Data Parse
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
    #endregion
}
