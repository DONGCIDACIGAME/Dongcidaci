public enum BT_CHILD_NODE_NUM
{
    Zero,
    One,
    AtLeastOne
}


public static class BTDefine
{
    /// <summary>
    /// 树节点的执行结果
    /// </summary>
    public const int BT_ExcuteResult_Succeed = 0;  //成功
    public const int BT_ExcuteResult_Failed = 1; //失败
    public const int BT_ExcuteResult_Running = 2; //执行中

    /// <summary>
    /// 树节点数据加载结果
    /// </summary>
    public const int BT_LoadNodeResult_Succeed = 0;
    public const int BT_LoadNodeResult_Failed_NullData = -1;
    public const int BT_LoadNodeResult_Failed_WrongType = -2;
    public const int BT_LoadNodeResult_Failed_WrongDetailType = -3;
    public const int BT_LoadNodeResult_Failed_InvalidChildNum = -4;
    public const int BT_LoadNodeResult_Failed_InvalidArgNum = -5;
    public const int BT_LoadNodeResult_Failed_InvalidArgType = -6;
    public const int BT_LoadNodeResult_Failed_NullArg = -7;
    public const int BT_LoadNodeResult_Failed_ParseArgFailed= -8;
    public const int BT_LoadNodeResult_Failed_Unkown = -9;

    /// <summary>
    /// 节点参数类型
    /// </summary>
    public static string BT_ArgType_int = "int";
    public static string BT_ArgType_float = "float";
    public static string BT_ArgType_string = "string";
    public static string BT_ArgType_Vector2 = "Vector2";
    public static string BT_ArgType_Vector3 = "Vector3";


    /// <summary>
    /// 节点的大类型
    /// </summary>
    public const int BT_Node_Type_Tree = 0;
    public const int BT_Node_Type_Composite = 1;
    public const int BT_Node_Type_Decor = 2;
    public const int BT_Node_Type_Leaf = 3;


    /// <summary>
    /// 节点类型的详细类型
    /// 每个大类型预留了100个详细类型空位，应该可以保证新增时和其他大类里的详细类型不冲突
    /// </summary>
    public const int BT_Node_Type_Tree_Entry = 0;
    public const int BT_Node_Type_Tree_ChildTree = 1;

    public const int BT_Node_DetailType_Composite_Sequence              = BT_Node_Type_Composite * 100 + 1;
    public const int BT_Node_DetailType_Composite_Selector              = BT_Node_Type_Composite * 100 + 2;
    public const int BT_Node_DetailType_Composite_Parallel              = BT_Node_Type_Composite * 100 + 3;

    public const int BT_Node_Type_Decor_Invert                          = BT_Node_Type_Decor * 100 + 1;
    public const int BT_Node_Type_Decor_Repeat                          = BT_Node_Type_Decor * 100 + 2;
    public const int BT_Node_Type_Decor_Reset                           = BT_Node_Type_Decor * 100 + 3;
    public const int BT_Node_Type_Decor_UntilTrue                       = BT_Node_Type_Decor * 100 + 4;
    public const int BT_Node_Type_Decor_Once                            = BT_Node_Type_Decor * 100 + 5;

    public const int BT_Node_Type_Leaf_WaitTime                         = BT_Node_Type_Leaf * 100 + 1;
    public const int BT_Node_Type_Leaf_WaitFrame                        = BT_Node_Type_Leaf * 100 + 2;
    public const int BT_Node_Type_Leaf_WaitMeter                        = BT_Node_Type_Leaf * 100 + 3;
    public const int BT_Node_Type_Leaf_CheckDetectAgent                 = BT_Node_Type_Leaf * 100 + 4;
    public const int BT_Node_Type_Leaf_ChangeTowards                    = BT_Node_Type_Leaf * 100 + 5;
    public const int BT_Node_Type_Leaf_MoveTime                         = BT_Node_Type_Leaf * 100 + 6;
    public const int BT_Node_Type_Leaf_MoveMeter                        = BT_Node_Type_Leaf * 100 + 7;
    public const int BT_Node_Type_Leaf_MoveToPosition                   = BT_Node_Type_Leaf * 100 + 8;
    public const int BT_Node_Type_Leaf_MoveDistance                     = BT_Node_Type_Leaf * 100 + 9;


    /// <summary>
    /// 子树数据的拷贝类型
    /// </summary>
    public const int BT_ChildTreeCopyType_New = 0; // 新建一份数据
    public const int BT_ChildTreeCopyType_Reference = 1; // 引用源数据


    /// <summary>
    /// 不改变朝向
    /// </summary>
    public const int BT_ChangeTowardsTo_NotDefine = 0;
    /// <summary>
    /// 随机朝向
    /// </summary>
    public const int BT_ChangeTowardsTo_Random = 1;
    /// <summary>
    /// 反向
    /// </summary>
    public const int BT_ChangeTowardsTo_Invert = 2;
    /// <summary>
    /// 朝向给定目标（上下文中的）
    /// </summary>
    public const int BT_ChangeTowardsTo_GivenTarget = 3;
}
