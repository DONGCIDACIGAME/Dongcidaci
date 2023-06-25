[System.Serializable]
public class BTNodeData
{
    /// <summary>
    /// 节点名称
    /// 方便策划配置时快速理解节点含义
    /// </summary>
    public string nodeName;

    /// <summary>
    /// 节点描述
    ///  节点的一些备注，可为空
    /// </summary>
    public string nodeDesc;

    /// <summary>
    /// 结点的大类型
    /// </summary>
    public int nodeType;

    /// <summary>
    /// 结点的详细类型
    /// </summary>
    public int nodeDetailType;

    /// <summary>
    /// 所有的节点参数数据
    /// </summary>
    public BTNodeArg[] Args;

    /// <summary>
    /// 所有子节点
    /// </summary>
    public BTNodeData[] ChildNodes;
}
