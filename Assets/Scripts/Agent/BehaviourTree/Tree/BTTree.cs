using System.Collections.Generic;

public abstract class BTTree : BTNode
{
    protected BTNode mChildNode;

    public void SetChildNode(BTNode childNode)
    {
        if(childNode == null)
        {
            Log.Error(LogLevel.Normal, "BTTree SetChildNode Error, child node is null!");
            return;
        }

        mChildNode = childNode;
        childNode.SetParentNode(this);
    }

    public override void Initialize(Agent excutor, Dictionary<string, object> context)
    {
        base.Initialize(excutor, context);

        if (mChildNode != null)
            mChildNode.Initialize(excutor, context);
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        if (mChildNode == null)
        {
            info = "树节点必须有子节点!";
            return false;
        }

        return true;
    }

    public BTNode GetChildNode()
    {
        return mChildNode;
    }

    public void RemoveChildNode()
    {
        if(mChildNode != null)
            mChildNode.Dispose();

        mChildNode = null;
    }

    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Tree;
    }

    public override int Excute(float deltaTime)
    {
        if (mChildNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        return mChildNode.Excute(deltaTime);
    }

    public override void Reset()
    {
        if (mChildNode == null)
            return;

        mChildNode.Reset();
    }


    protected override void CustomDispose()
    {
        if (mChildNode == null)
            return;

        mChildNode.Dispose();
    }
}