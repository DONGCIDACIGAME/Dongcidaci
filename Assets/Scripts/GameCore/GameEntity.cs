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
    /// 缩放比例
    /// </summary>
    protected Vector3 mLocalScale;

    /// <summary>
    /// 用于更新GameEntity相关的基础表现逻辑，对外，对上层都不开放
    /// </summary>
    private GameEntityView mEntityView;

    protected void BindGameEntityView(GameEntityView view)
    {
        this.mEntityView = view;
    }

    /// <summary>
    /// 更具此时表现层的数据,同步逻辑层数据
    /// </summary>
    public void SyncAllTansformInfoFromView()
    {
        if (mEntityView !=null)
        {
            //mPosition = mEntityView.ViewTransform.position;
            //mRotation = mEntityView.ViewTransform.eulerAngles;
            //mLocalScale = mEntityView.ViewTransform.localScale;
            SetPosition(mEntityView.ViewTransform.position);
            SetRotation(mEntityView.ViewTransform.eulerAngles);
            SetScale(mEntityView.ViewTransform.localScale);
        }
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
    //public virtual Vector2 GetWorld2DPosition()
    //{
    //    if (mEntityView != null)
    //    {
    //        return new Vector2(mPosition.x, mPosition.z);
    //    }
    //    else
    //    {
    //        return Vector2.zero;
    //    }
    //}

    /// <summary>
    /// 获取这个实体在世界坐标下绕Y轴旋转的角度
    /// Added by Weng in 2023/06/18
    /// </summary>
    /// <returns></returns>
    //public virtual float GetWorldRotationByYAxis()
    //{
    //    if (mEntityView != null)
    //    {
    //        return mRotation.y;
    //    }
    //    else
    //    {
    //        return 0;
    //    }
    //}

    /// <summary>
    /// 获取这个实体在父级下的缩放
    /// Added by Weng in 2023/06/18
    /// </summary>
    /// <returns></returns>
    public virtual Vector3 GetLocalScale()
    {
        if (mEntityView != null)
        {
            return mLocalScale;
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

    public virtual void SetPosition(Vector3 position)
    {
        mPosition = position;

        if(mEntityView != null)
        {
            mEntityView.SetPosition(position);
        }
    }

    public virtual void SetRotation(Vector3 rotation)
    {
        mRotation = rotation;

        if (mEntityView != null)
        {
            mEntityView.SetRotation(rotation);
        }
    }

    public virtual void SetScale(Vector3 scale)
    {
        mLocalScale = scale;
        if (mEntityView != null)
        {
            mEntityView.SetScale(scale);
        }
    }

    public abstract int GetEntityType();




}
