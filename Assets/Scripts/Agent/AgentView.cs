public class AgentView : MapEntityViewWithCollider
{
    /// <summary>
    /// 角色的视野范围配置 added by weng 0629
    /// </summary>
    protected GameVisionView mVisionView;

    /// <summary>
    /// 获取视野形状 added by weng
    /// </summary>
    /// <returns></returns>
    public IConvex2DShape GetVisionShape()
    {
        mVisionView = GetComponent<GameVisionView>();
        if (mVisionView == null) return null;
        var shape = GameColliderHelper.GetRegularShapeWith(mVisionView.shapeType,mVisionView.offset,mVisionView.size);
        shape.AnchorPos = mVisionView.transform.position;
        shape.AnchorAngle = mVisionView.transform.eulerAngles.y;
        return shape;
    }

    public override void Dispose()
    {
        base.Dispose();


    }

}
