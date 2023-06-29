using UnityEngine;

public class BTAgentChangeTowardsNode : BTLeafNode
{
    private int mChangeTowardsType;

    public void SetChangeTowardsType(int changeTowardsType)
    {
        mChangeTowardsType = changeTowardsType;
    }

    public int GetChangeTowardsType()
    {
        return mChangeTowardsType;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetChangeTowardsType(value);
        return BTDefine.BT_LoadNodeResult_Succeed;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("ChangeTowardsType", mChangeTowardsType);
        return new BTNodeArg[] { arg1 };
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if(mChangeTowardsType != BTDefine.BT_ChangeTowardsTo_Random
            && mChangeTowardsType != BTDefine.BT_ChangeTowardsTo_Invert
            && mChangeTowardsType != BTDefine.BT_ChangeTowardsTo_GivenTarget)
        {
            info = string.Format("Node {0} has invalid change towards type:{1}", NodeName, mChangeTowardsType);
            return false;
        }

        return true;
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTAgentChangeTowardsNode Excute Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if(mChangeTowardsType == BTDefine.BT_ChangeTowardsTo_Random)
        {
            Vector3 towards = new Vector3(Random.Range(0f, 1f), 0, Random.Range(0f, 1f));
            mExcutor.MoveControl.TurnTo(towards);
        }
        else if(mChangeTowardsType == BTDefine.BT_ChangeTowardsTo_Invert)
        {
            Vector3 towards = mExcutor.GetTowards();
            Vector3 invertTowards = new Vector3(-towards.x, 0, -towards.z);
            mExcutor.MoveControl.TurnTo(invertTowards);
        }
        else if(mChangeTowardsType == BTDefine.BT_ChangeTowardsTo_GivenTarget)
        {
            if(!mContext.TryGetValue("TargetEntity", out object obj))
            {
                Log.Error(LogLevel.Normal, "BTAgentChangeTowardsNode Excute Error, no TargetEntity!");
                return BTDefine.BT_ExcuteResult_Failed;
            }

            MapEntity targetEntity = obj as MapEntity;
            if(targetEntity == null)
            {
                Log.Error(LogLevel.Normal, "BTAgentChangeTowardsNode Excute Error, TargetEntity must be MapEntity!");
                return BTDefine.BT_ExcuteResult_Failed;
            }

            Vector3 towards = targetEntity.GetPosition() - mExcutor.GetPosition();
            mExcutor.MoveControl.TurnTo(towards);
        }

        Log.Error(LogLevel.Normal, "BTAgentChangeTowardsNode Excute Error, mChangeTowardsType is not valid!");
        return BTDefine.BT_ExcuteResult_Failed;
    }

    public override void Reset()
    {
        mChangeTowardsType = BTDefine.BT_ChangeTowardsTo_NotDefine;
    }

    protected override void CustomDispose()
    {
        mChangeTowardsType = BTDefine.BT_ChangeTowardsTo_NotDefine;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_ChangeTowards;
    }
}
