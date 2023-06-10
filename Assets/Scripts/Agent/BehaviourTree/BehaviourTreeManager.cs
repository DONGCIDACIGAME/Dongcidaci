using GameEngine;

/// <summary>
/// 行为树管理器
/// </summary>
public class BehaviourTreeManager : ModuleManager<BehaviourTreeManager>
{
    // TODO: 把已加载的行为树数据模版保存在这里，不要每次加载行为树数据都从磁盘上读取一遍文件


    /// <summary>
    /// 创建行为树节点
    /// </summary>
    /// <param name="data">节点数据</param>
    /// <returns></returns>
    public BTNode CreateBTNode(BTNodeData data)
    {
        if(data == null)
        {
            Log.Error(LogLevel.Normal, "BTNodeFactory CreateBTNode Failed, node data is null!");
            return null;
        }

        return CreateBTNode(data.nodeType, data.nodeDetailType);
    }


    /// <summary>
    /// 创建行为树节点
    /// </summary>
    /// <param name="nodeType">节点大类型</param>
    /// <param name="nodeDetailType">节点详细类型</param>
    /// <returns></returns>
    public BTNode CreateBTNode(int nodeType, int nodeDetailType)
    {
        if(nodeType == BTDefine.BT_Node_Type_TreeRoot)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_TreeRoot_Entry)
                return new BTTree();
        }

        if(nodeType == BTDefine.BT_Node_Type_Composite)
        {
            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Sequence)
                return new BTSequenceNode();

            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Selector)
                return new BTSelectNode();

            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Parallel)
                return new BTParallelNode();
        }

        if(nodeType == BTDefine.BT_Node_Type_Decor)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Invert)
                return new BTInvertNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Repeat)
                return new BTRepeatNode(0);
        }

        if(nodeType == BTDefine.BT_Node_Type_Leaf)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_WaitFrame)
                return new BTWaitTimeNode(0);

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_WaitFrame)
                return new BTWaitFrameNode(0);
        }


        Log.Error(LogLevel.Normal, "BTNodeFactory CreateBTNode Failed, nodeType:{0}, nodeDetailType:{1}, no matching class!", nodeType, nodeDetailType);
        return null;
    }

    public override void Dispose()
    {
        
    }

    public override void Initialize()
    {
        
    }
}
