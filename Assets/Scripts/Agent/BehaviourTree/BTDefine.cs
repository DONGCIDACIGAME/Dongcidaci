public static class BTDefine
{
    public const int BT_CheckResult_Succeed = 0;
    public const int BT_CheckResult_Failed = 1;
    public const int BT_CheckResult_Running = 2;

    public const int BT_LoadNodeResult_Succeed = 0;
    public const int BT_LoadNodeResult_Failed_NullData = -1;
    public const int BT_LoadNodeResult_Failed_WrongType = -2;
    public const int BT_LoadNodeResult_Failed_WrongDetailType = -3;
    public const int BT_LoadNodeResult_Failed_InvalidChildNodeNum = -4;

    public const int BT_Node_Type_TreeRoot = 0;
    public const int BT_Node_Type_Composite = 1;
    public const int BT_Node_Type_Decor = 2;
    public const int BT_Node_Type_Leaf = 3;

    public const int BT_Node_Type_TreeRoot_Entry = 0;

    // 详细类型每个预留了100个空位，应该可以保证新增时不用个更改其他大类里的详细类型id
    public const int BT_Node_DetailType_Composite_Sequence= BT_Node_Type_Composite * 100 + 1;
    public const int BT_Node_DetailType_Composite_Selector = BT_Node_Type_Composite * 100 + 2;
    public const int BT_Node_DetailType_Composite_Parallel = BT_Node_Type_Composite * 100 + 3;


    public const int BT_Node_Type_Decor_Invert = BT_Node_Type_Decor * 100 + 1;
    public const int BT_Node_Type_Decor_Repeat = BT_Node_Type_Decor * 100 + 2;

    public const int BT_Node_Type_Leaf_WaitTime = BT_Node_Type_Leaf * 100 + 1;
    public const int BT_Node_Type_Leaf_WaitFrame = BT_Node_Type_Leaf * 100 + 2;
}
