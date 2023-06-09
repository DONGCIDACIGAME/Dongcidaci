/// <summary>
/// 组合节点，包括以下几种类型
/// 1. Sequence节点：顺序完成节点
/// 2. Select节点：选择完成节点
/// 3. Parallel节点：并行完成节点
/// 4. 其他根据需要再进行添加
/// </summary>
public abstract class BTCompositeNode : BTNode
{
    protected BTNode[] mChildNodes;
    public BTCompositeNode(BTNode[] childNodes)
    {
        mChildNodes = childNodes;
    }
}
