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
        bool hasOperator = false;
        for(int i = 0;i<BTDefine.BT_ALL_OPERATORS.Length;i++)
        {
            if(BTDefine.BT_ALL_OPERATORS[i].Type == mOperator)
            {
                hasOperator = true;
                break;
            }
        }

        if (!hasOperator)
        {
            info = string.Format("Node {0} has Invalid Operator Type:{1}", NodeName, mOperator);
            return false;
        }

        if (mOperator == BTDefine.BT_Operator_Undefine)
        {
            info = string.Format("Node {0} operator undefine is invalid", NodeName);
            return false;
        }

        return true;
    }
}
