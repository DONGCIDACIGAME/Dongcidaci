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

    public BTCompositeNode()
    {
        mChildNodes = new List<BTNode>();
    }

    public List<BTNode> GetChildNodes()
    {
        return mChildNodes;
    }

    public void AddChildNode(BTNode node)
    {
        if(node == null)
        {
            Log.Error(LogLevel.Normal, "[0] AddChildNode Failed, child node is null", NodeName);
            return;
        }

        if (mChildNodes.Contains(node))
            return;

        mChildNodes.Add(node);
        node.SetParentNode(this);
    }

    public override void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        base.Initialize(excutor, context);
        
        foreach(BTNode childNode in mChildNodes)
        {
            if(childNode != null)
            {
                childNode.Initialize(excutor, context);
            }
        }
    }

    public void RemoveChildNode(BTNode node)
    {
        if (node == null)
        {
            Log.Error(LogLevel.Normal, "[0] RemoveChildNode Failed, target node is null", NodeName);
            return;
        }

        if (!mChildNodes.Contains(node))
        {
            Log.Error(LogLevel.Normal, "[0] RemoveChildNode Failed, target node doesn't in child nodes, node name:{1}", NodeName, node.NodeName);
            return;
        }

        node.Dispose();
        mChildNodes.Remove(node);
    }

    private int GetNodeIndexInChilds(BTNode node)
    {
        if (node == null)
        {
            Log.Error(LogLevel.Normal, "[0] GetNodeIndexInChilds Failed, target node is null");
            return -1;
        }

        int index = -1;
        for (int i = 0; i < mChildNodes.Count; i++)
        {
            if (mChildNodes[i].Equals(node))
            {
                index = i;
            }
        }

        return index;
    }

    public void MoveNodeForward(BTNode node)
    {
        int index = GetNodeIndexInChilds(node);
        if(index < 0)
        {
            Log.Error(LogLevel.Normal, "[0] MoveNodeForward Failed, target node doesn't in child nodes, node name:{1}", NodeName, node.NodeName);
            return;
        }

        // 已经是第一个
        if (index == 0)
            return;

        BTNode temp = mChildNodes[index - 1];
        mChildNodes[index - 1] = node;
        mChildNodes[index] = temp;
    }

    public void MoveNodeBackward(BTNode node)
    {
        int index = GetNodeIndexInChilds(node);
        if (index < 0)
        {
            Log.Error(LogLevel.Normal, "[0] MoveNodeAfterward Failed, target node doesn't in child nodes, node name:{1}", NodeName, node.NodeName);
            return;
        }

        // 已经是最后一个
        if (index == mChildNodes.Count -1)
            return;

        BTNode temp = mChildNodes[index + 1];
        mChildNodes[index + 1] = node;
        mChildNodes[index] = temp;
    }

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

    public override BT_CHILD_NODE_NUM GetChildeNodeNum()
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
        if (mChildNodes == null || mChildNodes.Count == 0)
            return;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            node.Dispose();
        }

        mChildNodes.Clear();
    }
}
