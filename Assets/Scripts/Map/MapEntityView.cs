using UnityEngine;

[RequireComponent(typeof(GameColliderView))]
public abstract class MapEntityView : GameEntityView
{
    // collider 组件
    protected GameColliderView mColliderView;

    // collider 数据
    protected GameCollider2D mCollider;

    private void InitializeCollider()
    {
        mColliderView = GetComponent<GameColliderView>();
        GameColliderData2D colliderData = new GameColliderData2D(mColliderView.size, mColliderView.offset);
        mCollider = new GameCollider2D(colliderData, null, transform.position);
        GameColliderCenter.Ins.RegisterGameCollider(mCollider);
    }

    public virtual void Initialize()
    {
        InitializeCollider();
    }

}

