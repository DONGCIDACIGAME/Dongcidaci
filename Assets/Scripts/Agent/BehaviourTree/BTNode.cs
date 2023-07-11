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
    /// 子节点
    /// </summary>
    protected List<BTNode> mChildNodes;

    /// <summary>
    /// 是否开启日志
    /// </summary>
    protected bool mLogEnable;

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


    public BTNode()
    {
        mChildNodes = new List<BTNode>();
    }


    public void SetLogEnable(bool enable)
    {
        mLogEnable = enable;
        foreach(BTNode node in mChildNodes)
        {
            if(node != null)
            {
                node.SetLogEnable(enable);
            }
        }
    }

    private string mLastLog;

    protected void PrintLog(string additionalInfo)
    {
        string print = string.Format("<color=grey>Excute Node [{0}]-[{1}]---AdditionalInfo:{2}</color>",NodeName, BehaviourTreeHelper.GetNodeDetailTypeName(GetNodeDetailType()) , additionalInfo);

        if (print.Equals(mLastLog))
            return;

        mLastLog = print;
        Log.Logic(LogLevel.Info, print);
    }

    public void UnpackChildNode(BTNode node)
    {
        if (node == null)
        {
            Log.Error(LogLevel.Normal, "[0] UnpackChildNode Failed, target node is null", NodeName);
            return;
        }

        if (!mChildNodes.Contains(node))
        {
            Log.Error(LogLevel.Normal, "[0] UnpackChildNode Failed, target node doesn't in child nodes, node name:{1}", NodeName, node.NodeName);
            return;
        }

        mChildNodes.Remove(node);
    }

    public void UnpackAllChilds()
    {
        foreach (BTNode node in mChildNodes)
        {
            node.UnpackAllChilds();
        }

        mChildNodes.Clear();
    }

    /// <summary>
    /// 添加子节点
    /// 已经根据子节点的数量做了相应处理
    /// </summary>
    /// <param name="node"></param>
    public void AddChildNode(BTNode node)
    {
        BT_CHILD_NODE_NUM childNodeNum = GetChildNodeNum();
        if (childNodeNum == BT_CHILD_NODE_NUM.Zero)
            return;

        if (node == null)
        {
            Log.Error(LogLevel.Normal, "[0] AddChildNode Failed, child node is null", NodeName);
            return;
        }

        if(childNodeNum == BT_CHILD_NODE_NUM.One)
        {
            mChildNodes.Clear();
        }

        mChildNodes.Add(node);
        node.SetParentNode(this);
    }

    public void RemoveChildNode(BTNode node)
    {
        UnpackChildNode(node);

        if (node != null)
        {
            node.Dispose();
        }
    }

    public List<BTNode> GetChildNodes()
    {
        return mChildNodes;
    }

    public int GetNodeIndexInChilds(BTNode node)
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


    public virtual void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        mExcutor = excutor;
        mContext = context;

        foreach(BTNode childNode in mChildNodes)
        {
            childNode.Initialize(excutor, context);
        }
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
    public abstract BT_CHILD_NODE_NUM GetChildNodeNum();
    public abstract int GetNodeArgNum();
    protected abstract BTNodeArg[] GetNodeArgs();
    protected abstract int ParseNodeArgs(BTNodeArg[] args);
    protected abstract int LoadChildNodes(BTNodeData[] chlidNodes);
    protected abstract BTNodeData[] GetChildNodesData();
    public abstract int Excute(float deltaTime);


    /// <summary>
    /// Reset用于行为树执行一轮后开始新的循环时，将节点状态重置（树的参数不重置）
    /// </summary>
    public virtual void Reset()
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            if (node != null)
                node.Reset();
        }
    }

    /// <summary>
    /// Dipsose用于将行为树节点的数据全部抹除，变为未初始化的状态
    /// </summary>
    protected virtual void CustomDispose()
    {
        if (mChildNodes == null || mChildNodes.Count == 0)
            return;

        for (int i = 0; i < mChildNodes.Count; i++)
        {
            BTNode node = mChildNodes[i];
            if(node != null)
                node.Dispose();
        }

        mChildNodes.Clear();
    }



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
        BT_CHILD_NODE_NUM childNum = GetChildNodeNum();
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
            Log.Error(LogLevel.Normal, "[{0}] LoadFromBTNodeData Failed, right child num:{1}!", data.nodeName, GetChildNodeNum());
            return BTDefine.BT_LoadNodeResult_Failed_InvalidChildNum;
        }

        result = LoadChildNodes(data.ChildNodes);
        if(result != BTDefine.BT_LoadNodeResult_Succeed)
        {
            return result;
        }

        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    /// <summary>
    /// 检查节点是否合法，用于保存前对节点进行子节点数量、类型、数据的检查
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public abstract bool BTNodeDataCheck(ref string info);

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

    /// <summary>
    /// 返回执行失败，并打印：BTNode Excute Error, invalid Excute result!
    /// </summary>
    /// <returns></returns>
    protected int InvalidExcuteResult()
    {
        Log.Error(LogLevel.Normal, "BTNode Excute Error, invalid Excute result!");
        return BTDefine.BT_ExcuteResult_Failed;
    }

    public void MoveNodeForward(BTNode node)
    {
        int index = GetNodeIndexInChilds(node);
        if (index < 0)
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
        if (index == mChildNodes.Count - 1)
            return;

        BTNode temp = mChildNodes[index + 1];
        mChildNodes[index + 1] = node;
        mChildNodes[index] = temp;
    }

    public BTNode Copy()
    {
        BTNodeData data = ToBTNodeData();
        BTNode node = BehaviourTreeHelper.CreateBTNode(data);
        node.LoadFromBTNodeData(data);
        return node;
    }

    public void Dispose()
    {
        mContext = null;
        mExcutor = null;
        mParentNode = null;
        mLogEnable = false;
        UnpackAllChilds();
        CustomDispose();
    }
}
