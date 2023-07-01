/// <summary>
/// 重置节点
/// 如果子节点返回成功，则重置子节点的状态
/// </summary>
public class BTResetNode : BTDecorNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_Reset;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;
        
        int ret = mChildNode.Excute(deltaTime);
        if (ret == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Failed;

        if (ret == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if(ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            mChildNode.Reset();
            return BTDefine.BT_ExcuteResult_Succeed;
        }

        return InvalidExcuteResult();
    }
}
