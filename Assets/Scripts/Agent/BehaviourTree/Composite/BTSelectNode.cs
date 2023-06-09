/// <summary>
/// 选择执行节点，当遇到第一个执行结果为succeed时即返回succeed
/// </summary>
public class BTSelectNode : BTCompositeNode
{
    public BTSelectNode(BTNode[] childNodes) : base(childNodes)
    {
    }

    public override int Excute()
    {
        for(int i = 0;i<mChildNodes.Length;i++)
        {
            BTNode node = mChildNodes[i];
            int result = node.Excute();

            if (result == BTDefine.BT_CheckResult_Succeed)
                return BTDefine.BT_CheckResult_Succeed;

            if (result == BTDefine.BT_CheckResult_Keep)
                return BTDefine.BT_CheckResult_Keep;
        }

        return BTDefine.BT_CheckResult_Failed;
    }
}