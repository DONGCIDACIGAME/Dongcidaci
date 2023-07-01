using System.Collections.Generic;
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
        if (childNode == null)
        {
            Log.Error(LogLevel.Normal, "BTDecorNode SetChildNode Error, child node is null!");
            return;
        }

        mChildNode = childNode;
        childNode.SetParentNode(this);
    }

    public override void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        base.Initialize(excutor, context);

        if (mChildNode != null)
            mChildNode.Initialize(excutor, context);
    }

    public BTNode GetChildNode()
    {
        return mChildNode;
    }

    public void UnpackChildNode()
    {
        mChildNode = null;
    }

    public override void UnpackChilds()
    {
        // 子节点
        if (mChildNode != null)
            mChildNode.UnpackChilds();

        RemoveChildNode();
    }

    public void RemoveChildNode()
    {
        if(mChildNode != null)
            mChildNode.Dispose();

        mChildNode = null;
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

    protected override int LoadChildNodes(BTNodeData[] childNodes)
    {
        // 加载子节点
        BTNodeData nodeData = childNodes[0];
        BTNode node = BehaviourTreeManager.Ins.CreateBTNode(nodeData);
        int childLoadResult = node.LoadFromBTNodeData(nodeData);

        // 严格要求所有节点都正确加载，这个AI行为树才可以使用
        if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
            return childLoadResult;

        SetChildNode(node);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mChildNode == null)
        {
            info = "装饰节点必须有子节点!";
            return false;
        }

        return true;
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
        if (mChildNode == null)
            return;

        mChildNode.Dispose();
        mChildNode = null;
    }
}
