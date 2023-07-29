using UnityEngine;

public abstract class AgentView : MapEntityViewWithCollider
{
    private void Start()
    {
        // 查找所有的特效的挂载点
        // Added by weng 0711
        if (_HIT_FX == null)
        {
            var retT = CommanHelper.FindChildNode(this.transform, "_HIT_FX");
            if (retT != null) _HIT_FX = retT.gameObject;
        }

        if (_FRONT_ATK_FX == null)
        {
            var retT = CommanHelper.FindChildNode(this.transform, "_FRONT_ATK_FX");
            if (retT != null) _FRONT_ATK_FX = retT.gameObject;
        }

        if (_WEAPON_FX == null)
        {
            var retT = CommanHelper.FindChildNode(this.transform, "_WEAPON_FX");
            if (retT != null) _WEAPON_FX = retT.gameObject;
        }

    }

    // Added by Weng 0710
    #region FX CARRY POINT DEFINE

    /// <summary>
    /// 受击效果的特效挂载点
    /// </summary>
    [SerializeField] private GameObject _HIT_FX;

    /// <summary>
    /// 前方攻击特效的挂载点
    /// </summary>
    [SerializeField] private GameObject _FRONT_ATK_FX;

    /// <summary>
    /// 武器特效的挂载点
    /// </summary>
    [SerializeField] private GameObject _WEAPON_FX;


    public GameObject GetFXCarryNode(string nodeName)
    {
        if (nodeName == "_HIT_FX") return _HIT_FX;
        if (nodeName == "_FRONT_ATK_FX") return _FRONT_ATK_FX;
        if (nodeName == "_WEAPON_FX") return _WEAPON_FX;

        // no defined
        var retT = CommanHelper.FindChildNode(this.transform, nodeName);
        if (retT != null)
        {
            return retT.gameObject;
        }
        else
        {
            Log.Error(LogLevel.Critical, "GetFXCarryNode in AgentView error, no fx node called {0}",nodeName);
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
