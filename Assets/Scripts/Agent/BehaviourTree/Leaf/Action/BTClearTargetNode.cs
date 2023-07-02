
public class BTClearTargetNode : BTLeafNode
{
    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        if(mContext.ContainsKey("TargetEntity"))
        {
            mContext.Remove("TargetEntity");
        }

        PrintLog("ret: Succeed");
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_ClearTarget;
    }

    public override void Reset()
    {
        
    }

    protected override void CustomDispose()
    {
        
    }
}
