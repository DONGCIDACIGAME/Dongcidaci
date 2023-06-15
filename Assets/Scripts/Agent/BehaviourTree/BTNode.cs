using GameEngine;
using System.Collections.Generic;

public abstract class BTNode : IGameDisposable
{
    /// <summary>
    /// 父节点
    /// </summary>
    private BTNode mParentNode;

    /// <summary>
    /// 执行者
    /// </summary>
    protected Agent mExcutor;

    /// <summary>
    /// 上下文对象
    /// </summary>
    protected Dictionary<string, object> mContext;

    /// <summary>
    /// 节点名称
    /// </summary>
    public string NodeName;

    /// <summary>
    /// 节点备注
    /// </summary>
    public string NodeDesc;

    public void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        mExcutor = excutor;
        mContext = context;
    }

    public void SetParentNode(BTNode node)
    {
        mParentNode = node;
    }

    public BTNode GetParentNode()
    {
        return mParentNode;
    }

    public abstract int GetNodeType();
    public abstract int GetNodeDetailType();
    public abstract BT_CHILD_NODE_NUM GetChildeNodeNum();
    public abstract int GetNodeArgNum();
    protected abstract BTNodeArg[] GetNodeArgs();
    protected abstract int ParseNodeArgs(BTNodeArg[] args);
    protected abstract int LoadChildNodes(BTNodeData[] chlidNodes);
    protected abstract BTNodeData[] GetChildNodesData();
    public abstract int Excute(float deltaTime);
    public abstract void Reset();

    protected virtual void CustomDispose() { }

    private bool CheckArgNum(BTNodeArg[] args)
    {
        int rightArgNum = GetNodeArgNum();

        if(args == null)
        {
            if (rightArgNum == 0)
                return true;

            return false;
        }

        return args.Length == rightArgNum;
    }

    private bool CheckChildNodeNum(BTNodeData[] childNodes)
    {
        BT_CHILD_NODE_NUM childNum = GetChildeNodeNum();
        if(childNodes == null)
        {
            if (childNum == 0)
                return true;

            return false;
        }

        if(childNum == BT_CHILD_NODE_NUM.One)
        {
            return childNodes.Length == 1;
        }

        if(childNum == BT_CHILD_NODE_NUM.AtLeastOne)
        {
            return childNodes.Length >= 1;
        }

        return false;
    }

    public int LoadFromBTNodeData(BTNodeData data)
    {
        if (data == null)
        {
            Log.Error(LogLevel.Normal, "LoadFromBTNodeData Failed,data is null");
            return BTDefine.BT_LoadNodeResult_Failed_NullData;
        }

        NodeName = data.nodeName;
        NodeDesc = data.nodeDesc;

        int nodeType = GetNodeType();
        int nodeDetailType = GetNodeDetailType();
        if (data.nodeType != nodeType)
        {
            Log.Error(LogLevel.Normal, "[{0}-{1}-{2}] LoadFromBTNodeData Failed, wrong node type:{3}",data.nodeName, nodeType, nodeDetailType, data.nodeType);
            return BTDefine.BT_LoadNodeResult_Failed_WrongType;
        }

        if (data.nodeDetailType != GetNodeDetailType())
        {
            Log.Error(LogLevel.Normal, "[{0}-{1}-{2}] LoadFromBTNodeData Failed, wrong node detail type:{3}", data.nodeName, nodeType, nodeDetailType, data.nodeDetailType);
            return BTDefine.BT_LoadNodeResult_Failed_WrongType;
        }


        if (!CheckArgNum(data.Args))
        {
            Log.Error(LogLevel.Normal, "[{0}] LoadFromBTNodeData Failed, right arg num:{1}!", data.nodeName, GetNodeArgNum());
            return BTDefine.BT_LoadNodeResult_Failed_InvalidArgNum;
        }

        int result = ParseNodeArgs(data.Args);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
        {
            return result;
        }

        if (!CheckChildNodeNum(data.ChildNodes))
        {
            Log.Error(LogLevel.Normal, "[{0}] LoadFromBTNodeData Failed, right child num:{1}!", data.nodeName, GetChildeNodeNum());
            return BTDefine.BT_LoadNodeResult_Failed_InvalidChildNum;
        }

        result = LoadChildNodes(data.ChildNodes);
        if(result != BTDefine.BT_LoadNodeResult_Succeed)
        {
            return result;
        }

        return BTDefine.BT_LoadNodeResult_Succeed;
    }
    public BTNodeData ToBTNodeData()
    {
        BTNodeData data = new BTNodeData();
        data.nodeName = NodeName;
        data.nodeDesc = NodeDesc;
        data.nodeType = GetNodeType();
        data.nodeDetailType = GetNodeDetailType();
        data.ChildNodes = GetChildNodesData();
        data.Args = GetNodeArgs();
        return data;
    }

    public void Dispose()
    {
        mContext = null;
        CustomDispose();
    }
}
