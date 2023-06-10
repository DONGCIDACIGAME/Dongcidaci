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
    /// <summary>
    /// 所有子节点
    /// </summary>
    protected List<BTNode> mChildNodes;

    protected void AddChildNode(BTNode node)
    {
        if(node == null)
        {
            Log.Error(LogLevel.Normal, "[0] AddChildNode Failed, child node is null", NodeName);
            return;
        }

        node.Initialize(mExcutor, mContext);
        mChildNodes.Add(node);
    }

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
        BTNodeData data = base.ToBTNodeData();
        BTNodeData[] childNodes = new BTNodeData[mChildNodes.Count];
        for(int i = 0;i< mChildNodes.Count; i++)
        {
            childNodes[i] = mChildNodes[i].ToBTNodeData();
        }
        data.ChildNodes = childNodes;
        return data;
    }

    public override void Reset()
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            node.Reset();
        }
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();

        if (mChildNodes == null || mChildNodes.Count == 0)
            return;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            node.Dispose();
        }
    }
}
