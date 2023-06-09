/// <summary>
/// 叶子结点，用来承载各种行为和逻辑判断
/// 叶子结点是终止结点，不再向下延伸
/// </summary>
public abstract class BTLeafNode : BTNode
{
    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Leaf;
    }

    public override BT_CHILD_NODE_NUM GetChildNodeNum()
    {
        return BT_CHILD_NODE_NUM.Zero;
    }

    protected override int LoadChildNodes(BTNodeData[] chlidNodes)
    {
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        return null;
    }
}
