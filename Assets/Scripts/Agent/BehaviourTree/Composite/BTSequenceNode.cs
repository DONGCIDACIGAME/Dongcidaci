/// <summary>
/// 顺序执行节点，只有所有子节点返回的结果都是succeed时才返回succeed
/// </summary>
public class BTSequenceNode : BTCompositeNode
{
    public BTSequenceNode(BTNode[] childNodes) :base(childNodes)
    {
        
    }

    public override int Excute()
    {
        for(int i=0;i<mChildNodes.Length;i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute();

            if (result == BTDefine.BT_CheckResult_Failed)
                return BTDefine.BT_CheckResult_Failed;

            if (result == BTDefine.BT_CheckResult_Keep)
                return BTDefine.BT_CheckResult_Keep;
        }

        return BTDefine.BT_CheckResult_Succeed;
    }
}
