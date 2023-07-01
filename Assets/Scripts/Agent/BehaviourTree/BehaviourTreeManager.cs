using GameEngine;
using System.Collections.Generic;

/// <summary>
/// 行为树管理器
/// </summary>
public class BehaviourTreeManager : ModuleManager<BehaviourTreeManager>
{
    /// <summary>
    /// 已经加载过的行为树数据都缓存在这里面
    /// 下次创建相同行为树时直接从缓存数据创建
    /// </summary>
    private Dictionary<string, BTNodeData> mLoadedBTNodeDatas;

    public override void Initialize()
    {
        mLoadedBTNodeDatas = new Dictionary<string, BTNodeData>();
    }

    /// <summary>
    /// 加载行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTTreeEntry LoadTreeWithFileFullPath(string filePath, bool forceReload = false)
    {
        BTNodeData data = LoadTreeData(filePath, forceReload);
        if (data == null)
            return null;
        BTTreeEntry tree = CreateBTNode(data) as BTTreeEntry;
        tree.LoadFromBTNodeData(data);
        return tree;
    }

    /// <summary>
    /// 加载行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTTreeEntry LoadTreeWithTreeName(string treeName, bool forceReload = false)
    {
        string fileFullPath = BehaviourTreeHelper.TreeNameToFileFullPath(treeName);
        return LoadTreeWithFileFullPath(fileFullPath, forceReload);
    }


    /// <summary>
    /// 加载行为树数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public BTNodeData LoadTreeData(string filePath, bool forceReload = false)
    {
        // 查询缓存数据
        BTNodeData data = null;
        if (forceReload || !mLoadedBTNodeDatas.TryGetValue(filePath, out data))
        {
            data = BehaviourTreeHelper.LoadBTNodeData(filePath);
        }

        if (data == null)
            return null;
        if (!mLoadedBTNodeDatas.ContainsKey(filePath))
        {
            mLoadedBTNodeDatas.Add(filePath, data);
        }

        return data;
    }

    /// <summary>
    /// 保存行为树
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="tree"></param>
    public void SaveTree(string filePath, BTTree tree)
    {
        if(tree == null)
        {
            Log.Error(LogLevel.Normal, "SaveTree Failed, tree is null!");
            return;
        }

        BTNodeData data = tree.ToBTNodeData();
        BehaviourTreeHelper.SaveBTNodeData(filePath, data);
    }

    /// <summary>
    /// 创建行为树节点
    /// </summary>
    /// <param name="data">节点数据</param>
    /// <returns></returns>
    public  BTNode CreateBTNode(BTNodeData data)
    {
        if (data == null)
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
    /// ---------------------------增加新节点时需要更新
    /// <returns></returns>
    public BTNode CreateBTNode(int nodeType, int nodeDetailType)
    {

        if (nodeType == BTDefine.BT_Node_Type_Tree)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_Tree_Entry)
                return new BTTreeEntry();

            if (nodeDetailType == BTDefine.BT_Node_Type_Tree_ChildTree)
                return new BTChildTree();
        }


        if (nodeType == BTDefine.BT_Node_Type_Composite)
        {
            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Sequence)
                return new BTSequenceNode();

            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Selector)
                return new BTSelectNode();

            if (nodeDetailType == BTDefine.BT_Node_DetailType_Composite_Parallel)
                return new BTParallelNode();
        }


        if (nodeType == BTDefine.BT_Node_Type_Decor)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Invert)
                return new BTInvertNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Repeat)
                return new BTRepeatNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Once)
                return new BTOnceNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_Reset)
                return new BTResetNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Decor_UntilTrue)
                return new BTUntilTrueNode();
        }

        if (nodeType == BTDefine.BT_Node_Type_Leaf)
        {
            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_WaitTime)
                return new BTWaitTimeNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_WaitFrame)
                return new BTWaitFrameNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_WaitMeter)
                return new BTWaitMeterNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_DetectAgent)
                return new BTDetectAgentNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_ChangeTowards)
                return new BTAgentChangeTowardsNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_MoveTime)
                return new BTMoveTimeNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_MoveMeter)
                return new BTMoveMeterNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_MoveDistance)
                return new BTMoveDistanceNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_MoveOneFrame)
                return new BTMoveOneFrameNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_MoveToPosition)
                return new BTMoveToPositionNode();

            if (nodeDetailType == BTDefine.BT_Node_Type_Leaf_CheckDistance)
                return new BTCheckDistanceToTarget();
        }

        Log.Error(LogLevel.Normal, "BTNodeFactory CreateBTNode Failed, nodeType:{0}, nodeDetailType:{1}, no matching class!", nodeType, nodeDetailType);
        return null;
    }

    public override void Dispose()
    {
        mLoadedBTNodeDatas = null;
    }
}
