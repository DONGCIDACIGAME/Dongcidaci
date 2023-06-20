public enum BT_CHILD_NODE_NUM
{
    Zero,
    One,
    AtLeastOne
}


public static class BTDefine
{
    public const int BT_ExcuteResult_Succeed = 0;
    public const int BT_ExcuteResult_Failed = 1;
    public const int BT_ExcuteResult_Running = 2;

    public const int BT_LoadNodeResult_Succeed = 0;
    public const int BT_LoadNodeResult_Failed_NullData = -1;
    public const int BT_LoadNodeResult_Failed_WrongType = -2;
    public const int BT_LoadNodeResult_Failed_WrongDetailType = -3;
    public const int BT_LoadNodeResult_Failed_InvalidChildNum = -4;
    public const int BT_LoadNodeResult_Failed_InvalidArgNum = -5;
    public const int BT_LoadNodeResult_Failed_InvalidArgType = -6;
    public const int BT_LoadNodeResult_Failed_NullArg = -7;
    public const int BT_LoadNodeResult_Failed_ParseArgFailed= -8;

    public static string BT_ArgType_int = "int";
    public static string BT_ArgType_float = "float";
    public static string BT_ArgType_string = "string";
    public static string BT_ArgType_Vector2 = "Vector2";
    public static string BT_ArgType_Vector3 = "Vector3";

    public const int BT_Node_Type_Tree = 0;
    public const int BT_Node_Type_Composite = 1;
    public const int BT_Node_Type_Decor = 2;
    public const int BT_Node_Type_Leaf = 3;

    public const int BT_Node_Type_Tree_Entry = 0;
    public const int BT_Node_Type_Tree_ChildTree = 1;

    // 详细类型每个预留了100个空位，应该可以保证新增时不用个更改其他大类里的详细类型id
    public const int BT_Node_DetailType_Composite_Sequence= BT_Node_Type_Composite * 100 + 1;
    public const int BT_Node_DetailType_Composite_Selector = BT_Node_Type_Composite * 100 + 2;
    public const int BT_Node_DetailType_Composite_Parallel = BT_Node_Type_Composite * 100 + 3;


    public const int BT_Node_Type_Decor_Invert = BT_Node_Type_Decor * 100 + 1;
    public const int BT_Node_Type_Decor_Repeat = BT_Node_Type_Decor * 100 + 2;

    public const int BT_Node_Type_Leaf_WaitTime = BT_Node_Type_Leaf * 100 + 1;
    public const int BT_Node_Type_Leaf_WaitFrame = BT_Node_Type_Leaf * 100 + 2;
}
