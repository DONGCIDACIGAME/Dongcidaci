using UnityEngine;

public class BTMoveToPositionNode : BTMoveNode
{
    private Vector3 mTargetPos;

    public void SetTargetPosition(Vector3 targetPos)
    {
        mTargetPos = targetPos;
    }

    public Vector3 GetTargetPosition()
    {
        return mTargetPos;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseVector3(args[0], out Vector3 value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetTargetPosition(value);
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_Vector3("TargetPosition", mTargetPos);
        return new BTNodeArg[] { arg1};
    }


    public override bool BTNodeDataCheck(ref string info)
    {
        return true;
    }

    public override int Excute(float deltaTime)
    {
        // TODO:需要寻路模块支持
        return 0;
    }


    public override void Reset()
    {
        
    }

    protected override void CustomDispose()
    {
        mTargetPos = Vector3.zero;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_MoveToPosition;
    }
}
