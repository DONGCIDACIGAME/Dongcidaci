using System.Collections.Generic;
/// <summary>
/// 组合节点，包括以下几种类型
/// 1. Sequence节点：顺序完成节点
/// 2. Select节点：选择完成节点
/// 3. Parallel节点：并行完成节点
/// 4. 其他根据需要再进行添加
/// </summary>
public abstract class BTCompositeNode : BTNode
{
    public override int GetNodeArgNum()
    {
        return 0;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        return null;
    }
    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Composite;
    }

    public override BT_CHILD_NODE_NUM GetChildNodeNum()
    {
        return BT_CHILD_NODE_NUM.AtLeastOne;
    }

    protected override int LoadChildNodes(BTNodeData[] chlidNodes)
    {
        // 加载子节点
        for (int i = 0; i < chlidNodes.Length; i++)
        {
            BTNodeData childNode = chlidNodes[i];
            BTNode node = BehaviourTreeManager.Ins.CreateBTNode(childNode);
            int childLoadResult = node.LoadFromBTNodeData(childNode);

            // 严格要求所有节点都正确加载，这个AI行为树才可以使用
            if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
                return childLoadResult;

            AddChildNode(node);
        }

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if(mChildNodes == null || mChildNodes.Count == 0)
        {
            info = "组合节点至少要有一个子节点!";
            return false;
        }

        return true;
    }

    protected override BTNodeData[] GetChildNodesData()
    {
        BTNodeData[] childNodes = new BTNodeData[mChildNodes.Count];
        for (int i = 0; i < mChildNodes.Count; i++)
        {
            childNodes[i] = mChildNodes[i].ToBTNodeData();
        }
        return childNodes;
    }
}
