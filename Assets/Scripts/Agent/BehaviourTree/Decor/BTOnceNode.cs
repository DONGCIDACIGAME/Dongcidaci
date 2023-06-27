/// <summary>
/// 该节点的子节点若成功执行过一次，则后续不再执行，直接返回succeed
/// </summary>
public class BTOnceNode : BTDecorNode
{
    private bool mFinished;

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Decor_Once;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        if (mFinished)
            return BTDefine.BT_ExcuteResult_Succeed;

        int ret = mChildNode.Excute(deltaTime);
        if (ret == BTDefine.BT_ExcuteResult_Failed)
            return BTDefine.BT_ExcuteResult_Failed;

        if (ret == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        if (ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            mFinished = true;
            return BTDefine.BT_ExcuteResult_Succeed;
        }

        return InvalidExcuteResult();
    }

    public override void Reset()
    {
        base.Reset();
        mFinished = false;
    }

    protected override void CustomDispose()
    {
        base.CustomDispose();
        mFinished = false;
    }

}
