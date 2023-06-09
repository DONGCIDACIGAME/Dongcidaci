/// <summary>
/// 装饰节点,包括以下几种类型
/// 1. invert节点：将返回结果反转
/// 2. repeate节点：将重复执行子节点n次
/// 
/// </summary>
public abstract class BTDecorNode : BTNode
{
    protected BTNode mChildNode;
    public BTDecorNode(BTNode childNode)
    {
        mChildNode = childNode;
    }
}
