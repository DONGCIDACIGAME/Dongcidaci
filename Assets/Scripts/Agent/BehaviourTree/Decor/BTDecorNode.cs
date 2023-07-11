using System.Collections.Generic;
/// <summary>
/// 装饰节点,包括以下几种类型
/// 1. invert节点：将返回结果反转
/// 2. repeate节点：将重复执行子节点n次
/// ...
/// </summary>
public abstract class BTDecorNode : BTNode
{

    public void SetChildNode(BTNode childNode)
    {
        if (childNode == null)
        {
            Log.Error(LogLevel.Normal, "BTDecorNode SetChildNode Error, child node is null!");
            return;
        }

        AddChildNode(childNode);
    }

    public BTNode GetChildNode()
    {
        if (mChildNodes.Count == 0)
            return null;

        return mChildNodes[0];
    }

    //public void UnpackChildNode(BTNode node)
    //{
    //    if(mChildNodes.Contains(node))
    //    {
    //        mChildNodes.Remove(node);
    //    }
    //}

    //public override void UnpackAllChilds()
    //{
    //    BTNode childNode = GetChildNode();
    //    if (childNode != null)
    //        childNode.UnpackChilds();

    //    mChildNodes.Clear();
    //}

    ///// <summary>
    ///// 删除子节点
    ///// 使用该方法会递归释放所有子节点
    ///// </summary>
    //public override void RemoveChildNode(BTNode node)
    //{
    //    UnpackChildNode(node);

    //    if(node != null)
    //    {
    //        node.Dispose();
    //    }
    //}

    public override int GetNodeArgNum()
    {
        return 0;
    }

    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Decor;
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

    protected override int LoadChildNodes(BTNodeData[] childNodes)
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

    public override bool BTNodeDataCheck(ref string info)
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
        {
            info = "装饰节点必须有子节点!";
            return false;
        }

        return true;
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
}
