public abstract class BTTree : BTNode
{
    //public override void AddChildNode(BTNode node)
    //{
    //    if(node == null)
    //    {
    //        Log.Error(LogLevel.Normal, "[0] AddChildNode Failed, child node is null", NodeName);
    //        return;
    //    }

    //    mChildNodes.Clear();
    //    mChildNodes.Add(node);
    //    node.SetParentNode(this);
    //}

    public void SetChildNode(BTNode childNode)
    {
        if (childNode == null)
        {
            Log.Error(LogLevel.Normal, "BTTree SetChildNode Error, child node is null!");
            return;
        }

        AddChildNode(childNode);
    }

    public override bool BTNodeDataCheck(ref string info)
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
        {
            info = "树节点必须有子节点!";
            return false;
        }

        return true;
    }

    public BTNode GetChildNode()
    {
        if (mChildNodes.Count == 0)
            return null;

        return mChildNodes[0];
    }

    //public void UnpackChildNode(BTNode node)
    //{
    //    if(mChildNodes.Contains(node))
    //    {
    //        mChildNodes.Remove(node);
    //    }
    //}

    //public override void UnpackAllChilds()
    //{
    //    BTNode childNode = GetChildNode();
    //    if (childNode != null)
    //        childNode.UnpackChilds();

    //    mChildNodes.Clear();
    //}

    ///// <summary>
    ///// 删除子节点
    ///// 使用该方法会递归释放所有子节点
    ///// </summary>
    //public override void RemoveChildNode(BTNode node)
    //{
    //    UnpackChildNode(node);

    //    if(node != null)
    //    {
    //        node.Dispose();
    //    }
    //}

    public override int GetNodeType()
    {
        return BTDefine.BT_Node_Type_Tree;
    }

    public override int Excute(float deltaTime)
    {
        BTNode childNode = GetChildNode();
        if (childNode == null)
            return BTDefine.BT_ExcuteResult_Failed;

        int ret = childNode.Excute(deltaTime);
        if(ret == BTDefine.BT_ExcuteResult_Succeed)
        {
            childNode.Reset();
        }

        return ret;
    }


}