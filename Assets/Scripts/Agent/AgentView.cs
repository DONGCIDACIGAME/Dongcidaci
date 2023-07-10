using UnityEngine;

public abstract class AgentView : MapEntityViewWithCollider
{
    // Added by Weng 0710
    #region FX CARRY POINT DEFINE

    /// <summary>
    /// 受击效果的特效挂载点
    /// </summary>
    [SerializeField] private GameObject _hitFXCarryNode;

    /// <summary>
    /// 武器特效的挂载点
    /// </summary>
    [SerializeField] private GameObject _weaponFXCarryNode;


    public GameObject GetFXCarryNode(FXCarryNodeDefine carryNodeType)
    {
        switch (carryNodeType)
        {
            case FXCarryNodeDefine.AgentHitNode:
                return _hitFXCarryNode;
            case FXCarryNodeDefine.AgentWeaponNode:
                return _weaponFXCarryNode;
            default:
                return null;
        }
    }



    #endregion




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



    public virtual void OnMyUpdate(Agent agt,float deltaTime)
    {
    }

    public virtual void OnMyLateUpdate(Agent agt, float deltaTime)
    {

    }

    public override void Dispose()
    {
        base.Dispose();
    }

}
