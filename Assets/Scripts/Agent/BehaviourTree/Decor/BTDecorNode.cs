/// <summary>
/// 装饰节点,包括以下几种类型
/// 1. invert节点：将返回结果反转
/// 2. repeate节点：将重复执行子节点n次
/// ...
/// </summary>
public abstract class BTDecorNode : BTNode
{
    protected BTNode mChildNode;

    public void SetChildNode(BTNode childNode)
    {
        mChildNode = childNode;
    }

    public override int GetNodeArgNum()
    {
        return 0;
    }

    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Decor;
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

    public override void Reset()
    {
        if (mChildNode == null)
            return;

        mChildNode.Reset();
    }


    protected override void CustomDispose()
    {
        base.CustomDispose();

        if(mChildNode != null)
        {
            mChildNode.Dispose();
        }
    }
}