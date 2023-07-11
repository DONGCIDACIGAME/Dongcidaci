public class BTTreeEntry : BTTree
{
    public override int GetNodeArgNum()
    {
        return 0;
    }

    public override BT_CHILD_NODE_NUM GetChildNodeNum()
    {
        return BT_CHILD_NODE_NUM.One;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        return null;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
        {
            Log.Error(LogLevel.Normal, "[{0}] GetChildNodesData Error, child node is null!", NodeName);
            return null;
        }

        BTNodeData childNodeData = childNode.ToBTNodeData();
        return new BTNodeData[] { childNodeData };
    }

    protected override int LoadChildNodes(BTNodeData[] chlidNodes)
    {
        // 加载子节点
        BTNodeData nodeData = chlidNodes[0];
        BTNode node = BehaviourTreeHelper.CreateBTNode(nodeData);
        int childLoadResult = node.LoadFromBTNodeData(nodeData);

        // 严格要求所有节点都正确加载，这个AI行为树才可以使用
        if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
            return childLoadResult;

        SetChildNode(node);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Tree_Entry;
    }
}