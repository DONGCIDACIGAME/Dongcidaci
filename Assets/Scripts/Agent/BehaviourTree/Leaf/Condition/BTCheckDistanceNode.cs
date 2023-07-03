public abstract class BTCheckDistanceNode : BTLeafNode
{
    protected float mCheckDistance;
    protected int mOperator;


    public void SetDistance(float distance)
    {
        mCheckDistance = distance;
    }

    public float GetDistance()
    {
        return mCheckDistance;
    }

    public void SetOperator(int op)
    {
        mOperator = op;
    }

    public int GetOperator()
    {
        return mOperator;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        result = BehaviourTreeHelper.ParseFloat(args[1], out float value2);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetOperator(value1);
        SetDistance(value2);
        return BTDefine.BT_ExcuteResult_Succeed;
    }


    public override bool BTNodeDataCheck(ref string info)
    {
        if (mOperator != BTDefine.BT_Operator_Equal
            && mOperator != BTDefine.BT_Operator_GreaterThan
            && mOperator != BTDefine.BT_Operator_GreaterEqual
            && mOperator != BTDefine.BT_Operator_LessEqual
            && mOperator != BTDefine.BT_Operator_LessThan)
        {
            info = string.Format("Node {0} has Invalid Operator Type:{1}", NodeName, mOperator);
            return false;
        }

        return true;
    }
}
