public class BTChildTree : BTTree
{
    private string mChildTreeName;

    public void SetChildTreeName(string fileName)
    {
        mChildTreeName = fileName;
    }

    public string GetChildTreeFileName()
    {
        return mChildTreeName;
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

        return true;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        return null;
    }

    /// <summary>
    /// 子树的文件名
    /// </summary>
    /// <returns></returns>
    public override int GetNodeArgNum()
    {
        return 1;
    }

    public override BT_CHILD_NODE_NUM GetChildeNodeNum()
    {
        return BT_CHILD_NODE_NUM.Zero;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Tree_ChildTree;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg = new BTNodeArg();
        arg.ArgName = "ChildTree";
        arg.ArgType = BTDefine.BT_ArgType_string;
        arg.ArgContent = mChildTreeName;
        return new BTNodeArg[] { arg };
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseString(args[0], out string value);
        if (result == BTDefine.BT_ExcuteResult_Succeed)
        {
            SetChildTreeName(value);
        }

        return result;
    }

    protected override int LoadChildNodes(BTNodeData[] chlidNodes)
    {
        // 加载子节点
        string childTreeFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(mChildTreeName);
        BTNodeData nodeData = BehaviourTreeManager.Ins.LoadTreeData(childTreeFullPath, true);
        BTNode node = BehaviourTreeManager.Ins.CreateBTNode(nodeData);
        int childLoadResult = node.LoadFromBTNodeData(nodeData);

        // 严格要求所有节点都正确加载，这个AI行为树才可以使用
        if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
            return childLoadResult;

        SetChildNode(node);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }
}
