/// <summary>
/// 叶子结点，用来承载各种行为和逻辑判断
/// 叶子结点是终止结点，不再向下延伸
/// </summary>
public abstract class BTLeafNode : BTNode
{
    public BTLeafNode()
    {

    }

    public override int LoadFromBTNodeData(BTNodeData data)
    {
        return base.LoadFromBTNodeData(data);
    }

    public override BTNodeData ToBTNodeData()
    {
        BTNodeData data = base.ToBTNodeData();
        return data;
    }
}
