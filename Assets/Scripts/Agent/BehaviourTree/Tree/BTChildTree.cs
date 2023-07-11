public class BTChildTree : BTTree
{
    /// <summary>
    /// 子树的名字
    /// </summary>
    private string mChildTreeName;

    /// <summary>
    /// 子树的copy类型
    /// BT_ChildTreeCopyType_New: 深拷贝，使用源树的数据新建了一份数据
    /// BT_ChildTreeCopyType_Reference:浅拷贝，保存的是对源树的引用
    /// </summary>
    private int mCopyType;

    public void SetChildTreeName(string fileName)
    {
        mChildTreeName = fileName;
    }

    public string GetChildTreeFileName()
    {
        return mChildTreeName;
    }

    public void SetCopyType(int copyType)
    {
        mCopyType = copyType;
    }

    public int GetCopyType()
    {
        return mCopyType;
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (!base.BTNodeDataCheck(ref info))
            return false;

        if (string.IsNullOrEmpty(mChildTreeName))
        {
            info = "子树名称不能为空!";
            return false;
        }

        BTNode childNode = GetChildNode();
        if (childNode is not BTTreeEntry)
        {
            info = "子树节点的子节点必须是树的入口节点!";
            return false;
        }

        return true;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        BTNode childNode = GetChildNode();
        if(childNode == null)
        {
            Log.Error(LogLevel.Normal, "[{0}] GetChildNodesData Error, child node is null!", NodeName);
            return null;
        }

        if(mCopyType == BTDefine.BT_ChildTreeCopyType_New)
        {
            BTNodeData childNodeData = childNode.ToBTNodeData();
            return new BTNodeData[] { childNodeData };
        }
        else if(mCopyType == BTDefine.BT_ChildTreeCopyType_Reference)
        {
            return null;
        }

        Log.Error(LogLevel.Normal, "BTChildTree GetChildNodesData Error, invalid copyType:{0}", mCopyType);
        return null;
    }

    /// <summary>
    /// 子树的文件名
    /// </summary>
    /// <returns></returns>
    public override int GetNodeArgNum()
    {
        return 2;
    }

    public override BT_CHILD_NODE_NUM GetChildNodeNum()
    {
        if(mCopyType == BTDefine.BT_ChildTreeCopyType_New)
        {
            return BT_CHILD_NODE_NUM.One;
        }
        else if(mCopyType == BTDefine.BT_ChildTreeCopyType_Reference)
        {
            return BT_CHILD_NODE_NUM.Zero;
        }

        Log.Error(LogLevel.Normal, "BTChildTree GetChildeNodeNum Error, unknown Copy Type!");
        return BT_CHILD_NODE_NUM.Zero;
    }


    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_string("ChildTree", mChildTreeName);
        BTNodeArg arg2 = new BTNodeArg_int("CopyType", mCopyType);
        return new BTNodeArg[] { arg1, arg2};
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        // 读取子树的名字
        int result1 = BehaviourTreeHelper.ParseString(args[0], out string value1);
        if (result1 != BTDefine.BT_LoadNodeResult_Succeed)
            return result1;

        // 读取子树的copy类型
        int result2 = BehaviourTreeHelper.ParseInt(args[1], out int value2);
        if (result2 != BTDefine.BT_LoadNodeResult_Succeed)
            return result2;


        SetChildTreeName(value1);
        SetCopyType(value2);

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override int LoadChildNodes(BTNodeData[] childNodes)
    {
        if(mCopyType == BTDefine.BT_ChildTreeCopyType_Reference)
        {
            // 加载子节点
            string childTreeFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(mChildTreeName);
            BTNodeData nodeData = DataCenter.Ins.BehaviourTreeCenter.LoadTreeData(childTreeFullPath, true);
            BTNode node = BehaviourTreeHelper.CreateBTNode(nodeData);
            int childLoadResult = node.LoadFromBTNodeData(nodeData);

            // 严格要求所有节点都正确加载，这个AI行为树才可以使用
            if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
                return childLoadResult;

            SetChildNode(node);
            return BTDefine.BT_LoadNodeResult_Succeed;
        }
        else if(mCopyType == BTDefine.BT_ChildTreeCopyType_New)
        {
            // 加载子节点
            BTNodeData nodeData = childNodes[0];
            BTNode node = BehaviourTreeHelper.CreateBTNode(nodeData);
            int childLoadResult = node.LoadFromBTNodeData(nodeData);

            // 严格要求所有节点都正确加载，这个AI行为树才可以使用
            if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
                return childLoadResult;

            SetChildNode(node);
            return BTDefine.BT_LoadNodeResult_Succeed;
        }

        return BTDefine.BT_LoadNodeResult_Failed_Unkown;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Tree_ChildTree;
    }
}
