/// <summary>
/// 装饰节点,包括以下几种类型
/// 1. invert节点：将返回结果反转
/// 2. repeate节点：将重复执行子节点n次
/// 
/// </summary>
public abstract class BTDecorNode : BTNode
{
    protected BTNode mChildNode;

    public override int LoadFromBTNodeData(BTNodeData data)
    {
        int result = base.LoadFromBTNodeData(data);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        if (data.nodeType != BTDefine.BT_Node_Type_Composite)
        {
            Log.Error(LogLevel.Normal, "BTCompositeNode LoadFromBTNodeData Failed, wrong node type:{0}", data.nodeType);
            return BTDefine.BT_LoadNodeResult_Failed_WrongType;
        }

        NodeType = data.nodeType;

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    public override BTNodeData ToBTNodeData()
    {
        BTNodeData data = mChildNode.ToBTNodeData();
        BTNodeData childNodeData = mChildNode.ToBTNodeData();
        data.ChildNodes = new BTNodeData[] { childNodeData };
        return data;
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
