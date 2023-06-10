using GameEngine;
using System.Collections.Generic;

public abstract class BTNode : IGameDisposable
{
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

    public int NodeType;
    public int NodeDetailType;

    public void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        mExcutor = excutor;
        mContext = context;
    }

    public abstract int Excute(float deltaTime);
    public abstract void Reset();

    protected virtual void CustomDispose() { }

    public virtual int LoadFromBTNodeData(BTNodeData data)
    {
        if (data == null)
        {
            Log.Error(LogLevel.Normal, "LoadFromBTNodeData Failed,data is null");
            return BTDefine.BT_LoadNodeResult_Failed_NullData;
        }

        NodeName = data.nodeName;
        NodeDesc = data.nodeDesc;


        return BTDefine.BT_LoadNodeResult_Succeed;
    }
    public virtual BTNodeData ToBTNodeData()
    {
        BTNodeData data = new BTNodeData();
        data.nodeName = NodeName;
        data.nodeDesc = NodeDesc;
        data.nodeType = NodeType;
        data.nodeDetailType = NodeDetailType;
        return data;
    }

    public void Dispose()
    {
        mContext = null;
        CustomDispose();
    }
}
