public class BTIfElseNode : BTCompositeNode
{
    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Composite_IfElse;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNodes == null || mChildNodes.Count != 3)
            return BTDefine.BT_ExcuteResult_Failed;

        BTNode condition = mChildNodes[0];
        BTNode action1 = mChildNodes[1];
        BTNode action2 = mChildNodes[2];

        int ret = condition.Excute(deltaTime);

        if (ret == BTDefine.BT_ExcuteResult_Running)
            return BTDefine.BT_ExcuteResult_Running;

        int ret2 = BTDefine.BT_ExcuteResult_Failed;
        if (ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            ret2 = action1.Excute(deltaTime);
        }
        else if(ret == BTDefine.BT_ExcuteResult_Failed)
        {
            ret2 = action2.Excute(deltaTime);
        }

        if(ret2 == BTDefine.BT_ExcuteResult_Succeed)
        {
            condition.Reset();
            action1.Reset();
            action2.Reset();
        }
        return ret2;
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if(mChildNodes.Count != 3)
        {
            info = "BTIfElseNode must have 3 child node!";
            return false;
        }

        return true;
    }
}
