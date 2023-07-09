using System.Collections.Generic;

public class BTCheckTargetEntityInLogicAreaNode : BTLeafNode
{
    private int mLogicAreaType;

    public void SetLogicAreaType(int logicAreaType)
    {
        mLogicAreaType = logicAreaType;
    }

    public int GetLogicAreaType()
    {
        return mLogicAreaType;
    }

    protected override int ParseNodeArgs(BTNodeArg[] args)
    {
        int result = BehaviourTreeHelper.ParseInt(args[0], out int value1);
        if (result != BTDefine.BT_LoadNodeResult_Succeed)
            return result;

        SetLogicAreaType(value1);
        return BTDefine.BT_ExcuteResult_Succeed;
    }

    public override int GetNodeArgNum()
    {
        return 1;
    }

    protected override BTNodeArg[] GetNodeArgs()
    {
        BTNodeArg arg1 = new BTNodeArg_int("LogicAreaType", mLogicAreaType);
        return new BTNodeArg[] { arg1};
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        bool hasType = false;
        for(int i = 0;i<BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length;i++)
        {
            if(BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Type == mLogicAreaType)
            {
                hasType = true;
                break;
            }
        }

        if (!hasType)
        {
            info = string.Format("Node {0} has Invalid LogicArea Type:{1}", NodeName, mLogicAreaType);
            return false;
        }

        if(mLogicAreaType == BTDefine.BT_LogicArea_Type_Undefine)
        {
            info = string.Format("Node {0} undefined area type is invalid", NodeName);
            return false;
        }

        return true;
    }

    public override int GetNodeDetailType()
    {
        return BTDefine.BT_Node_Type_Leaf_CheckTargetEntityInLogicArea;
    }

    private string GetLogicAreaName(int logicAreaType)
    {

        for (int i = 0; i < BTDefine.BT_ALL_LOGIC_AREA_TYPES.Length; i++)
        {
            if (BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Type == logicAreaType)
            {
                return BTDefine.BT_ALL_LOGIC_AREA_TYPES[i].Desc;
            }
        }

        return "undefine";
    }

    public override int Excute(float deltaTime)
    {
        if (mExcutor == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckTargetEntityInLogicAreaNode Error, mExcutor is null!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if (!mContext.TryGetValue("TargetEntity", out object obj))
        {
            Log.Error(LogLevel.Normal, "BTCheckTargetEntityInLogicAreaNode Error, no target entity!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        MapEntity target = obj as MapEntity;
        if (target == null)
        {
            Log.Error(LogLevel.Normal, "BTCheckTargetEntityInLogicAreaNode Error, target entity is not map entity!");
            return BTDefine.BT_ExcuteResult_Failed;
        }

        bool ret = false;
        switch (mLogicAreaType)
        {
            case BTDefine.BT_LogicArea_Type_Attack:
                float distance = (mExcutor.GetPosition() - target.GetPosition()).magnitude;
                ret = distance <= mExcutor.GetAttackRadius();
                break;
            case BTDefine.BT_LogicArea_Type_Interact:
                distance = (mExcutor.GetPosition() - target.GetPosition()).magnitude;
                ret = distance <= mExcutor.GetInteractRadius();
                break;
            case BTDefine.BT_LogicArea_Type_Vision:
                List <MapEntity> detectedEntities = mExcutor.DetectMapEntityInVision<MapEntity>();
                ret = (detectedEntities != null) && detectedEntities.Contains(target);
                break;
            case BTDefine.BT_LogicArea_Type_Follow:
                distance = (mExcutor.GetPosition() - target.GetPosition()).magnitude;
                ret = distance <= mExcutor.GetFollowRadius();
                break;
            default:
                break;
        }

        if(!ret)
        {
            if (mLogEnable)
            {
                PrintLog(string.Format("Check Result: NOT in {0}", GetLogicAreaName(mLogicAreaType)));
            }
            return BTDefine.BT_ExcuteResult_Failed;
        }

        if (mLogEnable)
        {
            PrintLog(string.Format("Check Result: in {0}", GetLogicAreaName(mLogicAreaType)));
        }
        return BTDefine.BT_ExcuteResult_Succeed;
    }
}
