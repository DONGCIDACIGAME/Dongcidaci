public class BTTree : BTNode
{
    private BTNode mChildNode;
    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_CheckResult_Failed;

        return mChildNode.Excute(deltaTime);
    }

    public override int LoadFromBTNodeData(BTNodeData data)
    {
        int result = base.LoadFromBTNodeData(data);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        if(data.nodeType != BTDefine.BT_Node_Type_TreeRoot)
        {
            Log.Error(LogLevel.Normal, "BTTree [{0}] LoadFromBTNodeData Failed, wrong node type:{1}", NodeName,  data.nodeType);
            return BTDefine.BT_LoadNodeResult_Failed_WrongType;
        }

        NodeType = data.nodeType;

        if(data.ChildNodes == null || data.ChildNodes.Length != 1)
        {
            Log.Error(LogLevel.Normal, "BTTree [{0}] LoadFromBTNodeData Failed, child node num != 1", NodeName);
            return BTDefine.BT_LoadNodeResult_Failed_InvalidChildNodeNum;
        }

        BTNodeData childNode = data.ChildNodes[0];
        // 加载子节点
        // 严格要求所有节点都正确加载，这个AI行为树才可以使用
        mChildNode = BehaviourTreeManager.Ins.CreateBTNode(childNode);
        int childLoadResult = mChildNode.LoadFromBTNodeData(childNode);
        if (childLoadResult != BTDefine.BT_LoadNodeResult_Succeed)
            return childLoadResult;

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
        if (mChildNode == null)
            return;

        mChildNode.Dispose();
    }
}