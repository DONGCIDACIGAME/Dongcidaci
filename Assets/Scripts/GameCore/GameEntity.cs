using GameEngine;
using UnityEngine;

public abstract class GameEntity : Entity
{
    // 旋转
    protected Vector3 mRotation;
    // 位置
    protected Vector3 mPosition;

    // 主体view
    protected GameEntityView mView;

    public void BindGameEntityView(GameEntityView view)
    {
        this.mView = view;
    }

    public virtual Vector3 GetPosition()
    {
        return mPosition;
    }

    public virtual Vector3 GetRotation()
    {
        return mRotation.normalized;
    }

    public void SetPosition(Vector3 position)
    {
        mPosition = position;

        if(mView != null)
        {
            mView.SetPosition(position);
        }
    }

    public void SetRotation(Vector3 rotation)
    {
        mRotation = rotation;

        if (mView != null)
        {
            mView.SetRotation(rotation);
        }
    }
}
