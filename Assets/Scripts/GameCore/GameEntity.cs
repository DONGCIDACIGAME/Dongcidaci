using GameEngine;
using UnityEngine;

public abstract class GameEntity : Entity
{
    /// <summary>
    /// 旋转
    /// </summary>
    protected Vector3 mRotation;
    /// <summary>
    /// 位置
    /// </summary>
    protected Vector3 mPosition;

    /// <summary>
    /// 用于更新GameEntity相关的基础表现逻辑，对外，对上层都不开放
    /// </summary>
    private GameEntityView mEntityView;

    protected void BindGameEntityView(GameEntityView view)
    {
        this.mEntityView = view;
    }

    public virtual Vector3 GetPosition()
    {
        return mPosition;
    }

    public virtual Vector3 GetRotation()
    {
        return mRotation;
    }

    public void SetPosition(Vector3 position)
    {
        mPosition = position;

        if(mEntityView != null)
        {
            mEntityView.SetPosition(position);
        }
    }

    public void SetRotation(Vector3 rotation)
    {
        mRotation = rotation;

        if (mEntityView != null)
        {
            mEntityView.SetRotation(rotation);
        }
    }
    public abstract int GetEntityType();
}
