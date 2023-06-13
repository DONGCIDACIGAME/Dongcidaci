public class BTTree : BTNode
{
    private BTNode mChildNode;

    public void SetChildNode(BTNode childNode)
    {
        mChildNode = childNode;
    }

    public BTNode GetChildNode()
    {
        return mChildNode;
    }

    public override int GetNodeArgNum()
    {
        return 0;
    }

    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Tree;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Tree_Entry;
    }

    public override BT_CHILD_NODE_NUM GetChildeNodeNum()
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

    protected override int LoadChildNodes(BTNodeData[] chlidNodes)
    {
        // 加载子节点
        BTNodeData nodeData = chlidNodes[0];
        BTNode node = BehaviourTreeManager.Ins.CreateBTNode(nodeData);
        int childLoadResult = node.LoadFromBTNodeData(nodeData);

        // 严格要求所有节点都正确加载，这个AI行为树才可以使用
        if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
            return childLoadResult;

        SetChildNode(node);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        BTNodeData childNodeData = mChildNode.ToBTNodeData();
        return new BTNodeData[] { childNodeData };
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        return mChildNode.Excute(deltaTime);
    }

    public override void Reset()
    {
        if (mChildNode == null)
            return;

        mChildNode.Reset();
    }


    protected override void CustomDispose()
    {
        if (mChildNode == null)
            return;

        mChildNode.Dispose();
    }
}