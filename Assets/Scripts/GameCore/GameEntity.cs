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

    /// <summary>
    /// 获取这个实体的2D位置信息,世界坐标
    /// Added by Weng in 2023/06/18
    /// </summary>
    /// <returns></returns>
    public virtual Vector2 GetWorld2DPosition()
    {
        if (mEntityView != null)
        {
            return new Vector2(mEntityView.ViewTransform.position.x, mEntityView.ViewTransform.position.z);
        }
        else
        {
            return Vector2.zero;
        }
    }

    /// <summary>
    /// 获取这个实体在世界坐标下绕Y轴旋转的角度
    /// Added by Weng in 2023/06/18
    /// </summary>
    /// <returns></returns>
    public virtual float GetWorldRotationByYAxis()
    {
        if (mEntityView != null)
        {
            return mEntityView.ViewTransform.eulerAngles.y;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 获取这个实体在世界坐标下的缩放
    /// Added by Weng in 2023/06/18
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetWorldScale()
    {
        if (mEntityView != null)
        {
            return mEntityView.ViewTransform.lossyScale;
        }
        else
        {
            return Vector3.one;
        }
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
